using Unity.Entities;
using Unity.Mathematics;

//[GenerateAuthoringComponent]
public struct BulletData : IComponentData
{
    public Entity bulletPrefab;
    public float3 bulletOffset;
    public float bulletVelocity;
}
