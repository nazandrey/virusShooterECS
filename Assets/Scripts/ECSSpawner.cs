﻿using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using Random = UnityEngine.Random;

public class ECSSpawner : MonoBehaviour
{
    public static EntityManager EntityManager;

    [SerializeField] private GameObject virusPrefab;
    [SerializeField] private int quantity;

    private BlobAssetStore _store;

    private void Start()
    {
        _store = new BlobAssetStore();
        EntityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        var settings = GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, _store);
        var virus = GameObjectConversionUtility.ConvertGameObjectHierarchy(virusPrefab, settings);

        for (int i = 0; i < quantity; i++)
        {
            var instance = EntityManager.Instantiate(virus);
            
            var x = Random.Range(-50, 50);
            var y = Random.Range(-50, 50);
            var z = Random.Range(-50, 50);

            var position = new float3(x, y, z);
            EntityManager.SetComponentData(instance, new Translation { Value = position });
        }
    }

    private void OnDestroy()
    {
        _store.Dispose();
    }
}