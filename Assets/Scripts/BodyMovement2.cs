using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyMovement2 : MonoBehaviour
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
    [SerializeField] private float bodyEyeThreshold;

    [SerializeField] private float stopInterval;
    private float stopTime;
    private bool isStopped = false;

    public float smoothingFactor = 0.1f;
    private bool isTracking = true;
    private bool isSameDirection = true;
    public bool isUpdatingVector = false;
    private Vector3 tmp = new Vector3(0f, 0f, 0f);

    //private Vector2 b = new Vector2(0f, 0f);
    //private Quaternion startRotation;
    //private float t = 0f;
    private void Start()
    {
        prePos = src.position;
        //followPos3D = gameObject.GetComponent<FollowPos3D>();
    }
    void FixedUpdate()
    {

        // 有移動
        if (isUpdatingVector)
        {
            curPos = src.position;
            if ((curPos - prePos).magnitude >= movingThreshold)
            {
                //stopTime = 0f;
                //isStopped = false;
                updateMovingDirection();
                prePos = curPos;
            }
            // 如果停下來，就重置視野
            //else if (!isStopped) 
            //{
            //    stopTime += Time.deltaTime;
            //    if (stopTime >= stopInterval)
            //    {
            //        movingDir.Set(src.forward.x, 0f, src.forward.z);
            //        isStopped = true;
            //    }
            //}

            //isHeadBodySameDirection();
            if (isTracking)
            {
                ///// 平滑地将 body.forward 逐渐改变为 movingDir /////
                smoothRotate();
            }
            
        }        
    }
    private void updateMovingDirection()
    {
        Vector3 dir = (curPos - prePos);
        dir.Set(dir.x, 0f, dir.z);
        dir = dir.normalized;
        //dir = Vector3.Lerp(preMovingDir, dir, smoothingFactor);
        isSameDirection = isHeadBodySameDirection(dir);
        if (isSameDirection) movingDir = dir;
        preMovingDir = movingDir;
    }
    private bool isHeadBodySameDirection(Vector3 dir) // 如果移動方向和眼睛方向在同一面
    {
        tmp.Set(src.forward.x, 0f, src.forward.z);
        float angle = Vector3.Angle(tmp, dir);
        if (angle > bodyEyeThreshold) return false;
        else return true;
    }
    private void smoothRotate()
    {
        if (movingDir.Equals(Vector3.zero)) return;
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
        //prePos = src.position;
        //movingDir.Set(0f, 0f, 0f);
        isUpdatingVector = true;
        //isTracking = true;
    }
    public void stopTracking() {
        isUpdatingVector = false;
        //body.forward = new Vector3(src.forward.x, 0f, src.forward.z);
        
        //isTracking = false;
    }
}
