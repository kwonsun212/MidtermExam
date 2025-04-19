using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeTrap : MonoBehaviour
{
    public Vector3 upPosition;        // 튀어나올 위치 (월드 좌표 기준)
    public Vector3 downPosition;      // 들어간 위치
    public float moveSpeed = 2f;      // 이동 속도
    public float stayTime = 1.5f;     // 머무르는 시간

    private bool movingUp = true;

    // Start is called before the first frame update
    void Start()
    {
        // 시작 위치를 아래로 세팅
        transform.position = downPosition;
        StartCoroutine(MoveTrap());
    }

    // Update is called once per frame
    IEnumerator MoveTrap()
    {
        while (true)
        {
            Vector3 target = movingUp ? upPosition : downPosition;

            // 트랩이 목표 위치까지 이동
            while (Vector3.Distance(transform.position, target) > 0.01f)
            {
                transform.position = Vector3.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);
                yield return null;
            }

            // 방향 전환 후 일정 시간 대기
            movingUp = !movingUp;
            yield return new WaitForSeconds(stayTime);
        }
    }
}
