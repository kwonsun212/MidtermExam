using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    // Start is called before the first frame update
    public float duration = 0.3f; // 이펙트가 유지될 시간 (초)

    void Start()
    {
        Destroy(gameObject, duration);
    }
}
