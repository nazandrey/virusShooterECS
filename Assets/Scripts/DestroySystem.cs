using Unity.Entities;
using Unity.Jobs;

public class DestroySystem : JobComponentSystem
{
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var deltaTime = Time.DeltaTime;
        
        Entities
            .WithName(nameof(DestroySystem))
            .WithoutBurst()
            .WithStructuralChanges()
            .ForEach((Entity entity, ref DestroyData bulletData) =>
            {
                bulletData.DelayBeforeDestroy -= deltaTime;
                if (bulletData.DelayBeforeDestroy <= 0)
                    EntityManager.DestroyEntity(entity);
            })
            .Run();
        
        return inputDeps;
    }
}