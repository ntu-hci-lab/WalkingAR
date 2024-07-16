using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyMovement : MonoBehaviour
{
    [SerializeField] private float movingThreshold;
    [SerializeField] private Transform src;
    //[SerializeField] private Transform dst;
    [SerializeField] private Transform body;
    //[SerializeField] private Transform anchor;
    //[SerializeField] private float speed;
    private Vector3 curPos = new Vector3(0f, 0f, 0f);
    private Vector3 prePos = new Vector3(0f, 0f, 0f);
    private Vector3 movingDir;
    private Vector3 preMovingDir;

    private float rotationSpeed;
    [SerializeField] private float angleDifferenceThreshold;
    [SerializeField] private float maxRotationSpeed;
    [SerializeField] private float minRotationSpeed;
    [SerializeField] private float minConstantSpeedThreshold;

    [SerializeField] private float stopInterval;
    private float stopTime;
    private bool isStopped = false;

    public float smoothingFactor = 0.1f;
    private bool isTracking = true;
    public bool isUpdatingVector = false;
    //private Quaternion startRotation;
    //private float t = 0f;
    private void Start()
    {
        prePos = src.position;
    }
    void FixedUpdate()
    {

        // 有移動
        if (isUpdatingVector)
        {
            curPos = src.position;
            if ((curPos - prePos).magnitude >= movingThreshold)
            {
                stopTime = 0f;
                isStopped = false;
                updateMovingDirection();
            }
            else if (!isStopped)
            {
                stopTime += Time.deltaTime;
                if (stopTime >= stopInterval)
                {
                    movingDir.Set(src.forward.x, 0f, src.forward.z);
                    isStopped = true;
                }
            }
            if (isTracking)
            {
                ///// 平滑地将 body.forward 逐渐改变为 movingDir /////
                // smoothFollow(body, movingDir);
                // body.forward = movingDir;
                smoothRotate();
            }
            prePos = curPos;
        }

        

        ///// 將目標物的位置設定成 anchor /////
        //dst.position = anchor.position;
        //dst.rotation = anchor.rotation;
        
    }
    private void updateMovingDirection()
    {
        movingDir = (curPos - prePos);
        movingDir.Set(movingDir.x, 0f, movingDir.z);
        movingDir = movingDir.normalized;

        movingDir = Vector3.Lerp(preMovingDir, movingDir, smoothingFactor);
        preMovingDir = movingDir;
    }
    //private void smoothFollow(Transform t, Vector3 v)
    //{
    //    Vector3 result = t.forward + (v - t.forward).normalized * speed * Time.deltaTime;
    //    t.forward = result;
    //    if (Vector3.Distance(t.forward, result) <= Vet.forward = result; ctor3.Distance(t.forward, v)) t.forward = result;
    //}
    private void smoothRotate()
    {
        Quaternion targetRotation = Quaternion.LookRotation(movingDir);

        // 计算 a.forward 和 moveDir 之间的夹角
        float angle = Vector3.Angle(body.forward, movingDir);
        // Debug.Log(angle);

        
        if (angle > angleDifferenceThreshold)
        {
            ///// 減速移動 /////
            // 计算旋转速度
            rotationSpeed = Mathf.Lerp(minRotationSpeed, maxRotationSpeed, Mathf.InverseLerp(minConstantSpeedThreshold, 180f, angle));
            // 平滑地将 a.forward 逐渐改变为 moveDir
            Quaternion newRotation = Quaternion.RotateTowards(body.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            body.rotation = newRotation;
            // Debug.Log(angle + " " + rotationSpeed);
        }


        
    }
    public void startTracking() {
        prePos = src.position;
        movingDir.Set(0f, 0f, 0f);
        isUpdatingVector = true;
        //isTracking = true;
    }
    public void stopTracking() {
        isUpdatingVector = false;
        body.forward = new Vector3(src.forward.x, 0f, src.forward.z);
        
        //isTracking = false;
    }
}
