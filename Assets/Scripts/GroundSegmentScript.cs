using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class GroundSegmentScript : MonoBehaviour, IGroundSegment, IPlayerDamagable
{
    public Vector3 velocity;
    private SpriteRenderer sr;

    private void Awake()
    {
        if (!TryGetComponent(out sr))
        {
            Debug.LogError("SpriteRenderer component not found!");
        }
    }

    private void FixedUpdate()
    {
        transform.position += velocity * Time.fixedDeltaTime;
    }

    public float Length()
    {
        return sr.size.x * transform.localScale.x;
    }

    public float Width()
    {
        return sr.size.y * transform.localScale.y;
    }

    public float GetDamage(GameObject player, Collider2D collision)
    {
        if (collision == null)
            return 0f;

        if (collision is EdgeCollider2D)
            return 1f ;

        return 0f;
    }
}