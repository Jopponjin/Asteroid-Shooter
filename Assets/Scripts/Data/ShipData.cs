using Unity.Entities;
using Unity.Mathematics;

[GenerateAuthoringComponent]
public struct ShipData : IComponentData
{
    public Entity shipPrefab;
    public float3 shipDirection;
    public float moveSpeed;
    public float shotSpeed;
}