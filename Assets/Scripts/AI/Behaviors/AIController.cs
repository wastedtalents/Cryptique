using UnityEngine;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Controlls AI by a set of triggers.
/// </summary>
[AddComponentMenu("AI/Behaviors/AIController")]
public class AIController : AIBehavior
{
    public AITransition[] transitions;

    private List<AITrigger> _triggs;
    private AIBehavior _defaultBehavior;
    private AIBehavior _currentBehavior;

    protected override void Initialize()
    {
        _triggs = new List<AITrigger>();
        foreach (var trans in transitions)
        {
            if (trans.isDefault)
            {
                _defaultBehavior = trans.transitTo;
                _currentBehavior = _defaultBehavior;
            }
        else
            {
                trans.trigger.TriggerOn += (type, o) =>
                {
                    trans.transitTo.Reset(o);

                    _currentBehavior.isActive = false; // stop what youre doing.
                    _currentBehavior = trans.transitTo;
                    _currentBehavior.isActive = true; // start another.
                };
                trans.trigger.TriggerOff += (type, o) =>
                {
                    _currentBehavior.isActive = false;
                    _currentBehavior = _defaultBehavior;
                    _currentBehavior.isActive = true;
                };
            }

            if (trans.trigger != null)
                _triggs.Add(trans.trigger);         
        }
    }

    public override void Reset(object obj)
    {
    } 

    protected override void StartMove()
    {

    }

    protected override void UpdatePosition()
    {
        _triggs.ForEach(tr => tr.UpdateTrigger());
//        if (_currentBehavior != null)
//            _currentBehavior.UpdatePosition();
    }

}
