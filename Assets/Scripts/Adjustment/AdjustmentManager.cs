using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using Vuplex.WebView;

public class AdjustmentManager : MonoBehaviour
{
    public bool isSetting = false;
    public TMP_Text SystemLog;
    public float enterAdjustmentTime = 0f;

    [SerializeField] private GameObject LogCanvas;
    [SerializeField] private CanvasWebViewPrefab webView;
    [SerializeField] private GameObject mainWindow;

    [SerializeField] StudyManager studyManager;
    [SerializeField] DataManager dataManager;
    [SerializeField] LongitudinalDesignManager longitudinalDesignManager;
    [SerializeField] GameObject Followings;
    [SerializeField] GameObject bodyAnchor;
    [SerializeField] GameObject centerEyeAnchor;
    [SerializeField] GameObject resizeSphere;


    [SerializeField] private Material handMaterial;
    [SerializeField] private Renderer l_renderer;
    [SerializeField] private Renderer r_renderer;

    public void toggleIsSetting()
    {
        // if (longitudinalDesignManager.mode == "emergency") return;

        Debug.Log("Adjustment webView: " + webView);
        isSetting= !isSetting;
        
        if (isSetting)
        {
            LogCanvas.SetActive(true);
            SystemLog.text = "In Setting Mode";
            enterAdjustmentTime = Time.time;
            Followings.SetActive(false);
            
            turnAhead();

            // mainWindow.transform.parent = centerEyeAnchor.transform;

            // add materials

            // Check if the Renderer component exists and there is at least one Material assigned
            if (l_renderer != null && l_renderer.materials.Length > 0)
            {
                // Get the current list of materials
                List<Material> l_materialsList = new List<Material>(l_renderer.materials);
                List<Material> r_materialsList = new List<Material>(r_renderer.materials);

                // Append the new material to the list
                l_materialsList.Add(handMaterial);
                r_materialsList.Add(handMaterial);

                // Assign the updated materials list back to the Renderer
                l_renderer.materials = l_materialsList.ToArray();
                r_renderer.materials = r_materialsList.ToArray();
            }

            resizeSphere.SetActive(true);
        }
        else
        {
            Followings.SetActive(true);
            resizeSphere.SetActive(false);

            if (studyManager.studyType == "design")
            {
                dataManager.SaveSettingData();
                
                dataManager.UploadSettingData();

                if (dataManager.windowBehavior == "follow")
                {
                    mainWindow.transform.parent = bodyAnchor.transform;
                }
            } else if (studyManager.studyType == "long")
            {
                // Longitudinal save data
                longitudinalDesignManager.SaveSettingData();
                longitudinalDesignManager.UploadSettingData();

                if (longitudinalDesignManager.windowBehavior == "follow")
                {
                    mainWindow.transform.parent = bodyAnchor.transform;
                }
            }

            if (l_renderer != null && l_renderer.materials.Length > 0)
            {
                // Get the current list of materials
                List<Material> l_materialsList = new List<Material>(l_renderer.materials);
                List<Material> r_materialsList = new List<Material>(r_renderer.materials);

                // Remove the new material to the list
                l_materialsList.RemoveAt(1);
                r_materialsList.RemoveAt(1);

                // Assign the updated materials list back to the Renderer
                l_renderer.materials = l_materialsList.ToArray();
                r_renderer.materials = r_materialsList.ToArray();
            }

            turnAhead();
        }
    }

    public void turnAhead()
    {
        Quaternion rotationA = bodyAnchor.transform.localRotation;

        // Get the current rotation of gameObjectB
        Quaternion rotationB = centerEyeAnchor.transform.localRotation;

        // Set the y and z rotation of gameObjectA to be the same as gameObjectB
        Quaternion newRotation = Quaternion.Euler(0f, rotationB.eulerAngles.y, 0f);

        bodyAnchor.transform.localRotation = newRotation;
    }
}
