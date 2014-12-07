using UnityEngine;
using System.Collections;

public static class MovementUtilities  {

    /// <summary>
    /// Gets the line of sight transform.
    /// </summary>
    /// <param name="transform"></param>
    /// <param name="positionOffset"></param>
    /// <param name="targetObject"></param>
    /// <param name="usePhysics2D"></param>
    /// <returns></returns>
    public static Transform LineOfSight2D(Transform transform, Vector2 positionOffset, Transform targetObject, bool usePhysics2D)
    {
#if !(UNITY_4_0 || UNITY_4_0_1 || UNITY_4_1 || UNITY_4_2)
        if (usePhysics2D)
        {
            RaycastHit2D hit;
            if ((hit = Physics2D.Linecast(targetObject.position, ((Vector2)transform.position + positionOffset))))
            {
                Debug.Log("AAAA " + hit.transform + " : " + targetObject);
                if (hit.transform.Equals(targetObject))
                {
                    return targetObject; // return the target object meaning it is within sight
                }
            }
        }
        else
        {
#endif
            RaycastHit hit;
            if (Physics.Linecast(((Vector2)transform.position + positionOffset), targetObject.position, out hit))
            {
                if (hit.transform.Equals(targetObject))
                {
                    return targetObject; // return the target object meaning it is within sight
                }
            }
#if !(UNITY_4_0 || UNITY_4_0_1 || UNITY_4_1 || UNITY_4_2)
        }
#endif
        return null;
    }

    /// <summary>
    /// Checks whtehre object is within sight.
    /// </summary>
    /// <param name="transform"></param>
    /// <param name="positionOffset"></param>
    /// <param name="fieldOfViewAngle"></param>
    /// <param name="viewDistance"></param>
    /// <param name="objectLayerMask"></param>
    /// <returns></returns>
    public static Transform WithinCircle2D(Transform transform, Vector2 positionOffset, Transform targetObject, float viewDistance)
    {
        var hitColliders = Physics2D.OverlapCircleAll(transform.position, viewDistance);
        if (hitColliders != null)
        {
            for (int i = 0; i < hitColliders.Length; ++i)
            {
                if (hitColliders[i].transform.Equals(targetObject))
                    return targetObject;
            }
        }
        return null;
    }

    /// <summary>
    /// Check if transform is within sight.
    /// </summary>
    /// <param name="transform"></param>
    /// <param name="positionOffset"></param>
    /// <param name="fieldOfViewAngle"></param>
    /// <param name="viewDistance"></param>
    /// <param name="objectLayerMask"></param>
    /// <returns></returns>
    public static Transform WithinSight2D(Transform transform, Vector3 positionOffset, float fieldOfViewAngle, float viewDistance, LayerMask objectLayerMask)
    {
        Transform objectFound = null;
        var hitColliders = Physics2D.OverlapCircleAll(transform.position, viewDistance, objectLayerMask);
        if (hitColliders != null)
        {
            float minAngle = Mathf.Infinity;
            for (int i = 0; i < hitColliders.Length; ++i)
            {
                float angle;
                Transform obj;
                // Call the 2D WithinSight function to determine if this specific object is within sight
                if ((obj = WithinSight(transform, positionOffset, fieldOfViewAngle, viewDistance, hitColliders[i].transform, true, out angle)) != null)
                {
                    // This object is within sight. Set it to the objectFound GameObject if the angle is less than any of the other objects
                    if (angle < minAngle)
                    {
                        minAngle = angle;
                        objectFound = obj;
                    }
                }
            }
        }
        return objectFound;
    }

