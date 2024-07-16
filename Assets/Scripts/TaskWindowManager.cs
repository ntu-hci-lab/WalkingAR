using UnityEngine;

public class TaskWindowManager : MonoBehaviour
{
    // public StudyManager studyManager;
    public StartWindowManager startWindowManager;
    public AdjustmentManager adjustmentManager;
    [SerializeField] private SettingWindowManager settingWindowManager;
    [SerializeField] bool onlyInSettingMode = false;

    public bool isOpen = false;
    public void close()
    {
        this.gameObject.SetActive(false);
        isOpen = false;
    }

    public void open()
    {
        // if (!startWindowManager.isStudyStart) return;
        
        if (onlyInSettingMode)
        {
            if (!adjustmentManager.isSetting) return; // 沒在 settingMode 不能開啟
        } else
        {
            if (adjustmentManager.isSetting) return; // 在 settingMode 不能開啟
        }

        this.gameObject.SetActive(true);
        isOpen = true;
    }
}
