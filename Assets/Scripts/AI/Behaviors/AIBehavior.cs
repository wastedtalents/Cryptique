using UnityEngine;
using System.Collections;

/// <summary>
/// Interface for basic AI behaviors.
/// </summary>
public abstract class AIBehavior : MonoBehaviour
{
    public bool isActive = false;

    /// <summary>
    /// Ran first and once at start.
    /// </summary>
    protected abstract void Initialize();

    /// <summary>
    /// Reset state of object.
    /// </summary>
    /// <param name="obj"></param>
    public abstract void Reset(object obj);

    /// <summary>
    /// Updates position.
    /// </summary>
    protected abstract void UpdatePosition();

    /// <summary>
    /// Starts the move.
    /// </summary>
    protected abstract void StartMove();

    void Awake()
    {
        Initialize();
    }

    void Update()
    {
        if(isActive)
            UpdatePosition();
    }

    void Start()
    {
        StartMove();
    }
}
