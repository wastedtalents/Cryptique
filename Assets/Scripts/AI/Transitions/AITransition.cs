using System;
using UnityEngine;

[Serializable]
public class AITransition
{
    public string name;

    public bool isDefault;

    [Tooltip("Trigger that happened")]
    public AITrigger trigger;

    [Tooltip("Which behavior to fire")]
    public AIBehavior transitTo;

    public void Register(Action<AITriggerType, object> onAction, Action<AITriggerType, object> offAction)
    {
        trigger.TriggerOn += onAction;
        trigger.TriggerOff += offAction;
    }
}
