using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour
{

    public enum BulletOwner
    {
        Player, Enemy
    }

    public BulletOwner bulletOwner { get; set; }
    public float speed = 1f;
    public Vector2 direction = new Vector2(1f, 0f);
    public SpriteRenderer bulletSpriteRenderer;

    public float directionAngle;

    public float bulletXPosition, bulletYPosition;

    // Use this for initialization
    void Start()
    {
        // Cache the sprite renderer on start when bullets are initially created and pooled for better performance
        bulletSpriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    void OnEnable()
    {
        // When the bullet is enabled, ensure it faces the correct direction
        transform.eulerAngles = new Vector3(0.0f, 0.0f, directionAngle * Mathf.Rad2Deg);
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject != null)
        {
            // Account for bullet movement at any angle
            bulletXPosition += Mathf.Cos(directionAngle) * speed * Time.deltaTime;
            bulletYPosition += Mathf.Sin(directionAngle) * speed * Time.deltaTime;

            transform.position = new Vector2(bulletXPosition, bulletYPosition);

            // If the bullet is no longer visible by the main camera, then set it back to disabled, which means the bullet pooling system will then be able to re-use this bullet.
            if (!bulletSpriteRenderer.isVisible) gameObject.SetActive(false);
        }
    }
}
