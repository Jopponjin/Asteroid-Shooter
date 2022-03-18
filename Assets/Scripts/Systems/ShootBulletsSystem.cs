using Unity.Entities;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using Unity.Burst;


public class ShootBulletsSystem : MonoBehaviour
{
    private BeginInitializationEntityCommandBufferSystem beginInitECB;
    BlobAssetStore blob;

    public GameObject prefabGameObject;
    public Translation shipDataPos;
    public Rotation shipDataRot;

    Entity entityPrefab;
    Entity shipEntity;

    EntityManager entityManager;

    public float spawnInterval = 1.5f;
    public float currentTime;

    void Start()
    {
        blob = new BlobAssetStore();
        var settings = GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, blob);

        entityPrefab = GameObjectConversionUtility.ConvertGameObjectHierarchy(prefabGameObject, settings);
        entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

        var entityArray = entityManager.GetAllEntities(Allocator.Temp);

        for (int i = 0; i < entityArray.Length; i++)
        {
            if (entityManager.HasComponent(entityArray[i], typeof(PlayerTag)))
            {
                shipEntity = entityArray[i];
            }
        }
    }

    private void Update()
    {
        if (Input.GetMouseButton(0) && entityManager.HasComponent<Translation>(shipEntity))
        {
            if (currentTime >= spawnInterval)
            {

                var instance = entityManager.Instantiate(entityPrefab);
                var mousePosRaw = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector3 mousePos = new Vector3(mousePosRaw.x, 0, mousePosRaw.z);

                shipDataPos = entityManager.GetComponentData<Translation>(shipEntity);
                shipDataRot = entityManager.GetComponentData<Rotation>(shipEntity);

                var position = new Vector3(shipDataPos.Value.x, 0, shipDataPos.Value.z);

                entityManager.SetComponentData(instance, new Translation { Value = position });

                Quaternion bulletRotation = new Quaternion();
                Vector3 lookDir = mousePos - position;
                float angle = Mathf.Atan2(lookDir.x, lookDir.z) * Mathf.Rad2Deg - 0f;

                bulletRotation = Quaternion.AngleAxis(angle, Vector3.up);

                entityManager.SetComponentData(instance, new Rotation { Value = bulletRotation });
                entityManager.SetComponentData(instance, new MoveData { moveDircetion = new float3(1,0,0), moveSpeed = 300f });


                currentTime = 0;
            }
            else
            {
                currentTime += Time.deltaTime;
            }
        }
    }

    private void OnDestroy()
    {
        blob.Dispose();
    }

}