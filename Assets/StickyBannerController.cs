using YandexMobileAds;
using YandexMobileAds.Base;
using UnityEngine;
using System;
using System.Collections;

public class StickyBannerController : MonoBehaviour
{
    private Banner banner;

    private Coroutine _coroutineLoadBanner;

    private string adUnitId = "R-M-14329025-1"; // замените на "R-M-XXXXXX-Y"
    private void Awake()
    {
        RequestStickyBanner();

        DontDestroyOnLoad(this.gameObject);
    }
    private int GetScreenWidthDp()
    {
        int screenWidth = (int)Screen.safeArea.width;
        return ScreenUtils.ConvertPixelsToDp(screenWidth);
    }

    private void RequestStickyBanner()
    {
        BannerAdSize bannerMaxSize = BannerAdSize.StickySize(GetScreenWidthDp());
        banner = new Banner("demo-banner-yandex", bannerMaxSize, AdPosition.CenterRight);

        AdRequest request = new AdRequest.Builder().Build();

        banner.LoadAd(request);

        // Вызывается, когда реклама с вознаграждением была загружена
        banner.OnAdLoaded += HandleAdLoaded;

        // Вызывается, если во время загрузки произошла ошибка
        banner.OnAdFailedToLoad += HandleAdFailedToLoad;
    }

    private void TryReloadBanner()
    {
        AdRequest request = new AdRequest.Builder().Build();

        banner.LoadAd(request);
    }

    private IEnumerator ReloadBannerCoroutine()
    {
        while (true)
        {
            TryReloadBanner();
            yield return new WaitForSeconds(10);
        }
    }

    private void HandleAdLoaded(object sender, EventArgs args)
    {
        Debug.Log("AdLoaded event received");
        banner.Show();

        if (_coroutineLoadBanner != null)
        {
            StopCoroutine(_coroutineLoadBanner);
            _coroutineLoadBanner = null;
        }
    }

    private void HandleAdFailedToLoad(object sender, AdFailureEventArgs args)
    {
        if (_coroutineLoadBanner == null)
        {
           _coroutineLoadBanner = StartCoroutine(ReloadBannerCoroutine());
        }

        Debug.Log($"AdFailedToLoad event received with message: {args.Message}");
        // Настоятельно не рекомендуется пытаться загрузить новое объявление с помощью этого метода
    }
}
