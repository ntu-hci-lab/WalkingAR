using System.Collections;
using UnityEngine;
using Vuplex.WebView;

public class InstagramLogin : MonoBehaviour
{
    [SerializeField] private string accountName;
    [SerializeField] private string password;

    private CanvasWebViewPrefab webView;
    private bool loginLoaded = false;

    public async void Login(CanvasWebViewPrefab wv)
    {
        webView = wv;
        await webView.WebView.WaitForNextPageLoadToFinish();
        StartCoroutine(nameof(RunJS));
    }

    private IEnumerator RunJS()
    {
        yield return new WaitUntil(() => IsLoaded());
        
        string js =
            "let username = document.getElementsByTagName(\"input\")[0];" +
            "var nativeUsernameValueSetter = Object.getOwnPropertyDescriptor(window.HTMLInputElement.prototype,\"value\").set;" +
            "nativeUsernameValueSetter.call(username, \"" + accountName + "\");" +
            "var usernameEvent = new Event(\"input\", { bubbles: true });" +
            "username.dispatchEvent(usernameEvent);" +
            "let password = document.getElementsByTagName(\"input\")[1];" +
            "var nativePasswordValueSetter = Object.getOwnPropertyDescriptor(window.HTMLInputElement.prototype,\"value\").set;" +
            "nativePasswordValueSetter.call(password, \"" + password + "\");" +
            "var passwordEvent = new Event(\"input\", { bubbles: true });" +
            "password.dispatchEvent(passwordEvent);" +
            "document.getElementsByTagName(\"button\")[1].click();";

        webView.WebView.ExecuteJavaScript(js);
    }

    private bool IsLoaded()
    {
        CheckIsLoaded();
        return loginLoaded;
    }

    private async void CheckIsLoaded()
    {
        var result = await webView.WebView.ExecuteJavaScript("document.getElementsByTagName(\"input\")[0];");
        loginLoaded = (result != "undefined");
    }
}
