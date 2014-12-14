using UnityEngine;
using System;
using System.Collections.Generic;

/// <summary>
/// Service responsible for performing actions.
/// </summary>
public class ActionService : Singleton<ActionService> {

    private Dictionary<ComboAction, Action> _actionMappers;

    private PlayerController _player;

    void Awake()
    {
        // load components.
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

        _actionMappers = new Dictionary<ComboAction, Action>()
        {
            { ComboAction.BasicShot, PlayerShoot },
            { ComboAction.HeavyShot, PlayerHeavyShoot }
        };
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Execute(ComboAction action)
    {
        if (_actionMappers.ContainsKey(action)) // use predefined.
            _actionMappers[action]();
        else // oooor generic.
            _player.Skill(SkillService.Instance.GetSkill(action));
    }

    /// <summary>
    /// Playa shoots.
    /// </summary>
    protected void PlayerShoot()
    {
        _player.Shoot();
    }

    /// <summary>
    /// Playa shoots.
    /// </summary>
    protected void PlayerHeavyShoot()
    {
        _player.Shoot();
    }
}
