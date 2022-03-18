using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

public class BulletMoveSystem : SystemBase
{
    protected override void OnUpdate()
    {
        float deltaTime = Time.DeltaTime;

        Entities.WithAny<BulletTag>().ForEach((ref Translation translation, in Rotation rotation, in MoveData moveData) =>
        {
            float3 normallizedDir = math.forward(rotation.Value);
            translation.Value += normallizedDir * moveData.moveSpeed * deltaTime;

        }).ScheduleParallel();
    }
}

