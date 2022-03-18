using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

public class MoveForwardSystem : SystemBase
{
    protected override void OnUpdate()
    {
        float deltaTime = Time.DeltaTime;

        Entities.WithAny<RockTag>().ForEach((ref Translation translation, in Rotation rotation, in MoveData moveData) =>
        {
            float3 normallizedDir = math.normalizesafe(moveData.moveDircetion);
            translation.Value -= normallizedDir * moveData.moveSpeed * deltaTime;
            //translation.Value -= moveData.moveDircetion * moveData.moveSpeed * deltaTime;

        }).ScheduleParallel();

    }
}