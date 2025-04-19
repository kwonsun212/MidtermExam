using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeTrap : MonoBehaviour
{
    public Vector3 upPosition;        // Ƣ��� ��ġ (���� ��ǥ ����)
    public Vector3 downPosition;      // �� ��ġ
    public float moveSpeed = 2f;      // �̵� �ӵ�
    public float stayTime = 1.5f;     // �ӹ����� �ð�

    private bool movingUp = true;

    // Start is called before the first frame update
    void Start()
    {
        // ���� ��ġ�� �Ʒ��� ����
        transform.position = downPosition;
        StartCoroutine(MoveTrap());
    }

    // Update is called once per frame
    IEnumerator MoveTrap()
    {
        while (true)
        {
            Vector3 target = movingUp ? upPosition : downPosition;

            // Ʈ���� ��ǥ ��ġ���� �̵�
            while (Vector3.Distance(transform.position, target) > 0.01f)
            {
                transform.position = Vector3.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);
                yield return null;
            }

            // ���� ��ȯ �� ���� �ð� ���
            movingUp = !movingUp;
            yield return new WaitForSeconds(stayTime);
        }
    }
}
