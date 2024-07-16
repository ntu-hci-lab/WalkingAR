using UnityEngine;
using Unity.Services.CloudSave;
using System.Collections.Generic;

using UnityEngine.UI;
public class DataWindowManager : MonoBehaviour
{    
    [SerializeField]
    private AuthenticationManager authenticationManager;

    [SerializeField]
    private DataManager dataManager;

    [SerializeField]
    private ToastManager toastManager;

    [SerializeField]
    private ToggleListManager toggleListManager;

    [SerializeField]
    private Button saveButton;

    [SerializeField]
    private Button uploadButton;

    [SerializeField]
    private Button deleteButton;

    [SerializeField]
    private Button applyButton;

    void Start()
    {
        // 儲存資料到 DataManager
        saveButton.onClick.AddListener(() =>
        {
            dataManager.SaveSettingData();
            toastManager.ShowToast("save successfully");
        });

        // 套用已儲存的 Setting
        applyButton.onClick.AddListener(() =>
        {
           dataManager.ApplySettingData();
           toastManager.ShowToast("apply successfully");
        });

        deleteButton.onClick.AddListener(() =>
        {
            dataManager.DeleteSettingData();
            toastManager.ShowToast("delete successfully");
        });

        // 上傳資料到 Cloud Save
        uploadButton.onClick.AddListener(async () =>
        {
            if (!authenticationManager.isLogin) return;
            List<WindowSetting> windowSettings = dataManager.windowSettings;

            if (windowSettings.Count <= 0) {
                toastManager.ShowToast("setting list can't be empty");
                return; 
            }

            var data = new Dictionary<string, object> { { $"{authenticationManager.playerId}", windowSettings } };
            await CloudSaveService.Instance.Data.ForceSaveAsync(data);
            Debug.Log("CloudSave Manager Store Data");
            toastManager.ShowToast("Upload successfully");

        });
    }

    // For debugging
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            Debug.Log("click save");
            saveButton.onClick.Invoke();
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            Debug.Log("click apply");
            applyButton.onClick.Invoke();
        }

        if (Input.GetKeyDown(KeyCode.U))
        {
            Debug.Log("click upload");
            uploadButton.onClick.Invoke();
        }
    }
}
