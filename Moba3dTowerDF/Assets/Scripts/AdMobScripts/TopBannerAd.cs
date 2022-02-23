using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;

public class TopBannerAd : MonoBehaviour
{
    private BannerView bannerView;

    public void Start()
    {
        // Initialize the Google Mobile Ads SDK.
        MobileAds.Initialize(initStatus => { });

        this.RequestBanner();
    }

    private void RequestBanner()
    {
#if UNITY_ANDROID
        string adUnitId = "ca-app-pub-3940256099942544/6300978111"; //테스트 id
        //ca-app-pub-5521461896884251~1080890646 //실id
        //ca-app-pub-3940256099942544/6300978111 //테스트 id
#elif UNITY_IPHONE
            string adUnitId = "ca-app-pub-3940256099942544/2934735716"; //테스트 id
#else
            string adUnitId = "unexpected_platform";
#endif

        // Create a 320x50 banner at the top of the screen.
        AdSize adSize = new AdSize(320,32);

#if UNITY_EDITOR
        this.bannerView = new BannerView(adUnitId, AdSize.Banner, AdPosition.Top);
#elif UNITY_ANDROID
        this.bannerView = new BannerView(adUnitId, adSize, AdPosition.Top);
#else
        this.bannerView = new BannerView(adUnitId, AdSize.Banner, AdPosition.Top);
#endif
        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();

        // Load the banner with the request.
        this.bannerView.LoadAd(request);
    }
}
