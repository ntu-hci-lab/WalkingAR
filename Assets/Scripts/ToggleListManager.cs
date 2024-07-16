using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleListManager : MonoBehaviour
{
    public GameObject togglePrefab;
    public Transform toggleContainer;
    
    public List<WindowSetting> windowSettings;
    public List<Toggle> toggles;

    public Toggle selectedToggle;
    public int selectedIndex;

    [SerializeField]
    ToastManager toastManager;

    public void CreateToggle(WindowSetting windowSetting)
    {
        GameObject toggleObject = Instantiate(togglePrefab, toggleContainer);
        
        // Fix: 要生成在最下方
        toggleObject.transform.SetAsLastSibling();

        Toggle toggle = toggleObject.GetComponent<Toggle>();
        
        // 掛載 eventListener
        toggle.onValueChanged.AddListener(OnToggleValueChanged);
       
        toggleObject.transform.GetChild(1).gameObject.GetComponentInChildren<Text>().text = windowSetting.ToString();
        
        toggles.Add(toggle);
    }

    public void RemoveToggle()
    {
        Destroy(selectedToggle.gameObject);
    }

    void OnToggleValueChanged(bool isOn)
    {
        Toggle changedToggle = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.GetComponent<Toggle>();
        Debug.Log("TOGGLE", changedToggle);

        if (isOn)
        {
            if (selectedToggle != null && selectedToggle != changedToggle)
            {
                // Deselect the previously selected toggle
                selectedToggle.isOn = false;
            }

            // Set the currently selected toggle
            selectedToggle = changedToggle;
            selectedIndex = changedToggle.gameObject.transform.GetSiblingIndex();
            toastManager.ShowToast($"selectedIndex: {selectedIndex}");
        }
        else if (selectedToggle == changedToggle)
        {
            // If the current toggle was deselected, clear the selected toggle variable
            toastManager.ShowToast("same toggle");
            selectedToggle = null;
        }
    }

    //public void RemoveToggle(WindowSetting windowSetting)
    //{
    //    if (toggleObjects.TryGetValue(windowSetting, out GameObject toggleObject))
    //    {
    //        Destroy(toggleObject);
    //        toggleObjects.Remove(windowSetting);
    //    }
    //}
}

