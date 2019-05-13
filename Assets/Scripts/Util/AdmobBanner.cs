using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;

public class AdmobBanner : MonoBehaviour
{
    private readonly string unitID = "ca-app-pub-3940256099942544/6300978111";

    private BannerView banner;

    private void Start()
    {
        InitAd();
        banner.Show();
    }

    private void InitAd()
    {
        banner = new BannerView(unitID, AdSize.SmartBanner, AdPosition.Top);

        AdRequest request = new AdRequest.Builder().Build();

        banner.LoadAd(request);
    }

    public void ToggleAd(bool active)
    {
        if (active)
        {
            banner.Show();
        }
        else
        {
            banner.Hide();
        }
    }
}
