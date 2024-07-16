using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowDisplayManager : MonoBehaviour
{
    [SerializeField]
    private GameObject anchorObject;
    [SerializeField]
    private GameObject lookedObject;
    [SerializeField]
    private GameObject TaskWindow;

    public float openDataWindowTime;

    private void Start()
    {
        this.gameObject.SetActive(false);
    }
    private void Update()
    {
        if (!this.gameObject.activeSelf) return;
        this.gameObject.transform.LookAt(lookedObject.transform.position);
        this.gameObject.transform.forward *= -1;
    }

    public void showWindow()
    {
        this.gameObject.SetActive(true);
        if(this.gameObject.name == "DataWindow")
        {
            this.gameObject.transform.position = anchorObject.transform.position;
            TaskWindow.SetActive(false);
        }
    }

    public void closeWindow()
    {
        this.gameObject.SetActive(false);
    }
}
