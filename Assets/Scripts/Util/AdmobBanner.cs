using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;

public class AdmobBanner : Singleton<AdmobBanner>
{
    private readonly string unitID = "ca-app-pub-9954381112163314/7880925764";

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
