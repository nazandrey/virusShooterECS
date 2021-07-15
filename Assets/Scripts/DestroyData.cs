using Unity.Entities;

[GenerateAuthoringComponent]
public struct DestroyData : IComponentData
{
    public float DelayBeforeDestroy;
}
