using TMPro;
using UnityEngine;
using Vuplex.WebView;

public class SecPanelManager : MonoBehaviour
{
    [SerializeField] private CanvasWebViewPrefab webView;
    [SerializeField] private DataManager dataManager;
    [SerializeField] private Canvas canvas;
    public TMP_Text SystemLog;

    public float resolution = 480 / 0.9f ;
    public float resolutionType;

    async public void GoBack()
    {
        if (webView != null)
        {
            bool canGoBack = await webView.WebView.CanGoBack();
            if (canGoBack) webView.WebView.GoBack();
            Debug.Log("in null Clicked");
        }
    }

    async public void GoForward()
    {
        if (webView != null)
        {
            bool canGoForward = await webView.WebView.CanGoForward();
            if (canGoForward) webView.WebView.GoForward();
        }

    }

    public void Reload()
    {
        if (webView != null)
        {
            webView.WebView.Reload();
        }
    }

    public void LoadURL(string url)
    {
        if (webView != null)
        {
            webView.WebView.LoadUrl(url);
            dataManager.taskUrl= url;
        }
    }

    public void SetResolution(int widthPixels)
    {
        resolutionType = widthPixels;
        webView.Resolution = widthPixels / (canvas.transform as RectTransform).rect.width;
        resolution = webView.Resolution;

        string device = "Phone";
        if(widthPixels == 768)
        {
            device = "Tablet";
        } else if(widthPixels == 1024)
        {
            device = "Desktop";
        }

        SystemLog.text = device;
    }
}
