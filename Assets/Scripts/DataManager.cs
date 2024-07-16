using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Services.CloudSave;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Vuplex.WebView;

[Serializable]
public class WindowSetting
{
    public string UserID = "";
    public float playerHeight;
    public string windowBehavior = "follow";
    public float[] Position = { 0f, 0f, 0f };
    public float[] Rotation = { 0f, 0f, 0f };
    public float Transparency = 1f;
    public float width = 100f;
    public float height = 100f;
    public float resolution;
    public string task = "Youtube"; // YT, Medium, IG
    public string environment = "Campus"; // Campus, Sidewalk
    public float time = 0f; // 使用者按下開始設定的時間，為距離開始時間的秒數
    public bool isSelect = true;
    public bool isFavorite = false;
    public float centerEyeHeight;
    public float resolutionType = 480f;
    public string url = "";
    public string dateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
    public int positionGroup = 0;

    public override string ToString()
    {
        return $"Width: {width}, Height: {height}, Transparency: {Transparency}, Position: {Position}, Rotation: {Rotation}";
    }
}
public class UserData
{
    public string UserID = "";
    public float playerHeight;
    public int favoriteSettingIndex = -1;
    public string scenario = "";
    public List<WindowSetting> windowSettings;
}

public class DataManager : MonoBehaviour
{
    private WindowSetting defaultWindowSetting;

    public int favIndex = -1;
    public float playerHeight = 0f;
    public string windowBehavior = "follow"; // fixed or follow
    public float studyStartTime;
    public string task = "Youtube";
    public string taskUrl = "";
    public string env = "Campus";
    public TMP_Text SystemLog;

    [SerializeField]
    private GameObject mainWindow; // Get position and rotation
    [SerializeField]
    private CanvasWebViewPrefab canvasWebViewPrefab; // Get position and rotation
    [SerializeField]
    private RectTransform canvasRectTransform; // Get width and height
    [SerializeField]
    private RawImage canvasWebViewPrefabView; // Get trans
    [SerializeField]
    private GameObject debuggers;
    [SerializeField] private TMP_Text floorHeightLog;

    [SerializeField]
    private GameObject LogCanvas;

    [SerializeField]
    private GameObject centerEyeAnchor;

    [SerializeField]
    private GameObject bodyAndchor;

    [SerializeField]
    private ToggleListManager toggleListManager;

    [SerializeField]
    private SettingsPanelManager settingsPanelManager;

    [SerializeField]
    private AuthenticationManager authenticationManager;

    [SerializeField]
    private StartWindowManager startWindowManager;

    [SerializeField]
    private AdjustmentManager adjustmentManager;

    [SerializeField]
    private SecPanelManager secPanelManager;

    [SerializeField]
    private AdjustHeight adjustHeightManager;

    // 儲存所有 user 儲存的設定，之後要儲存到後端
    public List<WindowSetting> windowSettings = new List<WindowSetting>();

    private void Awake()
    {
        Web.SetUserAgent(false);
        Debug.Log("SetUserAgent");
    }

    private void Start()
    {
        // 把現在的資料存成 default
        defaultWindowSetting = CreateWindowSetting();
        Debug.Log("DefaultWindowSetting:" + defaultWindowSetting.Position[0] + defaultWindowSetting.Position[1] + defaultWindowSetting.Position[2]);
    }

    private void Update()
    {
        if(playerHeight == 0f && centerEyeAnchor.transform.position.y > 1f)
        {
            playerHeight = centerEyeAnchor.transform.position.y;
            Debug.Log("playerHeight" + playerHeight);
        }

        if (Input.GetKeyDown(KeyCode.N))
        {
            Debug.Log("Reload Scene");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            LogCanvas.SetActive(true);
            SystemLog.text = "New Trial";
            LogCanvas.GetComponent<SystemLogManager>().CloseSystemLog(3f);
        }
    }

    public void Restart()
    {
        if(windowSettings.Count <= 0) { return; }
        if (favIndex == -1) return;
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        LogCanvas.SetActive(true);
        SystemLog.text = "New Trial";
        LogCanvas.GetComponent<SystemLogManager>().CloseSystemLog(3f);
    }