    /// <summary>
    /// Is within sight.
    /// </summary>
    /// <param name="transform"></param>
    /// <param name="positionOffset"></param>
    /// <param name="fieldOfViewAngle"></param>
    /// <param name="viewDistance"></param>
    /// <param name="targetObject"></param>
    /// <param name="usePhysics2D"></param>
    /// <param name="angle"></param>
    /// <returns></returns>
    private static Transform WithinSight(Transform transform, Vector2 positionOffset, float fieldOfViewAngle, float viewDistance, Transform targetObject, bool usePhysics2D, out float angle)
    {
        // The target object needs to be within the field of view of the current object
        var direction = (Vector2)targetObject.position - ((Vector2)transform.position + positionOffset);
        if (usePhysics2D)
        {
            angle = Vector2.Angle(direction, transform.up);
        }
        else
        {
            angle = Vector3.Angle(direction, transform.forward);
        }
        if (direction.magnitude < viewDistance && angle < fieldOfViewAngle * 0.5f)
        {
            // The hit agent needs to be within view of the current agent
            if (LineOfSight(transform, positionOffset, targetObject, usePhysics2D) != null)
            {
                return targetObject; // return the target object meaning it is within sight
            }
            else if (targetObject.collider == null)
            {
                // If the linecast doesn't hit anything then that the target object doesn't have a collider and there is nothing in the way
                if (targetObject.gameObject.activeSelf)
                    return targetObject;
            }
        }
        // return null if the target object is not within sight
        return null;
    }

    // Public helper function that will automatically create an angle variable that is not used. This function is useful if the calling call doesn't
    // care about the angle between transform and targetObject
    public static Transform WithinSight(Transform transform, Vector3 positionOffset, float fieldOfViewAngle, float viewDistance, Transform targetObject)
    {
        float angle;
        return WithinSight(transform, positionOffset, fieldOfViewAngle, viewDistance, targetObject, false, out angle);
    }

    // Public helper function that will automatically create an angle variable that is not used. This function is useful if the calling call doesn't
    // care about the angle between transform and targetObject
    public static Transform WithinSight2D(Transform transform, Vector3 positionOffset, float fieldOfViewAngle, float viewDistance, Transform targetObject)
    {
        float angle;
        return WithinSight(transform, positionOffset, fieldOfViewAngle, viewDistance, targetObject, true, out angle);
    }

    public static Transform LineOfSight(Transform transform, Vector3 positionOffset, Transform targetObject, bool usePhysics2D)
    {
#if !(UNITY_4_0 || UNITY_4_0_1 || UNITY_4_1 || UNITY_4_2)
        if (usePhysics2D)
        {
            RaycastHit2D hit;
            if ((hit = Physics2D.Linecast(targetObject.position, (transform.position + positionOffset))))
            {
                if (hit.transform.Equals(targetObject))
                {
                    return targetObject; // return the target object meaning it is within sight
                }
            }
        }
        else
        {
#endif
            RaycastHit hit;
            if (Physics.Linecast((transform.position + positionOffset), targetObject.position, out hit))
            {
                if (hit.transform.Equals(targetObject))
                {
                    return targetObject; // return the target object meaning it is within sight
                }
            }
#if !(UNITY_4_0 || UNITY_4_0_1 || UNITY_4_1 || UNITY_4_2)
        }
#endif
        return null;
    }

    public static void DrawLineOfSight(Transform transform, Vector3 positionOffset, float fieldOfViewAngle, float viewDistance, bool usePhysics2D)
    {
#if UNITY_EDITOR
        float radius = viewDistance * Mathf.Sin(fieldOfViewAngle * Mathf.Deg2Rad);
        var oldColor = UnityEditor.Handles.color;
        UnityEditor.Handles.color = Color.yellow;
        // draw a disk at the end of the sight distance.
        var direction = (usePhysics2D ? transform.up : transform.forward);
        UnityEditor.Handles.DrawWireDisc(transform.position + positionOffset + direction * viewDistance, direction, radius);
        // draw to lines to represent the left and right side of the line of sight
        UnityEditor.Handles.DrawLine(transform.position + positionOffset, TransformPointIgnoreScale(transform, new Vector3(radius, 0, viewDistance)));
        UnityEditor.Handles.DrawLine(transform.position + positionOffset, TransformPointIgnoreScale(transform, new Vector3(-radius, 0, viewDistance)));
        UnityEditor.Handles.color = oldColor;
#endif
    }

    private static Vector3 TransformPointIgnoreScale(Transform transform, Vector3 point)
    {
        return transform.rotation * point + transform.position;
    }
}
