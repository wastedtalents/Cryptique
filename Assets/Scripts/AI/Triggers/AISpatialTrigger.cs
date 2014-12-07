using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;

public class AISpatialTrigger : AITrigger
{
    [Tooltip("Sets an offset from the centre")]
    public Vector2 offset;

    [Tooltip("The distance that the object needs to be within")]
    public float magnitude;

    private Transform foundObject; //we will fire this.
    private float sqrMagnitude;

    public List<Transform> trackedObjects;
    public string trackedObjectTag;

    public override AITriggerType TriggerType
    {
        get
        {
            return AITriggerType.Spatial;
        }
    }

    void Awake()
    {
        // if objects is null then find all of the objects using the objectTag
        if (!string.IsNullOrEmpty(trackedObjectTag) && (trackedObjects == null || trackedObjects.Count == 0))
        {
            var gameObjects = GameObject.FindGameObjectsWithTag(trackedObjectTag);
            trackedObjects = new List<Transform>();
            for (int i = 0; i < gameObjects.Length; ++i)
                trackedObjects.Add(gameObjects[i].transform);
        }
        sqrMagnitude = magnitude * magnitude;
    }

    public override void UpdateTrigger()
    {
        if (transform == null || trackedObjects == null)
            return;

        // check each object. All it takes is one object to be able to return success
        Vector2 direction;
        for (int i = 0; i < trackedObjects.Count; ++i)
        {
            direction = (Vector2)trackedObjects[i].position - ((Vector2)transform.position + offset);
            if(MovementUtilities.WithinCircle2D(transform, offset, trackedObjects[i].transform, magnitude) != null)
            {
                foundObject = trackedObjects[i];
                RaiseTriggerOn(foundObject);
                return;
            }
        }
        // no objects are within distance. Return failure
        if (foundObject != null) // there was some object found recently.
        {
            RaiseTriggerOff(foundObject);
            foundObject = null;
        }
        return;
    }

    public void OnDrawGizmos()
    {
        if (!drawGizmos)
            return;

        if (foundObject != null)
        {
            Gizmos.color = Registry.alarmColor;
            Gizmos.DrawLine((Vector2)transform.position, (Vector2)trackedObjects[0].position);
        }
    }
}
