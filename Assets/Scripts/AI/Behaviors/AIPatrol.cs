using UnityEngine;
using System.Collections.Generic;
using System;

[AddComponentMenu("AI/Behaviors/AIPatrol")]
public class AIPatrol : AIBehavior {

    public Transform[] points;
    public float distanceDelta = 0.2f;
    public float speed = 2f;
    public float turningSpeed = 4f;
    public float slowdownDistance = 0.6F;
    public bool randomize;
    public bool reuseNodes; // use already used.

    private List<int> _takenPositions;
    private int _currentPointIndex = 0;
    private Vector2 _target;
    private Transform _tr;
    protected Rigidbody2D _rigid;

    protected override void Initialize()
    {
        if(points.Length == 0)
        {
            Debug.Log("ERROR - cannot follow 0 point path");
        }
        _tr = transform;
        _rigid = rigidbody2D;

        if (randomize)
        {
            _takenPositions = new List<int>();
            _target = NextRandomPoint();
        }
        else
        {
            _target = points[_currentPointIndex].position;
        }
    }

    /// <summary>
    /// Reset this data.
    /// </summary>
    /// <param name="obj"></param>
    public override void Reset(object obj)
    {
        if (!(obj is Transform[]))
            return;
        points = (Transform[]) obj;
    }

    /// <summary>
    /// Next random point to be taken.
    /// </summary>
    /// <returns></returns>
    protected Vector2 NextRandomPoint()
    {
        if (reuseNodes) { // just use nodes. 
            return points[UnityEngine.Random.Range(0, points.Length)].position;
        }

        if (_takenPositions.Count == points.Length)
            _takenPositions.Clear();

        int randIndex = 0;
        do {
            randIndex = UnityEngine.Random.Range(0, points.Length);
        } while (_takenPositions.Contains(randIndex));
        _takenPositions.Add(randIndex);
        return points[randIndex].position;
    }

    protected override void UpdatePosition()
    {
        var currentPosition = (Vector2)_tr.position;
        float dist = XYSqrMagnitude(_target, currentPosition);
        if (dist < distanceDelta)
        {
            _currentPointIndex++;
            if (_currentPointIndex >= points.Length)
                _currentPointIndex = 0;

            if(randomize)
                _target = NextRandomPoint();
            else
                _target = points[_currentPointIndex].position;
        }

        // Calculate velocity.
        var targetDistance = (_target - currentPosition).magnitude;
        float slowdown = Mathf.Clamp01(targetDistance / slowdownDistance);
        Vector2 dir = (_target - currentPosition).normalized * speed * slowdown;

        if (_rigid != null)
            _rigid.AddForce(dir);
        else
            transform.Translate(dir * Time.deltaTime, Space.World);

        // Rotate.
        Quaternion rot = _tr.rotation;
        Quaternion toTarget = Quaternion.LookRotation((_target - currentPosition), -Vector3.forward);

        rot = Quaternion.Slerp(rot, toTarget, turningSpeed * Time.deltaTime);
        Vector3 euler = rot.eulerAngles;
        euler.y = 0;
        euler.x = 0;
        rot = Quaternion.Euler(euler);

        _tr.rotation = rot;
    }

    protected float XYSqrMagnitude(Vector2 a, Vector2 b)
    {
        float dx = b.x - a.x;
        float dy = b.y - a.y;
        return dx * dx + dy * dy;
    }

    protected override void StartMove()
    {
    }
}
