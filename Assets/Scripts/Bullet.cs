﻿using UnityEngine;

public class Bullet : MonoBehaviour
{
    public new Rigidbody2D rigidbody { get; private set; }
    public float speed = 500f;
    public float life = 10f;

    private void Start()
    {
        if(rigidbody == null)
            rigidbody = GetComponent<Rigidbody2D>();
        
        rigidbody.AddForce(transform.up * speed);
        
    }

    public void Update()
    {
        life -= Time.deltaTime;

        if (life < 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }

}
