using System.Collections;
using System.Collections.Generic;
using Unity.Media;
using UnityEngine;
using UnityEngine.UIElements;

public class RotationManager : MonoBehaviour
{
    private bool modifyOn = false;
    private string refHand;
    private float handRotation_z;
        
    [SerializeField]
    private GameObject OculusHand_R;
    [SerializeField]
    private GameObject OculusHand_L;
    [SerializeField]
    private GameObject MainWindow;

    private void Update()
    {
        if (!modifyOn) return;
        
        handRotation_z = 360 - OculusHand_R.transform.localEulerAngles.z;
        // Debug.Log(OculusHand_R.transform.eulerAngles.z + " rotation_z: " + handRotation_z);
        if(refHand == "left") handRotation_z = OculusHand_L.transform.localEulerAngles.z;

        // if (handRotation_z < 0 || handRotation_z > 90) return;

        var rotationVector = MainWindow.transform.localRotation.eulerAngles;
        rotationVector.x = 90 - handRotation_z;

        // Debug.Log(rotationVector.x + " " + handRotation_z);

        if (rotationVector.x > 90 || rotationVector.x < 0) return;
        MainWindow.transform.localRotation = Quaternion.Euler(rotationVector);
    }

    public void modifyRotationOn(string h)
    {
        modifyOn = true;
        refHand = h;
        Debug.Log("modify rotation on");
    }
    public void modifyRotationOff()
    {
        modifyOn = false;
        print("modify rotation off");
    }
}
