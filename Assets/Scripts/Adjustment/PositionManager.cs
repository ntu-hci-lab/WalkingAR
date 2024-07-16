using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PositionManager : MonoBehaviour
{
    [SerializeField] private AdjustmentManager adjustmentManager;
    [SerializeField] private TMP_Text SystemLog;

    [SerializeField] private GameObject leftHand;
    [SerializeField] private GameObject rightHand;
    [SerializeField] private GameObject centerEye;
    public GameObject target;
    private GameObject refHand;
    //[SerializeField] private float maxDistanceDelta;
    /*[SerializeField]
    private float magnification = 5f;*/
    [SerializeField] private float delayTime;
    [SerializeField] private float scaler;

    private bool isFollowing = false;
    private Vector3 previousPosition;
    private bool isLeft = false;
    private bool isRight = false;
    private void Update()
    {
        if (!adjustmentManager.isSetting) return;
        if (isFollowing)
        {
            Vector3 curPos = refHand.transform.position - centerEye.transform.position;
            Vector3 displacement = curPos - previousPosition;
            // Adapted magnification
            target.transform.position += displacement * Mathf.Max(1f, Vector3.Distance(refHand.transform.position, target.transform.position)) * scaler;
            //print(target.transform.position);
            previousPosition = curPos;
        }
    }

    public void StartFollow(string hand)
    {
        if (hand == "right")
        {
            isRight = true;
            refHand = rightHand;
            previousPosition = refHand.transform.position - centerEye.transform.position;
        }
        else if (hand == "left")
        {
            isLeft = true;
            refHand = leftHand;
            previousPosition = refHand.transform.position - centerEye.transform.position;
        }
        if (isLeft && isRight)
        {
            isFollowing = false;
        }
        else isFollowing = true;
        //if (Time.time > delayTime)
        //{
        //    isFollowing = true;
        //    previousPosition = refHand.transform.position;
        //}
        SystemLog.text = "Position";
    }

    public void StopFollow(string hand)
    {
        if (hand == "right") isRight = false;
        else if (hand == "left") isLeft = false;
        isFollowing = false;

        SystemLog.text = "";
    }


}