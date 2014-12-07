using UnityEngine;
using System.Collections;
using System;

public class AICanSeeTrigger : AITrigger
{
    [Tooltip("The object that we are searching for. If this value is null then the objectLayerMask will be used")]
    public Transform targetObject;

    [Tooltip("The LayerMask of the objects that we are searching for. Used when threres no target object.")]
    public LayerMask objectLayerMask;

    [Tooltip("The object that is within sight")]
    private Transform foundObject;

    [Tooltip("Sets an offset from the centre")]
    public Vector2 offset;

    [Tooltip("The distance that the object needs to be within")]
    public float magnitude;

    [Tooltip("The field of view angle of the agent (in degrees)")]
    public float fieldOfViewAngle = 90;

    [Tooltip("The distance that the agent can see ")]
    public float viewDistance = 100;


    public override AITriggerType TriggerType
    {
        get
        {
            return AITriggerType.CanSee;
        }
    }

    void Update()
    {
        UpdateTrigger();
    }

    public override void UpdateTrigger()
    {
        var prevObj = foundObject;
        // If the target object is null then determine if there are any objects within sight based on the layer mask
        if (targetObject == null)
        {
            foundObject = MovementUtilities.WithinSight2D(transform, offset, fieldOfViewAngle, viewDistance, objectLayerMask);
        }
        else
        { // If the target is not null then determine if that object is within sight
            foundObject = MovementUtilities.WithinSight2D(transform, offset, fieldOfViewAngle, viewDistance, targetObject);
        }

        // check.
        if (foundObject != null)
        {
            RaiseTriggerOn(foundObject);
        }
        else if(prevObj != null)
        {
            RaiseTriggerOff(prevObj);
        }
    }

    void OnDrawGizmos()
    {
#pragma warning disable CS0472
        if (fieldOfViewAngle == null || viewDistance == null)
#pragma warning restore CS0472
        {
            return;
        }
        MovementUtilities.DrawLineOfSight(transform, offset, fieldOfViewAngle, viewDistance, true);

        if(foundObject != null)
        {
            Gizmos.color = Registry.alarmColor;
        }
        else
        {
            Gizmos.color = Registry.undoneColor;
        }
        Gizmos.DrawLine((Vector2)transform.position, (Vector2)targetObject.position);
    }
}
