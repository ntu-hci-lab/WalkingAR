using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResizeManager : MonoBehaviour
{
    public AdjustmentManager adjustmentManager;
    public ManagerRotation rotationManager;
    public TMP_Text SystemLog;

    [SerializeField]
    private RectTransform canvasRectTransform;
    [SerializeField]
    private GameObject leftHand;
    [SerializeField]
    private GameObject rightHand;
    [SerializeField]
    private float CDGain = 3f;
    
    // states
    private bool rightOK = false;
    private bool leftOK = false;
    public bool resizing = false;

    private Vector3 previousPositionLeft;
    private Vector3 previousPositionRight;

    private void Update()
    {
        if (!adjustmentManager.isSetting) return;
        if(!resizing) return;

        Vector3 currentPositionLeft = leftHand.transform.position;
        Vector3 displacementLeft = currentPositionLeft - previousPositionLeft;
        Vector3 currentPositionRight = rightHand.transform.position;
        Vector3 displacementRight = currentPositionRight - previousPositionRight;

        float previousDistance = Vector3.Distance(previousPositionLeft, previousPositionRight);
        float currentDistance = Vector3.Distance(currentPositionLeft, currentPositionRight);
            
            
            
        Vector2 displacementTotal = new Vector2(
            Mathf.Abs(displacementLeft.x - displacementRight.x), 
            Mathf.Abs(displacementLeft.y - displacementRight.y)
        );

        // Shrink
        if (previousDistance > currentDistance)
        {
            displacementTotal *= -1;
        }

        ResizeCanvas(displacementTotal);

        previousPositionLeft = currentPositionLeft;
        previousPositionRight = currentPositionRight;

        rotationManager.banRotation();
    }

    private void ResizeCanvas(Vector2 sizeDeltaChange)
    {
        Debug.Log(sizeDeltaChange);
        Debug.Log(canvasRectTransform.sizeDelta);
        Debug.Log("##############");
        canvasRectTransform.sizeDelta += sizeDeltaChange * CDGain;        
    }

    #region Pose Actions
    public void StartRight()
    {
        rightOK = true;
        if (leftOK)
        {
            resizing = true;
            previousPositionLeft = leftHand.transform.position;
            previousPositionRight = rightHand.transform.position;
            if (adjustmentManager.isSetting) { SystemLog.text = "Resizing"; }
        }
    }

    public void StopRight()
    {
        rightOK = false;
        resizing = false;
        SystemLog.text = "";
    }

    public void StartLeft()
    {
        leftOK = true;
        if (rightOK)
        {
            resizing = true;
            previousPositionLeft = leftHand.transform.position;
            previousPositionRight = rightHand.transform.position;
            if (adjustmentManager.isSetting) { SystemLog.text = "Resizing"; }
        }
    }

    public void StopLeft()
    {
        leftOK = false;
        resizing = false;
        SystemLog.text = "";
    }
    #endregion
}
