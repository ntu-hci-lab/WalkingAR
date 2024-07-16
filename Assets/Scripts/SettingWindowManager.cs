using UnityEngine;

public class SettingWindowManager : MonoBehaviour
{
    public StartWindowManager startWindowManager;
    public bool isOpen = false;

    [SerializeField]
    private GameObject anchorObject;
    [SerializeField]
    private AdjustmentManager adjustmentManager;

    private void Start()
    {
        // this.gameObject.SetActive(false);
    }

    private void Update()
    {
        // 確保 apply 時不會出錯
        //if (isOpen)
        //{
        //    adjustmentManager.turnAhead();
        //}
    }

    public void close()
    {
        this.gameObject.SetActive(false);
        isOpen = false;
    }

    public void open()
    {
        Debug.Log("Open SettingWindow");
        if (!startWindowManager.isStudyStart) return;
        this.gameObject.SetActive(true);
        // this.gameObject.transform.position = anchorObject.transform.position;
        isOpen = true;
    }
}
