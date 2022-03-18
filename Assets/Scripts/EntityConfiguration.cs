using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Rendering;
using Unity.Mathematics;
using UnityEngine.Events;
using Unity.Collections;

public class EntityConfiguration : MonoBehaviour, IDeclareReferencedPrefabs, IConvertGameObjectToEntity
{
    public GameObject rockPrefab1;
    public GameObject rockPrefab2;
    BlobAssetStore blob;
    
    EntityManager entityManager;
    public Entity enemyEntityPrefab;
    [Space]
    NativeArray<Entity> enemyArray = new NativeArray<Entity>(100, Allocator.Temp);

    public int spawnCount = 100;
    public float entityLifeTime = 15f;
    [Space]
    public float waveTime;
    public float currentWaveDuration = 10f;
    public float spawnInterval = 1f;
    public bool hasLastRockSpawned = false;

    [SerializeField]int spawnIndex = 0;

    public enum RockType
    {
        small,
        medium
    }
    public RockType rockType;

    public void Convert(Entity entity, EntityManager m_entitytManager, GameObjectConversionSystem conversionSystem)
    {
        var bulletData = default(BulletData);

        

        m_entitytManager.AddComponentData(entity, bulletData);
    }

    public void DeclareReferencedPrefabs(List<GameObject> referencedPrefabs)
    {
        referencedPrefabs.Add(rockPrefab1);
    }

    void Start()
    {
        entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

        blob = new BlobAssetStore();
        rockType = RockType.small;

       // if (rockPrefab1) Debug.Log("rockPrefab1 is true");

        World rockWorld = new World("AsteroidSubScene", WorldFlags.Game);

        GameObjectConversionSettings settings = GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, blob);
        enemyEntityPrefab = GameObjectConversionUtility.ConvertGameObjectHierarchy(rockPrefab1, settings);

        //if (enemyEntityPrefab != null) Debug.Log("enemyEntityPrefab is NOT Null");

        enemyArray = new NativeArray<Entity>(spawnCount, Allocator.Persistent);

        SpawnWave();
    }

    IEnumerator SpawnEntity()
    {
        //Debug.Log("StartCoroutine(SpawnEntity()) called!");
        yield return new WaitForSeconds(0.5f);

        if (spawnIndex < enemyArray.Length)
        {
            //Debug.Log("spawnIndex is NOT the same lenght as enemyArray.");
            StartCoroutine(SpawnEntity());
            SpawnEntity(spawnIndex);
        }
        if (spawnIndex == enemyArray.Length)
        {
            //Debug.Log("spawnIndex IS the same lenght as enemyArray.");
            enemyArray.Dispose();
            StopAllCoroutines();
            spawnCount += 10;
            if (spawnInterval > 0.05f)
            {
                spawnInterval -= 0.1f;
            }
            enemyArray = new NativeArray<Entity>(spawnCount, Allocator.Persistent);
            spawnIndex = 0;
            SpawnWave();
            
        }
    }

    void SpawnEntity(int entityIndex)
    {
        //Debug.Log("SpawnEntity() called");
        entityManager.SetEnabled(enemyArray[entityIndex], true);
        spawnIndex++;
    }

    void SpawnWave()
    {
        //Debug.Log("SpawnWave() called");
        Vector3[] rockPositions = new Vector3[enemyArray.Length];

        for (int i = 0; i < enemyArray.Length; i++)
        {
            float theta = i * 2 * Mathf.PI / enemyArray.Length;
            float x = math.sin(theta) * 200f;
            float z = math.cos(theta) * 200f;

            x += UnityEngine.Random.Range(-5f, 5f);
            z += UnityEngine.Random.Range(-5f, 5f);

            rockPositions[i] = new Vector3(x, 0, z);
            //Debug.Log("X spawn point: " + x + " Z Spawn Point: " + z);
        }

        for (int i = 0; i < enemyArray.Length; i++)
        {
            enemyArray[i] = entityManager.Instantiate(enemyEntityPrefab);

            Vector3 rockSpawnPos = rockPositions[UnityEngine.Random.Range(0, enemyArray.Length)];

            float targetZ = UnityEngine.Random.Range(-50f, 50f);
            float targetX = UnityEngine.Random.Range(-50f, 50f);
            Vector3 rockTarget = new Vector3(targetX, 0, targetZ);

            entityManager.SetComponentData(enemyArray[i], new Translation { Value = rockSpawnPos });

            Translation rockPosition = entityManager.GetComponentData<Translation>(enemyArray[i]);

            float3 newRockDirection = new float3(rockPosition.Value.x, 0, rockPosition.Value.z) - new float3(0,0,0);

            newRockDirection += (float3)rockTarget;

            entityManager.SetComponentData(enemyArray[i], new MoveData { moveDircetion = newRockDirection, moveSpeed = UnityEngine.Random.Range(5f, 60f) });

            entityManager.SetComponentData(enemyArray[i], new LifeTimeData { maxTime = entityLifeTime });

            entityManager.SetComponentData(enemyArray[i], new RotationData { turnSpeed = UnityEngine.Random.Range(-2f, 2f)});

            entityManager.SetEnabled(enemyArray[i], false);
        }

        StartCoroutine(SpawnEntity());
    }

    private void OnDestroy()
    {
        blob.Dispose();
        enemyArray.Dispose();
    }
}
