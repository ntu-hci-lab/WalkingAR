using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class ToastManager : MonoBehaviour
{
    public TMP_Text toastText;
    public float displayDuration = 1f;

    private bool isShowing = false;

    public void ShowToast(string message)
    {
        if (!isShowing)
        {
            toastText.text = message;
            isShowing = true;
            this.gameObject.SetActive(true);
            StartCoroutine(ShowToastCoroutine());
        }
    }

    private System.Collections.IEnumerator ShowToastCoroutine()
    {
        // Show the toast by animating its position
        // You can use Unity's Animation, Tweening libraries or custom code for the animation
        yield return new WaitForSeconds(displayDuration);

        // Hide the toast after the display duration
        // You can animate its position back off-screen

        isShowing = false;
        this.gameObject.SetActive(false);
    }
}
