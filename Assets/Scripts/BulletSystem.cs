using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;

public class BulletSystem : JobComponentSystem
{
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var jobHandle = Entities
            .WithName(nameof(BulletSystem))
            .ForEach((ref PhysicsVelocity physicsVelocity, ref Rotation rotation, ref BulletData bulletData) =>
            {
                physicsVelocity.Angular = float3.zero;
                physicsVelocity.Linear = bulletData.Speed * math.forward(rotation.Value);
            })
            .Schedule(inputDeps);

        jobHandle.Complete();
        
        return jobHandle;
    }
}