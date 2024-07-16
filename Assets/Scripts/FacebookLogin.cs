using System.Collections;
using UnityEngine;
using Vuplex.WebView;

public class FacebookLogin : MonoBehaviour
{
    [SerializeField] private string email;
    [SerializeField] private string password;
    [SerializeField] private CanvasWebViewPrefab webView;

    private bool loginLoaded = false;

    // For testing purposes: uncomment to delete cache on each interation
    private void Awake()
    {
        Web.ClearAllData();
    }

    public void Login()
    {
        // await webView.WaitUntilInitialized();
        // await webView.WebView.WaitForNextPageLoadToFinish();
        webView.WebView.LoadUrl("https://www.facebook.com/");
        loginLoaded = false;
        StartCoroutine(nameof(RunJS));
    }

    private IEnumerator RunJS()
    {
        yield return new WaitUntil(() => webView.WebView.Url.Contains("facebook"));
        yield return new WaitUntil(() => IsLoaded());
        yield return new WaitForSecondsRealtime(1.0f); // extra buffer for loading time

        string js =
            "document.getElementById(\"email\").value = \"" + email + "\";" +
            "document.getElementById(\"pass\").value = \"" + password + "\";" +
            "document.getElementsByName(\"login\")[0].click();";

        webView.WebView.ExecuteJavaScript(js);
    }

    private bool IsLoaded()
    {
        CheckIsLoaded();
        return loginLoaded;
    }

    private async void CheckIsLoaded()
    {
        // await webView.WebView.WaitForNextPageLoadToFinish();
        var result = await webView.WebView.ExecuteJavaScript("document.getElementsByTagName(\"input\")[0];");
        loginLoaded = (result != "undefined");
    }
}