    public void ResetHeight()
    {
        adjustmentManager.turnAhead();

        float currentCenterHeight = centerEyeAnchor.transform.position.y;
        float diffHeight = currentCenterHeight - playerHeight;
        float newWindowheight = mainWindow.transform.position.y + diffHeight;

        playerHeight = currentCenterHeight;

        Debug.Log("currentCenterHeight" + currentCenterHeight);
        debuggers.transform.position = new Vector3(debuggers.transform.position.x, debuggers.transform.position.y + diffHeight, debuggers.transform.position.z);
        mainWindow.transform.position = new Vector3(mainWindow.transform.position.x, newWindowheight, mainWindow.transform.position.z);

        LogCanvas.SetActive(true);
        SystemLog.text = "manul-reset level";
        floorHeightLog.text = " manual " + debuggers.transform.position.y.ToString("#.###");
        
        if(!adjustmentManager.isSetting)
        {
            LogCanvas.GetComponent<SystemLogManager>().CloseSystemLog(1.5f);
        }
        
        
        Debug.Log("RESET FLOOR LEVEL");
    }

    public void SwitchBehavior(string wb)
    {
        // 要在 setting mode 才能換
        if (!adjustmentManager.isSetting) return;

        // 轉到同方向
        adjustmentManager.turnAhead();

        windowBehavior = wb;

        string Log = "Head Anchor";
        GameObject refereceObject = centerEyeAnchor;

        if (windowBehavior == "follow") { refereceObject = bodyAndchor; Log = "Path Anchor"; }
        else { adjustHeightManager.heightStop(); }
        
        mainWindow.transform.parent = refereceObject.transform; 

        LogCanvas.SetActive(true);
        SystemLog.text = Log;

    }

    private WindowSetting CreateWindowSetting()
    {
        // centerEyeAnchor.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        GameObject refereceObject = centerEyeAnchor;
        if (windowBehavior == "follow") refereceObject = bodyAndchor;
        
    
        // 抓現在的視窗設定
        Vector3 position = GetTargetRelativePosition(mainWindow, refereceObject);
        Vector3 rotation = GetTargetRelativeEulerAngles(mainWindow, refereceObject);

        // Get the width and height of the canvas
        float canvasWidth = canvasRectTransform.rect.width;
        float canvasHeight = canvasRectTransform.rect.height;

        Debug.Log("width " + canvasWidth + " height " + canvasHeight);

        WindowSetting setting = new WindowSetting
        {
            UserID = authenticationManager.playerId,
            playerHeight = playerHeight,
            Position = new float[3] { position.x, position.y, position.z },
            Rotation = new float[3] { rotation.x, rotation.y, rotation.z },
            Transparency = canvasWebViewPrefabView.color.a,
            width = canvasWidth,
            height = canvasHeight,
            time =  adjustmentManager.enterAdjustmentTime - studyStartTime,
            resolution = canvasWebViewPrefab.Resolution,
            resolutionType = secPanelManager.resolutionType,
            task = task,
            environment = env,
            windowBehavior = windowBehavior,
            isSelect = true,
            centerEyeHeight = centerEyeAnchor.transform.position.y,
            url = taskUrl,
            isFavorite = false,
        };

        return setting;
    }

    public void ResetSettingData()
    {
        if (!adjustmentManager.isSetting) return;

        if (windowBehavior == "follow")
        {
            mainWindow.transform.localPosition = new Vector3(0f, 1f, 1f);
        }
        else
            mainWindow.transform.localPosition = new Vector3(0f, -0.5f, 1f);
        
        mainWindow.transform.localEulerAngles = new Vector3(0f, 0f, 0f);

        // set transparency of mainWindow
        canvasWebViewPrefabView.color = new Vector4(1f, 1f, 1f, defaultWindowSetting.Transparency);

        // set width and height of mainWindow
        canvasRectTransform.sizeDelta = new Vector2(defaultWindowSetting.width, defaultWindowSetting.height);

        canvasWebViewPrefab.Resolution = defaultWindowSetting.resolution;

        SystemLog.text = "Reset Design";
        Debug.Log("RESET!");
    }


    public void SaveSettingData()
    {
        // 抓現在的視窗設定
        WindowSetting setting = CreateWindowSetting();

        Debug.Log("save setting: " + setting);

        windowSettings.Add(setting);
        // toggleListManager.CreateToggle(setting);
        settingsPanelManager.AddSavedSetting();

        Debug.Log("windowSettings: " + windowSettings);
    }

