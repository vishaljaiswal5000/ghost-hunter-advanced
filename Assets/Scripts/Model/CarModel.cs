using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarModel
{
    public Transform Target { get; private set; }

    public Vector3 StartPos { get; private set; }

    public Vector3 EndPos { get; private set; }

    public float StartTime { get; private set; }

    public float Duration { get; private set; }

    public KeyCode input { get; private set; }
    public CarModel(Transform targetObject, Vector3 startPos, Vector3 endPos, float startTime, float duration, KeyCode input)
    {
        this.Target = targetObject;
        this.StartPos = startPos;
        this.EndPos = endPos;
        this.StartTime = startTime;
        this.Duration = duration;
        this.input = input;
    }

}
