using UnityEngine;
using UnityEngine.UI;

public class SettingManager : MonoBehaviour
{
    private SettingsPanelManager settingsPanelManager;
    private Toggle toggle;

    private void Start()
    {
        settingsPanelManager = this.GetComponentInParent<SettingsPanelManager>();
        toggle = this.GetComponent<Toggle>();
    }

    public void SelectThis()
    {
        if (toggle.isOn) settingsPanelManager.CurrentSelected = this.transform.GetSiblingIndex();
        else settingsPanelManager.CurrentSelected = -1;
    }
}
