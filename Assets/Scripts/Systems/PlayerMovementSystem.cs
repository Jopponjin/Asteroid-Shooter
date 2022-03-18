using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class PlayerMovementSystem : SystemBase
{
    protected override void OnUpdate()
    {
        float deltaTime = Time.DeltaTime;

        Entities.ForEach((ref Translation translation, ref Rotation rotation, in InputData inputData) =>
        {
            float3 lookDir = inputData.mousePosition - translation.Value;

            Quaternion targetRotation = Quaternion.LookRotation(new Vector3(lookDir.x, 0, lookDir.z));
            rotation.Value = targetRotation;

            float3 normallizedDir = math.normalizesafe(inputData.moveDirection);
            translation.Value += normallizedDir * inputData.moveSpeed * deltaTime;

        }).Run();
    }
}
