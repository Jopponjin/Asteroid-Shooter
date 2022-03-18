using UnityEngine;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using static Unity.Mathematics.math;
using Unity.Physics;
using Unity.Physics.Systems;


public class DeathOnCollison : JobComponentSystem
{

    private BuildPhysicsWorld buildPhysicsWorld;
    private StepPhysicsWorld stepPhysicsWorld;

    private EndSimulationEntityCommandBufferSystem commandBufferSystem;

    protected override void OnCreate()
    {
        buildPhysicsWorld = World.GetOrCreateSystem<BuildPhysicsWorld>();
        stepPhysicsWorld = World.GetOrCreateSystem<StepPhysicsWorld>();

        commandBufferSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
    }

    [BurstCompile]
    struct DeathOnCollisionSystemJob : ITriggerEventsJob
    {
        [ReadOnly] public ComponentDataFromEntity<RockTag> allRocks;
        [ReadOnly] public ComponentDataFromEntity<BulletTag> allBullets;
        [ReadOnly] public ComponentDataFromEntity<PlayerTag> allShips;

        public EntityCommandBuffer entityCommandBuffer;

        public void Execute(TriggerEvent triggerEvent)
        {
            Entity entityA = triggerEvent.EntityA;
            Entity entityB = triggerEvent.EntityB;

            bool entityIsRock = allRocks.HasComponent(entityA);
            bool entityIsShip = allShips.HasComponent(entityB);
            bool entityIsBullet = allBullets.HasComponent(entityB);

            if (allRocks.HasComponent(entityA) && allRocks.HasComponent(entityB))
            {
                entityCommandBuffer.DestroyEntity(entityA);
                entityCommandBuffer.DestroyEntity(entityB);
            }
            if (entityIsBullet && entityIsRock)
            {
                entityCommandBuffer.DestroyEntity(entityA);
                entityCommandBuffer.DestroyEntity(entityB);
            }

            if (allRocks.HasComponent(entityA) && allShips.HasComponent(entityB))
            {
                Debug.Log("DeathOnCollison.cs: Destroyed Entity A.");
                entityCommandBuffer.DestroyEntity(entityB);
            }
            else if (allShips.HasComponent(entityA) && allRocks.HasComponent(entityB))
            {
                Debug.Log("DeathOnCollison.cs: Destroyed Entity B.");
                entityCommandBuffer.DestroyEntity(entityA);
            }
        }
    }

    protected override JobHandle OnUpdate(JobHandle inputDependencies)
    {
        var job = new DeathOnCollisionSystemJob();
        job.allBullets = GetComponentDataFromEntity<BulletTag>(true);
        job.allRocks = GetComponentDataFromEntity<RockTag>(true);
        job.allShips = GetComponentDataFromEntity<PlayerTag>(true);
        job.entityCommandBuffer = commandBufferSystem.CreateCommandBuffer();

        JobHandle jobHandle = job.Schedule(stepPhysicsWorld.Simulation, ref buildPhysicsWorld.PhysicsWorld,inputDependencies);

        commandBufferSystem.AddJobHandleForProducer(jobHandle);

        return jobHandle;
    }
}
