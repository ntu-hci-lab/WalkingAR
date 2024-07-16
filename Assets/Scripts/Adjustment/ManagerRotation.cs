using System.Collections;
using System.Collections.Generic;
using TMPro;
//using Unity.Media;
using UnityEngine;
using UnityEngine.UIElements;

//public class ManagerRotation : MonoBehaviour
//{
//    public AdjustmentManager adjustmentManager;
//    public ResizeManager resizeManager;
//    [SerializeField] TaskWindowManager resolutionWindowManager;

//    public TMP_Text SystemLog;
//    public bool isPitchClick = false;
//    public bool isYawClick = false;

//    private bool modifyOn = false;
//    private string refHand;
//    private float handRotation_z;
//    private Vector3 originalRotationHand;
//    private Vector3 originalRotationTarget;
//    private Vector3 originalRotationPlayer;
//    private Quaternion originalRotationHandq;
//    private Quaternion originalRotationTargetq;
//    private Quaternion relatedRotation;
//    private GameObject hand;

//    [SerializeField] private GameObject OculusHand_R;
//    [SerializeField] private GameObject OculusHand_L;
//    public GameObject target;
//    private void Start()
//    {

//    }

//    private void Update()
//    {
//        if (!adjustmentManager.isSetting) return;
//        if (resolutionWindowManager.isOpen) return;

//        if (resizeManager.resizing) return;

//        if (!modifyOn) return;

//        if (!isPitchClick && !isYawClick) return;

//        Quaternion newRotation = hand.transform.rotation * originalRotationHandq;
//        if (isPitchClick)
//        {
//            target.transform.rotation = hand.transform.rotation * originalRotationHandq;
//            target.transform.localEulerAngles = new Vector3(target.transform.localEulerAngles.x, target.transform.localEulerAngles.y, 0f);
//        }

//        target.transform.rotation = hand.transform.rotation * originalRotationHandq;
//        target.transform.localEulerAngles = new Vector3(target.transform.localEulerAngles.x, target.transform.localEulerAngles.y, 0f);
//    }

//    public void modifyRotationOn(string h)
//    {
//        modifyOn = true;
//        refHand = h;
//        Debug.Log("modify rotation on");

//        if (h == "right") hand = OculusHand_R;
//        else if (h == "left") hand = OculusHand_L;
//        originalRotationHand = hand.transform.eulerAngles;
//        originalRotationTarget = target.transform.eulerAngles;
//        originalRotationHandq = Quaternion.Inverse(hand.transform.rotation) * target.transform.rotation;
//        originalRotationTargetq = target.transform.rotation;


//        if (adjustmentManager.isSetting) { SystemLog.text = "Rotation"; }
//    }
//    public void modifyRotationOff()
//    {
//        modifyOn = false;
//        print("modify rotation off");
//    }
//}

public class ManagerRotation : MonoBehaviour
{
    private bool modifyOn = false;
    private Quaternion originalRotationHand;
    private GameObject hand;

    public AdjustmentManager adjustmentManager;
    public ResizeManager resizeManager;
    public TMP_Text SystemLog;
    [SerializeField] TaskWindowManager resolutionWindowManager;

    [SerializeField] private GameObject OculusHand_R;
    [SerializeField] private GameObject OculusHand_L;
    public int rotationMode; // 0: none 1: pitch, 2: yaw
    public bool canRotate = false;
    public GameObject target;
    private float tmp;

    private void Update()
    {
        if (!adjustmentManager.isSetting) return;
        // if (resolutionWindowManager.isOpen) return;
        if (resizeManager.resizing) return;
        if (!canRotate) return;
        if (!modifyOn) return;

        target.transform.rotation = hand.transform.rotation * originalRotationHand;

        if (adjustmentManager.isSetting) { SystemLog.text = "Rotation"; }

        if (rotationMode == 1) target.transform.localEulerAngles = new Vector3(calculate1(target.transform.localEulerAngles.x), tmp, 0f);
        else if(rotationMode == 2) target.transform.localEulerAngles = new Vector3(tmp, calculate(target.transform.localEulerAngles.y), 0f);
    }

    private float calculate(float num)
    {
        //Debug.Log(num+" "+ (num-360));
        num = num % 360;
        if (num > 180) num -= 360;
        num /= 2f;
        return Mathf.Max(-70f, Mathf.Min(70f, num));
    }
    private float calculate1(float num)
    {
        //Debug.Log(num+" "+ (num-360));
        num = num % 360;
        if (num > 180) num -= 360;
        return Mathf.Max(-90f, Mathf.Min(90f, num));
    }

    public void modifyRotationOn(string h)
    {
        if (rotationMode == 0) return;

        modifyOn = true;
        //Debug.Log("modify rotation on");
        if (h == "right") hand = OculusHand_R;
        else if (h == "left") hand = OculusHand_L;
        originalRotationHand = Quaternion.Inverse(hand.transform.rotation) * target.transform.rotation;

        if (rotationMode == 2) tmp = target.transform.localEulerAngles.x;
        else if(rotationMode == 1) tmp = target.transform.localEulerAngles.y;
    }
    public void modifyRotationOff()
    {
        modifyOn = false;
        SystemLog.text = "";
        //print("modify rotation off");
    }
    public void pitchMode()
    {
        rotationMode = 1;
    }
    public void yawMode()
    {
        rotationMode = 2;
    }

    public void banRotation()
    {
        canRotate = false;
        if (adjustmentManager.isSetting) { SystemLog.text = "Can Resize"; }
    }
    public void canRotation()
    {
        canRotate = true;
        if (adjustmentManager.isSetting) { SystemLog.text = "Can Rotate"; }
    }
}