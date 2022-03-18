using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class RotationSystem : SystemBase
{
    protected override void OnUpdate()
    {
        float deltaTime = Time.DeltaTime;

        Entities.ForEach((ref Rotation rotation, in RotationData spinData) =>
            {
                quaternion normalizedRot = math.normalize(rotation.Value);
                quaternion angleToRotate = quaternion.AxisAngle(math.up(), spinData.turnSpeed * deltaTime);

                rotation.Value = math.mul(normalizedRot, angleToRotate);

            }).ScheduleParallel();
    }
}
