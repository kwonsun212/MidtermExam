using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;

using System.Linq;

public class RankPage : MonoBehaviour
{
    [SerializeField] Transform contentRoot;

    [SerializeField] GameObject rowPrefab;

    StageResultList allData;

    // Start is called before the first frame update
    void Awake()
    {

        allData = StageResultSaver.LoadRank();
        RefreshRankList(1);
    }

    public void RefreshRankList(int idx)
    {
        foreach (Transform child in contentRoot)
        {
            Destroy(child.gameObject);
        }


        var sortedData1 = allData.results.Where(r => r.stage == idx).OrderByDescending(x => x.score).ToList();


        for (int i = 0; i < sortedData1.Count; i++)
        {
            GameObject row = Instantiate(rowPrefab, contentRoot);
            TMP_Text rankText = row.GetComponentInChildren<TMP_Text>();
            rankText.text = $"stage {idx} : {i + 1}. {sortedData1[i].playerName} - {sortedData1[i].score}";
        }
        
    }
}   