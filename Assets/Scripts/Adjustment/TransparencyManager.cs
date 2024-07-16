using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TransparencyManager : MonoBehaviour
{
    public AdjustmentManager adjustmentManager;
    public TMP_Text SystemLog;

    [SerializeField] private Transform fingerTipLeft;
    [SerializeField] private Transform fingerTipRight;
    public RawImage target;
    [SerializeField] private float scaler;
    [SerializeField] private float minTransparency;

    private bool modifyOn = false;
    // private float wholeLength = 0.2f;  // 實測出 0.2 的結果
    private Transform refHand;
    private float fingerStartingPoint;
    private float originalTransparency;

    void Update()
    {
        if (!adjustmentManager.isSetting) return;
        if (!modifyOn) return;
        float displacement = refHand.position.y - fingerStartingPoint;
        float trans = originalTransparency + displacement * scaler;
        trans = Mathf.Min(1f, Mathf.Max(minTransparency, trans));
        print(displacement + " " + trans);
        target.color = new Vector4(1f, 1f, 1f, trans);

    }
    public void modifyTransparentOn(string h)
    {
        modifyOn = true;
        if (h == "right") refHand = fingerTipRight;
        else if (h == "left") refHand = fingerTipLeft;
        fingerStartingPoint = refHand.position.y;
        originalTransparency = target.color.a;
        // Debug.Log("modify on");
        if (adjustmentManager.isSetting) { SystemLog.text = "Setting Opacity"; }
    }
    public void modifyTransparentOff()
    {
        modifyOn = false;
        // print("modify off");
        SystemLog.text = "";
    }

}