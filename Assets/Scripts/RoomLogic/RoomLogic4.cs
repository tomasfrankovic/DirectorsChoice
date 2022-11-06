using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RoomLogic4 : MonoBehaviour
{
    private void Start()
    {
        SceneChangeManager.instance.EndTransition();
        StartWindows.instance.ShowWin();

        AnalyticsClass.SendCustomEvent("FinishedGame",
                new Dictionary<string, object>() {
                    {"time", Time.unscaledTime }
                });
    }
}
