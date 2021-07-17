using System;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using Random = UnityEngine.Random;

public class ECSSpawner : MonoBehaviour
{
    public static Entity WhiteBloodCell;
    
    [SerializeField] private GameObject virusPrefab;
    [SerializeField] private GameObject redBloodCellPrefab;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private GameObject whiteBloodCellPrefab;
    [SerializeField] private int spawnQuantity;
    [SerializeField] private int bulletQuantity;
    [SerializeField] private Transform player;

    private EntityManager _entityManager;
    private BlobAssetStore _store;
    private Entity _bullet;

    private void Start()
    {
        _store = new BlobAssetStore();
        _entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        
        var settings = GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, _store);
        
        var virus = GameObjectConversionUtility.ConvertGameObjectHierarchy(virusPrefab, settings);
        var redBloodCell = GameObjectConversionUtility.ConvertGameObjectHierarchy(redBloodCellPrefab, settings);
        _bullet = GameObjectConversionUtility.ConvertGameObjectHierarchy(bulletPrefab, settings);
        WhiteBloodCell = GameObjectConversionUtility.ConvertGameObjectHierarchy(whiteBloodCellPrefab, settings);

        Spawn(virus);
        Spawn(redBloodCell);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            for (var i = 0; i < bulletQuantity; i++)
            {
                var instance = _entityManager.Instantiate(_bullet);
                var startPosition = player.position + Random.insideUnitSphere * 2;
                _entityManager.SetComponentData(instance, new Translation {Value = startPosition});
                _entityManager.SetComponentData(instance, new Rotation {Value = player.rotation});
            }
        }
    }

    private void OnDestroy()
    {
        _store.Dispose();
    }

    private void Spawn(Entity virus)
    {
        for (int i = 0; i < spawnQuantity; i++)
        {
            var instance = _entityManager.Instantiate(virus);

            var x = Random.Range(-50, 50);
            var y = Random.Range(-50, 50);
            var z = Random.Range(-50, 50);

            var position = new float3(x, y, z);
            var speed = Random.Range(1f, 3f);
            _entityManager.SetComponentData(instance, new Translation {Value = position});
            _entityManager.SetComponentData(instance, new WanderData {Speed = speed});
        }
    }
}
