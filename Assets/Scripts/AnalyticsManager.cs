using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public class AnalyticsManager : MonoBehaviour
{
    private void Awake()
    {
        EndLevel(1, 3, 5000);
    }
    public void EndLevel(int lvl, int vidas, int puntaje)
    {
        Analytics.CustomEvent("level_finished", new Dictionary<string, object>
        {
            { "nivel",lvl },
            { "vidas",lvl },
            { "puntaje",lvl }
        });
    }
}
