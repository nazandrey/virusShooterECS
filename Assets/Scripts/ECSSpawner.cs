using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using Random = UnityEngine.Random;

public class ECSSpawner : MonoBehaviour
{
    public static EntityManager EntityManager;

    [SerializeField] private GameObject virusPrefab;
    [SerializeField] private GameObject redBloodCellPrefab;
    [SerializeField] private int quantity;

    private BlobAssetStore _store;

    private void Start()
    {
        _store = new BlobAssetStore();
        EntityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        var settings = GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, _store);
        var virus = GameObjectConversionUtility.ConvertGameObjectHierarchy(virusPrefab, settings);
        var redBloodCell = GameObjectConversionUtility.ConvertGameObjectHierarchy(redBloodCellPrefab, settings);

        Spawn(virus);
        Spawn(redBloodCell);
    }

    private void Spawn(Entity virus)
    {
        for (int i = 0; i < quantity; i++)
        {
            var instance = EntityManager.Instantiate(virus);

            var x = Random.Range(-50, 50);
            var y = Random.Range(-50, 50);
            var z = Random.Range(-50, 50);

            var position = new float3(x, y, z);
            var speed = Random.Range(1f, 3f);
            EntityManager.SetComponentData(instance, new Translation {Value = position});
            EntityManager.SetComponentData(instance, new WanderData {Speed = speed});
        }
    }

    private void OnDestroy()
    {
        _store.Dispose();
    }
}
