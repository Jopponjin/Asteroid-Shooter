using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

[UpdateBefore(typeof(TransformSystemGroup))]
public class ObjectDestroySystem : SystemBase
{
    private EndSimulationEntityCommandBufferSystem commandBufferSystem;

    protected override void OnCreate()
    {
        commandBufferSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
    }

    protected override void OnUpdate()
    {
        EntityCommandBuffer entityCommandBuffer = commandBufferSystem.CreateCommandBuffer();

        float deltaTIme = Time.DeltaTime;

        Entities.ForEach((Entity entity, ref LifeTimeData timeData) =>
        {
            if (timeData.currentTime > timeData.maxTime)
            {
                entityCommandBuffer.DestroyEntity(entity);
            }
            else
            {
                timeData.currentTime += deltaTIme;
            }
        }).Schedule();

        commandBufferSystem.AddJobHandleForProducer(Dependency);
    }
}
