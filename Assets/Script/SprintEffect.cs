using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SprintEffect : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Invoke("DestroySprintE", 0.03f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void DestroySprintE()
    {
        Destroy(gameObject);
    }
}
