using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriangleObstacleScript : MonoBehaviour, IPlayerDamagable
{
    public GameObject       boundary;
    public delegate void    CollideBoundary(GameObject obj);
    public CollideBoundary  collideBoundary;
    public GameObject       particleEffect;

    private void Start()
    {
        collideBoundary += particleEvent;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision == boundary.GetComponent<Collider2D>())
        {
            collideBoundary(gameObject);
        }
    }

    public float GetDamage(GameObject player, Collider2D collision)
    {
        return 1f;
    }

    private void particleEvent(GameObject obj)
    {
        var ptc = Instantiate(particleEffect);
        ptc.transform.position = new Vector3(transform.position.x, transform.position.y, ptc.transform.position.z);
        ptc.GetComponent<ParticleSystem>().Play();
        ptc.SetActive(true);
    }
}
