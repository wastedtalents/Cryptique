using UnityEngine;
using System;
using System.Collections.Generic;

/// <summary>
/// Service responsible for performing actions.
/// </summary>
public class ActionService : Singleton<ActionService>
{
    private Dictionary<ComboAction, WeaponPreset> _presets;
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

        // this should be loaded from xml.
        _presets = new Dictionary<ComboAction, WeaponPreset>()
        {
            { ComboAction.BasicShot, new WeaponPreset() // basic.
            {
                bulletPrefab = SceneManager.Instance.WeaponManager.basicShotPrefab,
                bulletCount =  10,
                bulletRandomness = 0.15f,
                bulletSpacing = 1,
                bulletSpeed = 2.5f,
                bulletSpread = 2.5f,
                weaponFireRate = 1
            }},
            { ComboAction.HeavyShot, new WeaponPreset() // heavy.
            {
                bulletPrefab = SceneManager.Instance.WeaponManager.basicShotPrefab,
                bulletCount =  2,
                bulletRandomness = 1,
                bulletSpacing = 2,
                bulletSpeed = 5,
                bulletSpread = 1,
                weaponFireRate = 2
            } }
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
        _player.ApplyWeapon(_presets[ComboAction.BasicShot]);
        _player.Shoot();
    }

    /// <summary>
    /// Playa shoots.
    /// </summary>
    protected void PlayerHeavyShoot()
    {
        _player.ApplyWeapon(_presets[ComboAction.HeavyShot]);
        _player.Shoot();
    }
}
