using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AdjustHeight : MonoBehaviour
{
    [SerializeField] private GameObject target;
    [SerializeField] private GameObject MainWindow;
    [SerializeField] private GameObject Debuggers;
    [SerializeField] private float threshold;
    [SerializeField] private float movingSpeed;

    [SerializeField] private StudyManager studyManager;
    [SerializeField] private DataManager dataManager;
    [SerializeField] private LongitudinalDesignManager longitudinalDesignManager;
    [SerializeField] private StartWindowManager startWindowManager;
    [SerializeField] private AdjustmentManager adjustmentManager;
    [SerializeField] private TMP_Text floorHeightLog;
    [SerializeField] private GameObject LogCanvas;
    [SerializeField] private TMP_Text SystemLog;

    private float newWindowLevel;
    private float newFloorLevel;

    public bool isUpdatingHeight;
    [System.Serializable]
    public struct obj
    {
        public Transform trans;
        public Vector3 pos;
        public Vector3 rot;
    }
    public List<obj> list = new List<obj>();
    private Vector3 pos;

    void Update()
    {
        // 如果高度差距超過 threshold
        if (!startWindowManager.isStudyStart) return;
        pos = target.transform.position;

        float curPlayerHeight = dataManager.playerHeight;
        if (studyManager.studyType == "long") curPlayerHeight = longitudinalDesignManager.playerHeight;

        Debug.Log(" curPlayerHeight:" + curPlayerHeight + "centerEye: " + pos);

        float diff = pos.y - curPlayerHeight;
        Debug.Log("diff: " + diff);

        if (Mathf.Abs(diff) > threshold)
        {
            curPlayerHeight = pos.y;
            if (studyManager.studyType == "long") 
                longitudinalDesignManager.playerHeight = curPlayerHeight; 
            else 
                dataManager.playerHeight = curPlayerHeight;

            Debug.Log("Reset height: " + curPlayerHeight + " " + pos.y);
            isUpdatingHeight = true;
            newWindowLevel = MainWindow.transform.localPosition.y + diff;
            newFloorLevel = Debuggers.transform.localPosition.y + diff;
        }

        if(Mathf.Abs(MainWindow.transform.localPosition.y - newWindowLevel) <= 0.01)
        {
            isUpdatingHeight= false;
        }


        smooth();
    }
    private void smooth()
    {
        float y;
        float dy;
        // 使用线性插值（Lerp）来平滑移动物体
        if (isUpdatingHeight && dataManager.windowBehavior == "follow" && !adjustmentManager.isSetting)
        {
            Debug.Log("smoothing");
            y = Mathf.Lerp(MainWindow.transform.localPosition.y, newWindowLevel, Time.deltaTime * movingSpeed);
            MainWindow.transform.localPosition = new Vector3(MainWindow.transform.localPosition.x, y, MainWindow.transform.localPosition.z);
            // isUpdatingHeight = false;

            //LogCanvas.SetActive(true);
            //SystemLog.text = "auto-reset level";

            //if (!adjustmentManager.isSetting)
            //{
            //    LogCanvas.GetComponent<SystemLogManager>().CloseSystemLog(1f);
            //}
        }
        dy = Mathf.Lerp(Debuggers.transform.localPosition.y, newFloorLevel, Time.deltaTime * movingSpeed);
        Debuggers.transform.localPosition = new Vector3(Debuggers.transform.localPosition.x, dy, Debuggers.transform.localPosition.z);
        floorHeightLog.text = " auto " + Debuggers.transform.localPosition.y.ToString("#.###");
        //Debug.Log("smooth " + y);

        // 当物体接近目标位置时，重置时间参数和目标位置，以实现循环移动
        //if (t >= 1f)
        //{
        //    t = 0f;
        //    Vector3 temp = targetPosition;
        //    targetPosition = objectToMove.position;
        //    objectToMove.position = temp;
        //}
    }
    public void heightStop()
    {
        isUpdatingHeight = false;
    }
    public void heightStart()
    {
        // interactable.transform.localPosition = ;
        isUpdatingHeight = true;
    }
}
