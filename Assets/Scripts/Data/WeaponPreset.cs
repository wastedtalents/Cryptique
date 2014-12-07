using System;
using UnityEngine;
using System.Collections;

/// <summary>
/// Presets for the weapon loaded from xml.
/// </summary>
[Serializable]
public class WeaponPreset
{
    public string name;
    public int bulletCount = 8;
    public float weaponFireRate = 0.5f;
    public float bulletSpacing = 0.5f;
    public float bulletSpread = 0.65f;
    public float bulletSpeed = 15f;
    public float bulletRandomness = 0.65f;
}
