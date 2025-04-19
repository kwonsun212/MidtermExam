using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisappeaarPlatform : MonoBehaviour
{
    [SerializeField] float fallSec = 0.5f, destroysec = 2f;
    Rigidbody2D rb;


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.isKinematic = true;
    }



    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Invoke("FallPlatform", fallSec);
            Destroy(gameObject, destroysec);
        }
    }


    void FallPlatform()
    {
        rb.isKinematic = false;

    }
}
