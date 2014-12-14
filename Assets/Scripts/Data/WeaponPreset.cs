using System;
using UnityEngine;
using System.Collections;

/// <summary>
/// Presets for the weapon loaded from xml.
/// </summary>
[Serializable]
public class WeaponPreset
{
    public GameObject bulletPrefab;

    public float RemainingCooldown
    {
        get
        {
            var cool = _lastCooldown - (Time.time - _timeLastFired);
            return cool < 0 ? 0 : cool;
        }
    }

    /// <summary>
    /// Current cooldown based on last fire.
    /// </summary>
    /// <param name="cooldown"></param>
    public void SetCooldown(float cooldown, float timeLastFired)
    {
        _lastCooldown = cooldown;
        _timeLastFired = timeLastFired;
    }

    private float _lastCooldown = 0;
    private float _timeLastFired = 0;

    public string name;
    public int bulletCount = 8;
    public float weaponFireRate = 0.5f;
    public float bulletSpacing = 0.5f;
    public float bulletSpread = 0.65f;
    public float bulletSpeed = 15f;
    public float bulletRandomness = 0.65f;

    /// <summary>
    /// Optional.
    /// </summary>
    public Transform gunPointUp;
    public Transform gunPointDown;
    public Transform gunPointLeft;
    public Transform gunPointRight;

}
