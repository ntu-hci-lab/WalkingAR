using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.Rendering.DebugUI;

public class RotationHandler : MonoBehaviour
{
    public void rotateWindowDown()
    {
        Debug.Log("ROTATE DOWNWARD!!");
        var rotationVector = this.gameObject.transform.localRotation.eulerAngles;
        rotationVector.x += 15;
        this.gameObject.transform.localRotation = Quaternion.Euler(rotationVector);
    }

    public void rotateWindowUp()
    {
        Debug.Log("ROTATE UPWARD!!");
        var rotationVector = this.gameObject.transform.localRotation.eulerAngles;
        rotationVector.x -= 15;
        this.gameObject.transform.localRotation = Quaternion.Euler(rotationVector);
    }
}
