using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;


[AddComponentMenu("DOTS SpaceShooter/Spawn From Entity")]
public class SpawnFromPrefab : MonoBehaviour, IDeclareReferencedPrefabs, IConvertGameObjectToEntity
{
    public GameObject prefabGameObject1;
    public GameObject prefabGameObject2;

    public void DeclareReferencedPrefabs(List<GameObject> referencedPrefabs)
    {
        referencedPrefabs.Add(prefabGameObject1);
        referencedPrefabs.Add(prefabGameObject2);
    }
    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        var bulletData = new BulletData
        {
            bulletPrefab = conversionSystem.GetPrimaryEntity(prefabGameObject1),
            bulletVelocity = 10f,
            bulletOffset = new float3(0, 0, 3)
        };

        var lifeTimeData = new LifeTimeData
        {
            maxTime = 15f,
            currentTime = 0
        };
        dstManager.AddComponentData(entity, bulletData);
        dstManager.AddComponentData(entity, lifeTimeData);
    }
}
