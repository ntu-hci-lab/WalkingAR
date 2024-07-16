using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Oculus.Interaction;
using System;

public class PanelPositionHandler : MonoBehaviour
{
    [SerializeField] private Transform playerHead;
    // [SerializeField] private bool parallelMode = true;
    [SerializeField] public Vector3 offset = new Vector3(0f, 0f, 0f);
    private Vector3 originalPosition = new Vector3(0f, 0f, 0f);

    public bool isSetting = false;
    public string behavior = "auto-centering";

    private GameObject window;

    public Grabbable grabbable = null;
    


    private void Start()
    {
        window = this.gameObject;
        originalPosition = window.transform.position;
    }

    private void Update()
    {
        // show the panel in front of the player
        if (grabbable.isGrabbed || isSetting) { return; }
        Vector3 playerPosition = playerHead.position;

        if (behavior == "auto-centering")
        {
            // window.transform.position = playerHead.TransformPoint(offset);

            // rotate the panel to face the player frame by frame
            // window.transform.LookAt(new Vector3(playerPosition.x, playerPosition.y, playerPosition.z));
            // window.transform.forward *= -1;
        } else if (behavior == "fixed")
        {
            originalPosition = window.transform.position;
            Vector3 newPosition = playerHead.TransformPoint(offset);
            window.transform.position = new Vector3(newPosition.x, originalPosition.y, newPosition.z);
            Debug.Log(window.transform.position.y);

            // window.transform.LookAt(new Vector3(playerPosition.x, playerPosition.y, playerPosition.z));
            // window.transform.forward *= -1;
        }
    }
}
