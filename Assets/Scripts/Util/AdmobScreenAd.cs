using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;

public class AdmobScreenAd : MonoBehaviour
{
    private readonly string unitID = "ca-app-pub-3940256099942544/1033173712";

    private InterstitialAd screenAd;

    private void Start()
    {
        InitAd();
        Invoke("Show", 10f);
    }

    private void InitAd()
    {
        screenAd = new InterstitialAd(unitID);

        AdRequest request = new AdRequest.Builder().Build();

        screenAd.LoadAd(request);
    }

    public void Show()
    {
        StartCoroutine(ShowScreenAd());
    }

    private IEnumerator ShowScreenAd()
    {
        while (!screenAd.IsLoaded())
        {
            yield return null;
        }

        screenAd.Show();
    }
}
