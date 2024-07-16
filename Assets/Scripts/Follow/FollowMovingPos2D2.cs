using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMovingPos2D2 : MonoBehaviour
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

    private float rotationSpeed;
    [SerializeField] private float angleDifferenceThreshold;
    [SerializeField] private float maxRotationSpeed;
    [SerializeField] private float minRotationSpeed;
    [SerializeField] private float minConstantSpeedThreshold;
    //private Quaternion startRotation;
    //private float t = 0f;
    private void Start()
    {
        prePos = src.position;
        //int arraySize = Mathf.CeilToInt(timeWindow / Time.fixedDeltaTime);
        //positionRecord = new Vector3[arraySize];
    }
    void FixedUpdate()
    {
        //print(src.forward + " " + dst.forward);
        // 有移動
        curPos = src.position;
        if ((curPos - prePos).magnitude >= movingThreshold)
        {
            updateMovingDirection();
        }
        ///// 平滑地将 body.forward 逐渐改变为 movingDir /////
        // smoothFollow(body, movingDir);
        // body.forward = movingDir;
        smoothRotate();

        ///// 將目標物的位置設定成 anchor /////
        //dst.position = anchor.position;
        //dst.rotation = anchor.rotation;
        prePos = curPos;
    }
    private void updateMovingDirection()
    {
        movingDir = (curPos - prePos);
        movingDir.Set(movingDir.x, 0f, movingDir.z);
        movingDir = movingDir.normalized;
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
            Debug.Log(angle + " " + rotationSpeed);

            ///// BezierCurve /////
            //if (t >= 1f)
            //{
            //    startRotation = body.rotation;
            //    t = 0f;
            //}
            //t += rotationSpeed * Time.deltaTime;
            //Quaternion newRotation = BezierCurve(startRotation, targetRotation, t);
            //body.rotation = newRotation;
        }


        
    }
    private Quaternion BezierCurve(Quaternion start, Quaternion end, float t)
    {
        Vector3 startEulerAngles = start.eulerAngles;
        Vector3 endEulerAngles = end.eulerAngles;

        float tSquared = t * t;
        float oneMinusT = 1f - t;
        float oneMinusTSquared = oneMinusT * oneMinusT;

        float x = Mathf.LerpAngle(startEulerAngles.x, endEulerAngles.x, t);
        float y = Mathf.LerpAngle(startEulerAngles.y, endEulerAngles.y, t);
        float z = Mathf.LerpAngle(startEulerAngles.z, endEulerAngles.z, t);

        Quaternion result = Quaternion.Euler(x, y, z);
        return result;
    }
}