    // 把儲存的設置 apply 到現在的 window 上
    public void ApplySettingData()
    {
        adjustmentManager.turnAhead();
        int index = settingsPanelManager.currentSelected;
        Debug.Log("index:" + index);

        if (index > windowSettings.Count) { return; }

        WindowSetting targetSetting = windowSettings[index];

        Debug.Log("index targetSetting size" + targetSetting.width + " " + targetSetting.height);

        //centerEyeAnchor.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        GameObject refereceObject = centerEyeAnchor;
        if (windowSettings[index].windowBehavior == "follow") refereceObject = bodyAndchor;
        

        Vector3 relativePosition = new Vector3(targetSetting.Position[0], targetSetting.Position[1], targetSetting.Position[2]);
        Vector3 relativeEulerAngles = new Vector3(targetSetting.Rotation[0], targetSetting.Rotation[1], targetSetting.Rotation[2]);

        // set world position of mainWindow
        Vector3 worldPosition = refereceObject.transform.TransformPoint(relativePosition);
        mainWindow.transform.position = worldPosition;

        // 套用 height difference
        if (targetSetting.windowBehavior == "follow") {
            
            float diffHeight = centerEyeAnchor.transform.position.y - targetSetting.centerEyeHeight;

            Debug.Log("diffHeight: " + diffHeight);
            mainWindow.transform.position = new Vector3(mainWindow.transform.position.x, mainWindow.transform.position.y + diffHeight, mainWindow.transform.position.z);
        }

        // set world rotation of mainWindow
        mainWindow.transform.rotation = refereceObject.transform.rotation* Quaternion.Euler(relativeEulerAngles);

        // set transparency of mainWindow
        canvasWebViewPrefabView.color = new Vector4(1f, 1f, 1f, targetSetting.Transparency);

        // set width and height of mainWindow
        canvasRectTransform.sizeDelta = new Vector2(targetSetting.width, targetSetting.height);

        // 套用 resolution
        canvasWebViewPrefab.Resolution = targetSetting.resolution;

        Debug.Log(index + " windowBehavior:" + targetSetting.windowBehavior);

        // 套用 window behavior
        if (targetSetting.windowBehavior == "follow")
        {
            LogCanvas.SetActive(true);
            SystemLog.text = "Path Anchor";

            windowBehavior = "follow";
            mainWindow.transform.parent = refereceObject.transform;

            // adjustHeightManager.heightStart();
        }
        else
        {
            LogCanvas.SetActive(true);
            SystemLog.text = "Head Anchor";

            windowBehavior = "fixed";
            mainWindow.transform.parent = refereceObject.transform;

            adjustHeightManager.heightStop();
        }
        LogCanvas.GetComponent<SystemLogManager>().CloseSystemLog(1.5f);
    }

    public void DeleteSettingData()
    {
        int index = settingsPanelManager.currentSelected;
        if(index < 0) return;
        if(index > windowSettings.Count) return;
        settingsPanelManager.DeleteSavedSetting();
        windowSettings[index].isSelect = false;
    }

    public async void UploadSettingData()
    {
        if (!authenticationManager.isLogin) return;
        if (!startWindowManager.isStudyStart) return;

        UserData userData = new UserData
        {
            UserID = authenticationManager.playerId,
            playerHeight = playerHeight,
            favoriteSettingIndex = settingsPanelManager.currentFavorite,
            scenario = task + "_" + env,
            windowSettings = windowSettings,
        };

        favIndex = userData.favoriteSettingIndex;
        if(favIndex > 0)
            userData.windowSettings[favIndex].isFavorite = true;

        var data = new Dictionary<string, object> { { $"{authenticationManager.playerId}_{task}_{env}", userData } };
        await CloudSaveService.Instance.Data.ForceSaveAsync(data);
        Debug.Log("CloudSave Manager Store Data");
        if (LogCanvas.activeSelf == false)
        {
            LogCanvas.SetActive(true);
        }
        SystemLog.text = "Upload successfully";
        LogCanvas.GetComponent<SystemLogManager>().CloseSystemLog(3f);
    }

    public Vector3 GetTargetRelativePosition(GameObject targetGameObject, GameObject baseGameObject)
    {
        // Get the relative position of A(target) with respect to B(base)
        Vector3 relativePosition = baseGameObject.transform.InverseTransformPoint(targetGameObject.transform.position);
        Debug.Log("relativePosition: " + relativePosition);
        return relativePosition;  
    }

    public Vector3 GetTargetRelativeEulerAngles(GameObject targetGameObject, GameObject baseGameObject)
    {
        // Get the relative position of A(target) with respect to B(base)
        Quaternion relativeRotation = Quaternion.Inverse(baseGameObject.transform.rotation) * targetGameObject.transform.rotation;
        Vector3 relativeEulerAngles = relativeRotation.eulerAngles;
        Debug.Log("relativeEulerAngles: " + relativeEulerAngles);
        return relativeEulerAngles;
    }
    public void transformDataToWindowSettings()
    {
        
    }

}
