using Unity.Entities;
using Unity.Mathematics;

[GenerateAuthoringComponent]
struct GameSettingsData : IComponentData
{
    public Entity bulletPrefab;
    public float3 bulletStartPosition;
    public float3 bulletOffset;
    public float bulletVelocity;
}
