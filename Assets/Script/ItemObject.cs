using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : MonoBehaviour
{
    [SerializeField] ItemSO data;
    // Start is called before the first frame update
  public int GetPoint()
    {
        return data.point;
    }
}
