using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsPanelManager : MonoBehaviour
{
    // TODO: Check anchor of Settings Panel vertical layout group

    /// <summary>
    /// The "div" that contains all settings
    /// </summary>
    [SerializeField] private GameObject savedSettings;
    [SerializeField] private GameObject settingPrefab;
    [SerializeField] private Button applyButton;
    [SerializeField] private Button deleteButton;
    [SerializeField] private Button favoriteButton;
    [SerializeField] private Button uploadButton;
    /// <summary>
    /// Text that will be appended to the start of the current selected favorite
    /// </summary>
    [SerializeField] private string favoriteText;
    [SerializeField] private DataManager dataManager;
    private int settingIndex = -1;

    public int currentSelected = -1;
    public int CurrentSelected
    {
        set
        {
            if (currentSelected != -1) savedSettings.transform.GetChild(currentSelected).GetComponent<Toggle>().isOn = false; // Turn off initial toggle
            currentSelected = value; // Set new currentSelected
            // Debug.Log(currentSelected);

            if (currentSelected == -1)
            {
                applyButton.interactable = deleteButton.interactable = favoriteButton.interactable = false;
            }
            else
            {
                applyButton.interactable = deleteButton.interactable = favoriteButton.interactable = true;
            }
        }
    }

    public int currentFavorite = -1;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            Debug.Log("click save");
            AddSavedSetting();
        }
    }

    public void AddSavedSetting()
    {
        // Instantiating
        GameObject newSetting = Instantiate(settingPrefab);
        
        newSetting.transform.SetParent(savedSettings.transform);
        newSetting.transform.SetAsLastSibling();
        (newSetting.transform as RectTransform).localPosition = new Vector3((newSetting.transform as RectTransform).localPosition.x, (newSetting.transform as RectTransform).localPosition.y, 0f);
        (newSetting.transform as RectTransform).localScale = new Vector3(1f, 1f, 1f);
        (newSetting.transform as RectTransform).localEulerAngles = new Vector3(0f, 0f, 0f);

        // Update text to reflect time
        // newSetting.GetComponentInChildren<TMP_Text>().text = (System.DateTime.Now.Hour < 10 ? "0" : "") + System.DateTime.Now.Hour + ":" + (System.DateTime.Now.Minute < 10 ? "0" : "") + System.DateTime.Now.Minute + ":" + (System.DateTime.Now.Second < 10 ? "0" : "") + System.DateTime.Now.Second;
        settingIndex += 1;

        string windowBehaviorText = "Head Anchor";
        if (dataManager.windowBehavior == "follow") windowBehaviorText = "Path Anchor";

        newSetting.GetComponentInChildren<TMP_Text>().text = settingIndex.ToString() + " " + windowBehaviorText;
    }

    public void DeleteSavedSetting()
    {
        if (currentFavorite == currentSelected)
        {
            uploadButton.interactable = false;
            currentFavorite = -1;
        }
        savedSettings.transform.GetChild(currentSelected).gameObject.SetActive(false); // setting cannot be deleted due to loss of index for referral
        CurrentSelected = -1;
    }

    public void SetFavorite()
    {
        if (currentFavorite != -1) savedSettings.transform.GetChild(currentFavorite).GetComponentInChildren<TMP_Text>().text = savedSettings.transform.GetChild(currentFavorite).GetComponentInChildren<TMP_Text>().text[(favoriteText.Length + 1)..];
        currentFavorite = currentSelected;
        savedSettings.transform.GetChild(currentFavorite).GetComponentInChildren<TMP_Text>().text = favoriteText + " " + savedSettings.transform.GetChild(currentFavorite).GetComponentInChildren<TMP_Text>().text;
        uploadButton.interactable = true;
    }


}
