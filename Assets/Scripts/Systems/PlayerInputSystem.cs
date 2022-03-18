using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using System;

public class PlayerInputSystem : SystemBase
{
    protected override void OnUpdate()
    {
        float3 moveInput = new float3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));

        bool playerShootInput = Input.GetMouseButton(0);

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition); 

        Entities.ForEach((ref InputData inputData) => { inputData.moveDirection = moveInput; inputData.isShooting = playerShootInput; inputData.mousePosition = mousePos; }).Run();
    }
}