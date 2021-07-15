using System;
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
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private int spawnQuantity;
    [SerializeField] private int bulletQuantity;
    [SerializeField] private Transform player;

    private BlobAssetStore _store;
    private Entity bullet;

    private void Start()
    {
        _store = new BlobAssetStore();
        EntityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        var settings = GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, _store);
        var virus = GameObjectConversionUtility.ConvertGameObjectHierarchy(virusPrefab, settings);
        var redBloodCell = GameObjectConversionUtility.ConvertGameObjectHierarchy(redBloodCellPrefab, settings);
        bullet = GameObjectConversionUtility.ConvertGameObjectHierarchy(bulletPrefab, settings);

        Spawn(virus);
        Spawn(redBloodCell);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            for (var i = 0; i < bulletQuantity; i++)
            {
                var instance = EntityManager.Instantiate(bullet);
                var startPosition = player.position + Random.insideUnitSphere * 2;
                EntityManager.SetComponentData(instance, new Translation {Value = startPosition});
                EntityManager.SetComponentData(instance, new Rotation {Value = player.rotation});
            }
        }
    }

    private void OnDestroy()
    {
        _store.Dispose();
    }

    private void Spawn(Entity virus)
    {
        for (int i = 0; i < bulletQuantity; i++)
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
}
