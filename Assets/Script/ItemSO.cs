using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Game/Item",fileName = "NextItem")]
public class ItemSO : ScriptableObject
{
    [Header("Score Value")]

    public int point = 10;
}
