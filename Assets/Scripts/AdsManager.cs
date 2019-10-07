using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using System;

public class Ads : MonoBehaviour
{
    private string gameIDAndroid = "3317313";
    private string videoId = "video";
    private string videoRewardId = "rewardedVideo";

    private void Awake()
    {
        Advertisement.Initialize(gameIDAndroid,true);
    }
    public void UIWatchAd()
    {
        WatchVideoAd(VideoAdEnd);
    }
    public void UIWatchRewardAd()
    {
        WatchVideoRewardAd(VideoRewardAdEnd);
    }

    public void WatchVideoAd(Action<ShowResult> result)
    {
        if (Advertisement.IsReady(videoId))
        {
            ShowOptions so = new ShowOptions();
            so.resultCallback = result;
            Advertisement.Show(videoId);
        }
        else
        {
            Debug.Log("No cargo el video");
        }
    }

    public void WatchVideoRewardAd(Action<ShowResult> result)
    {
        if (Advertisement.IsReady(videoRewardId))
        { 
        ShowOptions so = new ShowOptions();
        so.resultCallback = result;
        Advertisement.Show(videoRewardId);
        }
        else
        {
            Debug.Log("No cargo el video reward");
        }
    }

    public void VideoAdEnd(ShowResult result)
    {
        switch (result)
        {
            case ShowResult.Failed:
                Debug.Log("El Ad Fallo");
                break;
            case ShowResult.Finished:
                Debug.Log("El Ad termino");
                break;
            case ShowResult.Skipped:
                Debug.Log("El Ad fue skipeado");
                break;
        }
    }

    public void VideoRewardAdEnd(ShowResult result)
    {
        switch (result)
        {
            case ShowResult.Failed:
                Debug.Log("El Ad Fallo");
                break;
            case ShowResult.Finished:
                Debug.Log("El Ad termino");
                break;
            case ShowResult.Skipped:
                Debug.Log("El Ad fue skipeado");
                break;
        }
    }
}
