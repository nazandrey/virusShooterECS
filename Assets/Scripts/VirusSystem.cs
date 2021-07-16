using Unity.Entities;
using Unity.Jobs;

[UpdateAfter(typeof(BulletCollisionEventSystem))]
public class VirusSystem : JobComponentSystem
{
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        Entities
            .WithName(nameof(VirusSystem))
            .WithoutBurst()
            .WithStructuralChanges()
            .ForEach((ref VirusData virusData, in Entity entity) =>
            {
                if (!virusData.IsAlive)
                {
                    EntityManager.DestroyEntity(entity);
                }
            }).Run();

        return inputDeps;
    }
}
