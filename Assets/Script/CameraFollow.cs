using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject A;  //A라는 GameObject변수 선언
    Transform AT;
    void Start()
    {
        AT = A.transform;
    }
    void LateUpdate()
    {
        transform.position = new Vector3(AT.position.x, AT.position.y + 2, transform.position.z);
    }
}
