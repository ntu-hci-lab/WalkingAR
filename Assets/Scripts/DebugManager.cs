using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugManager : MonoBehaviour
{
    public GameObject mainWindow;
    public GameObject centerEyeAnchor;
    // public GameObject DebugCube;
    public DataManager dataManager;

    // Start is called before the first frame update
    void Start()
    {
        dataManager.GetTargetRelativePosition(mainWindow, centerEyeAnchor);
        dataManager.GetTargetRelativeEulerAngles(mainWindow, centerEyeAnchor);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
