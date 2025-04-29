using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class HighScore
{
    private const string KEY = "HighScore";

    // Start is called before the first frame update
    public static int Load(int stage)
    {
        return PlayerPrefs.GetInt(KEY + "_" + stage, 0);
    }

    // Update is called once per frame
    public static void Tryset(int stage, int newScore)
    {
        if (newScore <= Load(stage))
            return;

        PlayerPrefs.SetInt(KEY + "_" + stage, newScore);
        PlayerPrefs.Save();
    }
}
