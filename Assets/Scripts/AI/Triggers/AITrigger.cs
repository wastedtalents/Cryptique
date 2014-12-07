using System;
using UnityEngine;
using System.Collections;

/// <summary>
/// Triggers some AI behavior.
/// </summary>
public abstract class AITrigger : MonoBehaviour {

    public abstract AITriggerType TriggerType { get; }
    
    [Tooltip("Determines whether to draw gizmos")]
    public bool drawGizmos = false;

    #region Events.

    private event Action<AITriggerType, object> _triggerOn;
    private event Action<AITriggerType, object> _triggerOff;

    public event Action<AITriggerType, object> TriggerOff
    {
        add
        {
            _triggerOff += value;
        }
        remove
        {
            _triggerOff -= value;
        }
    }
    public event Action<AITriggerType, object> TriggerOn
    {
        add
        {
            _triggerOn += value;
        }
        remove
        {
            _triggerOn -= value;
        }
    }

    /// <summary>
    /// Raises the ON trigger.
    /// </summary>
    /// <param name="obj"></param>
    protected void RaiseTriggerOn(Transform obj)
    {
        if (_triggerOn != null)
            _triggerOn(TriggerType, obj);
    }

    /// <summary>
    /// Raises the OFF trigger.
    /// </summary>
    /// <param name="obj"></param>
    protected void RaiseTriggerOff(Transform obj)
    {
        if (_triggerOff != null)
            _triggerOff(TriggerType, obj);
    }

    #endregion

    /// <summary>
    /// Updates the trigger.
    /// </summary>
    public abstract void UpdateTrigger();
}
