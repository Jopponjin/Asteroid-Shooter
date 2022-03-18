using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

[GenerateAuthoringComponent]
public struct InputData : IComponentData
{
    public float3 moveDirection;
    public float3 mousePosition;

    public float aimSensitivity;
    public float moveSpeed;
    public bool isShooting;
}
