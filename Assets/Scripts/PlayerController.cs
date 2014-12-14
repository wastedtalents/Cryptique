using UnityEngine;
using System.Collections;

/// <summary>
/// Basic direction.
/// </summary>
public enum Direction
{
    Up,
    Down,
    Left,
    Right
}

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(ComboTracker))]
public class PlayerController : MonoBehaviour {

    public PlayerData playerData;
    private WeaponPreset _currentWeapon;

    #region Gun settings.

    public Transform gunPointUp;
    public Transform gunPointDown;
    public Transform gunPointLeft;
    public Transform gunPointRight;

    [Tooltip("How many bullets to be shot. Move this to weapon settings.")]
    [Range(5f, 35f)]
    public float _bulletSpeed = 10f;

    [Tooltip("How many bullets to be shot. Move this to weapon settings.")]
    [Range(0f, 5f)]
    public float _bulletRandomness = 0.15f;

    [Tooltip("How many bullets to be shot. Move this to weapon settings.")]
    [Range(0, 20)]
    public float _bulletCount = 10; // do przerobki

    [Tooltip("How many bullets to be shot. Move this to weapon settings.")]
    [Range(0f, 5f)]
    public float _bulletSpread = 2.5f;

    [Tooltip("How many bullets to be shot. Move this to weapon settings.")]
    [Range(0f, 5f)]
    public float _bulletSpacing = 2.5f;

    private float _aimAngle;// TBM to weapon settings.
    private float _bulletSpreadPingPongMax = 2.5f,// TBM to weapon settings.
        _bulletSpreadPingPongMin; // TBM to weapon settings.
    private float _coolDown; // TBM to weapon settings.
    private float _weaponFireRate = 0.5f; // TBM to weapon settings.
    private float _bulletSpreadInitial; // TBM to weapon settings.
    private float _bulletSpacingInitial; // TBM to weapon settings.
    private float _bulletSpreadIncrement; // TBM to weapon settings.
    private float _bulletSpacingIncrement; // TBM to weapon settings.

    #endregion

    private Transform _currentGunPoint;
    private Vector2 _tempVector = new Vector2();        

    private ComboTracker _comboTracker;

    void Awake()
    {
        _comboTracker = GetComponent<ComboTracker>();
    }

	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update ()
	{
	    ParseMovement();        
        ParseCombos();

        // Update cooldown.
        _coolDown -= Time.deltaTime;
    }

    private void ParseCombos()
    {
        if (InputService.Instance.ComboUpPressed)
        {            
            _comboTracker.Register(SKeyCode.Up);
            SceneManager.Instance.HUDManager.ShowArrow(SKeyCode.Up);
        }
        if (InputService.Instance.ComboDownPressed)
        {
            _comboTracker.Register(SKeyCode.Down);
            SceneManager.Instance.HUDManager.ShowArrow(SKeyCode.Down);
        }
        if (InputService.Instance.ComboLeftPressed)
        {
            _comboTracker.Register(SKeyCode.Left);
            SceneManager.Instance.HUDManager.ShowArrow(SKeyCode.Left);
        }
        if (InputService.Instance.ComboRightPressed)
        {
            _comboTracker.Register(SKeyCode.Right);
            SceneManager.Instance.HUDManager.ShowArrow(SKeyCode.Right);
        }
    }

    private void ParseMovement()
    {
        _tempVector.Set(0, 0);

        if (InputService.Instance.UpHeld)
        {
            rigidbody2D.AddForce(Vector2.up * playerData.Speed * Time.deltaTime);
            _tempVector.Set(0, 1);
            _currentGunPoint = gunPointUp;
        }
        if (InputService.Instance.DownHeld)
        {
            rigidbody2D.AddForce(-Vector2.up * playerData.Speed * Time.deltaTime);
            _tempVector.Set(0, -1);
            _currentGunPoint = gunPointDown;
        }
        // left.
        if (InputService.Instance.LeftHeld)
        {
            rigidbody2D.AddForce(-Vector2.right * playerData.Speed * Time.deltaTime);
            _tempVector.Set(-1, 0);
            _currentGunPoint = gunPointLeft;
        }
        if (InputService.Instance.RightHeld)
        {
            rigidbody2D.AddForce(Vector2.right * playerData.Speed * Time.deltaTime);
            _tempVector.Set(1, 0);
            _currentGunPoint = gunPointRight;
        }

        if (_tempVector.x == 0 && _tempVector.y == 0)
            return;

        CalculateAimAndFacingAngles(_tempVector);
    }

