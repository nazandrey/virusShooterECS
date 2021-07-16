using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Physics;
using Unity.Physics.Systems;
using UnityEngine.PlayerLoop;

[UpdateAfter(typeof(EndFramePhysicsSystem))]
public class BulletCollisionEventSystem : JobComponentSystem
{
    private BuildPhysicsWorld _buildPhysicsWorld;
    private StepPhysicsWorld _stepPhysicsWorld;
    
    protected override void OnCreate()
    {
        base.OnCreate();
        _buildPhysicsWorld = World.GetOrCreateSystem<BuildPhysicsWorld>();
        _stepPhysicsWorld = World.GetOrCreateSystem<StepPhysicsWorld>();
    }

    private struct CollisionEventImpulseJob : ICollisionEventsJob
    {
        [ReadOnly] public ComponentDataFromEntity<BulletData> BulletGroup;
        public ComponentDataFromEntity<VirusData> VirusGroup;
        
        public void Execute(CollisionEvent collisionEvent)
        {
            var entitiyA = collisionEvent.Entities.EntityA;
            var entitiyB = collisionEvent.Entities.EntityB;

            var isTargetA = VirusGroup.Exists(entitiyA);
            var isTargetB = VirusGroup.Exists(entitiyB);
            
            var isBulletA = BulletGroup.Exists(entitiyA);
            var isBulletB = BulletGroup.Exists(entitiyB);

            if (isTargetA && isBulletB)
                MarkVirusNotAlive(entitiyA);
            
            if (isTargetB && isBulletA)
                MarkVirusNotAlive(entitiyB);
        }

        private void MarkVirusNotAlive(Entity virusEntity)
        {
            var aliveComponentData = VirusGroup[virusEntity];
            aliveComponentData.IsAlive = false;
            VirusGroup[virusEntity] = aliveComponentData;
        }
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var jobHandle = new CollisionEventImpulseJob
        {
            BulletGroup = GetComponentDataFromEntity<BulletData>(),
            VirusGroup = GetComponentDataFromEntity<VirusData>(),
        }.Schedule(_stepPhysicsWorld.Simulation, ref _buildPhysicsWorld.PhysicsWorld, inputDeps);
        
        jobHandle.Complete();
        
        return jobHandle;
    }
    
}