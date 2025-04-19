using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public GameObject explosionEffect; // Æø¹ß ÀÌÆåÆ® ÇÁ¸®ÆÕ

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            // Æø¹ß ÀÌÆåÆ®¸¦ »ý¼º
            if (explosionEffect != null)
            {
                Instantiate(explosionEffect, transform.position, Quaternion.identity);
            }

            // Bullet Á¦°Å
            Destroy(gameObject);
        }
    }

    public float BulletSpeed;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("DestroyBullet", 2);
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.rotation.y == 0 )
        {
            transform.Translate(transform.right * BulletSpeed * Time.deltaTime);
        }
        else
        {
            transform.Translate(transform.right * -1 * BulletSpeed * Time.deltaTime);
        }
        
    }


    void DestroyBullet()
    {
        Destroy(gameObject);
    }
}
