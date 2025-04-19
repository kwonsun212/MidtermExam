using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpEffect : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Invoke("DestroyJumpE", 0.1f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void DestroyJumpE()
    {
        Destroy(gameObject);
    }
}
