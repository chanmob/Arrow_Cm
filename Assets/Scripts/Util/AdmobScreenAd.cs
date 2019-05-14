using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;

public class AdmobScreenAd : Singleton<AdmobScreenAd>
{
    private readonly string unitID = "ca-app-pub-9954381112163314/7445811499";

    private InterstitialAd screenAd;

    private void InitAd()
    {
        screenAd = new InterstitialAd(unitID);

        AdRequest request = new AdRequest.Builder().Build();

        screenAd.LoadAd(request);
    }

    public void Show()
    {
        InitAd();
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
