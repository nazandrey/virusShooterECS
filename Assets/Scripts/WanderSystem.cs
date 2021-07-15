using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;

public class WanderSystem : JobComponentSystem
{
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var elapsedTime = Time.ElapsedTime;

        var jobHandle = Entities
            .WithName(nameof(WanderSystem))
            .ForEach((ref PhysicsVelocity physicsVelocity, ref WanderData wanderData) =>
            {
                var uniqueRandomFactor = wanderData.Speed;
                var sin = (float) math.sin(elapsedTime + uniqueRandomFactor) * wanderData.Speed;
                var cos = (float) math.cos(elapsedTime + uniqueRandomFactor) * wanderData.Speed;
                
                physicsVelocity.Linear = new float3(sin, cos, sin);
            })
            .Schedule(inputDeps);

        jobHandle.Complete();
        
        return jobHandle;
    }
}
