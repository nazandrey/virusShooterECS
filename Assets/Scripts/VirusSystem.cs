using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using Random = UnityEngine.Random;

[UpdateAfter(typeof(BulletCollisionEventSystem))]
public class VirusSystem : JobComponentSystem
{
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        Entities
            .WithName(nameof(VirusSystem))
            .WithoutBurst()
            .WithStructuralChanges()
            .ForEach((ref VirusData virusData, ref Translation translation, in Entity entity) =>
            {
                if (!virusData.IsAlive)
                {
                    for (var i = 0; i < 10; i++)
                    {
                        var whiteBloodCell = EntityManager.Instantiate(ECSSpawner.WhiteBloodCell);
                        EntityManager.SetComponentData(whiteBloodCell, new Translation
                        {
                            Value = translation.Value + new float3(Random.insideUnitSphere * 2)
                        });
                        EntityManager.SetComponentData(whiteBloodCell, new PhysicsVelocity
                        {
                            Linear = new float3(Random.insideUnitSphere * 20)
                        });
                    }

                    EntityManager.DestroyEntity(entity);
                }
            }).Run();

        return inputDeps;
    }
}
