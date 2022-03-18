using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;

[GenerateAuthoringComponent]
public struct LifeTimeData : IComponentData
{
    public float currentTime;
    public float maxTime;
}