    /// <summary>
    /// Calc angle of the shot.
    /// </summary>
    /// <param name="facingDirection"></param>
    private void CalculateAimAndFacingAngles(Vector2 facingDirection)
    {
        _aimAngle = Mathf.Atan2(facingDirection.y, facingDirection.x);
        if (_aimAngle < 0f)
            _aimAngle = Mathf.PI * 2 + _aimAngle;
    }

    public void Shoot()
    {
        if (_coolDown <= 0f)
        {
            ShootGuns();
            _coolDown = _weaponFireRate;
        }
    }

    /// <summary>
    /// Actually shoot the gun/skill.
    /// </summary>
    private void ShootGuns()
    {
        if (_bulletCount > 1)
        {
            _bulletSpreadInitial = -_bulletSpread / 2;
            _bulletSpacingInitial = _bulletSpacing / 2;
            _bulletSpreadIncrement = _bulletSpread / (_bulletCount - 1);
            _bulletSpacingIncrement = _bulletSpacing / (_bulletCount - 1);
        }
        else
        {
            _bulletSpreadInitial = 0f;
            _bulletSpacingInitial = 0f;
            _bulletSpreadIncrement = 0f;
            _bulletSpacingIncrement = 0f;
        }

        // For each 'gun' attachment the player has we'll setup each bullet accordingly...
        for (var i = 0; i < _bulletCount; i++)
        {

            var bullet = (GameObject)Instantiate(SceneManager.Instance.WeaponManager.basicShotPrefab);
            var bulletComponent = (Bullet)bullet.GetComponent(typeof(Bullet));

            var offsetX = Mathf.Cos(_aimAngle - Mathf.PI / 2) * (_bulletSpacingInitial - i * _bulletSpacingIncrement);
            var offsetY = Mathf.Sin(_aimAngle - Mathf.PI / 2) * (_bulletSpacingInitial - i * _bulletSpacingIncrement);

            bulletComponent.directionAngle = _aimAngle + _bulletSpreadInitial + i * _bulletSpreadIncrement;
            bulletComponent.speed = _bulletSpeed;

            var initialPosition = _currentGunPoint.position + (_currentGunPoint.transform.forward * (_bulletSpacingInitial - i * _bulletSpacingIncrement));
            var bulletPosition = new Vector3(initialPosition.x + offsetX + Random.Range(0f, 1f) * _bulletRandomness - _bulletRandomness / 2,
                initialPosition.y + offsetY + Random.Range(0f, 1f) * _bulletRandomness - _bulletRandomness / 2, 0f);

            //var initialPosition = _currentGunPoint.position + (_currentGunPoint.transform.forward * (_bulletSpacingInitial));
            //var bulletPosition = new Vector3(initialPosition.x + offsetX + Random.Range(0f, 1f) * _bulletRandomness - _bulletRandomness / 2,
            //initialPosition.y + offsetY + Random.Range(0f, 1f) * _bulletRandomness - _bulletRandomness / 2, 0f);

            bullet.transform.position = bulletPosition;
            bulletComponent.bulletXPosition = bullet.transform.position.x;
            bulletComponent.bulletYPosition = bullet.transform.position.y;

            bullet.SetActive(true);
            if (bulletComponent.bulletSpriteRenderer != null)
                bulletComponent.bulletSpriteRenderer.color = Color.red;
        }
    }

    /// <summary>
    /// Applies a weapon.
    /// </summary>
    /// <param name="preset"></param>
    public void ApplyWeapon(WeaponPreset preset)
    {
        if (_currentWeapon == preset)
            return;

        _bulletCount = preset.bulletCount;
        _bulletRandomness = preset.bulletRandomness;
        _bulletSpacing = preset.bulletSpacing;
        _bulletSpeed = preset.bulletSpeed;
        _bulletSpread = preset.bulletSpread;
        _weaponFireRate = preset.weaponFireRate;

        // override gunpoints.
        if (preset.gunPointDown != null)
            gunPointDown = preset.gunPointDown;
        if (preset.gunPointLeft != null)
            gunPointLeft = preset.gunPointLeft;
        if (preset.gunPointRight != null)
            gunPointRight = preset.gunPointRight;
        if (preset.gunPointUp != null)
            gunPointUp = preset.gunPointUp;

        // setup cooldowns.
        if(_currentWeapon != null)
            _currentWeapon.SetCooldown(_coolDown, Time.time);
        _currentWeapon = preset;
        _coolDown = _currentWeapon.RemainingCooldown;
    }

    /// <summary>
    /// Fire up a skill.
    /// </summary>
    /// <param name="skillPreset"></param>
    internal void Skill(SkillPreset skillPreset)
    {
        // For now just start.
        StartCoroutine(skillPreset.Execute(null));
    }
}
