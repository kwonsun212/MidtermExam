using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    // Start is called before the first frame update
    public float duration = 0.3f; // ����Ʈ�� ������ �ð� (��)

    void Start()
    {
        Destroy(gameObject, duration);
    }
}
