using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FollowManager : MonoBehaviour
{
    public AdjustmentManager adjustmentManager;
    public TMP_Text SystemLog;
    [SerializeField]
    private GameObject objectToFollow;
    [SerializeField]
    private GameObject follower;
    [SerializeField]
    private float maxDistanceDelta = 10f;
    /*[SerializeField]
    private float magnification = 5f;*/
    [SerializeField]
    private float delayTime = 5f;

    private bool isFollowing = false;

    private Vector3 previousPosition;


    private void Update()
    {
        if (!adjustmentManager.isSetting) return;
        if (isFollowing)
        {
            Vector3 toFollowCurrentPosition = objectToFollow.transform.position;
            Vector3 displacement = toFollowCurrentPosition - previousPosition;
            // Adapted magnification
            follower.transform.position += displacement * Vector3.Distance(toFollowCurrentPosition, follower.transform.position) * maxDistanceDelta;

            previousPosition = toFollowCurrentPosition;
        }        
    }

    public void StartFollow()
    {
        if (Time.time > delayTime)
        {
            isFollowing = true;
            if (adjustmentManager.isSetting) { SystemLog.text = "Position"; }
            previousPosition = objectToFollow.transform.position;
        }
    }

    public void StopFollow()
    {
        isFollowing = false;
        SystemLog.text = "";
    }


}
