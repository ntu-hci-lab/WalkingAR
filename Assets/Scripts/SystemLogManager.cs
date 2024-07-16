using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemLogManager : MonoBehaviour
{

    public void CloseSystemLog(float duration)
    {
        StartCoroutine(Close(duration));
    }
    IEnumerator Close(float duration)
    {
        // Wait for the specified duration
        yield return new WaitForSeconds(duration);

        // Close or disable the game object
        this.gameObject.SetActive(false);
    }
}
