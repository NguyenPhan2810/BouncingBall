using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public interface IPlayerDamagable
{
    // This should return damage percent relative to max HP
    // 0 means 0%; 1 means 100%
    float GetDamage(GameObject player, Collider2D collision);
}

public class PlayerControllerScript : MonoBehaviour
{
    private float bounceHeight;
    private Vector2 initialVelocity;
    private Rigidbody2D rigidBody;
    private bool touchGround = false;
    private Vector3 originalScale;
    private Vector3 touchScale;

    private float currentHP;
    private float maxHP;

    public float BounceHeight
    {
        get { return bounceHeight; }
        set
        {
            bounceHeight = value;
            initialVelocity.y = (float)Math.Sqrt(2 * rigidBody.gravityScale * bounceHeight);
        }
    }

    // Start is called before the first frame update
    private void Start()
    {
        originalScale = transform.localScale;
        touchScale = new Vector3(originalScale.x * 1.25f, originalScale.y / 1.25f, originalScale.z);

        rigidBody = GetComponent<Rigidbody2D>();
        rigidBody.gravityScale = 700f;
        BounceHeight = 5000f;

        maxHP = 100;
        currentHP = maxHP;
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetAxis("Jump") != 0 && !touchGround)
        {
            var vel = rigidBody.velocity;
            rigidBody.velocity = new Vector2(vel.x, -5000f);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            touchGround = true;
            rigidBody.velocity = initialVelocity;
            //transform.localScale = touchScale;
        }
        
        var damagable = collision.gameObject.GetComponent<IPlayerDamagable>();
        if (damagable != null)
        {   
            currentHP -= maxHP * damagable.GetDamage(gameObject, collision);
            if (currentHP < 0)
                Destroy(gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            touchGround = false;
            transform.localScale = originalScale;
        }
    }
}
