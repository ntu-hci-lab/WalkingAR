using TMPro;
using UnityEngine;

public class StartWindowManager : MonoBehaviour
{
    public bool isStudyStart = false;
    public GameObject centerAnchorEye;
    public TMP_Text SystemLog;
    public TMP_Text PlayerHeightLog;
    [SerializeField] private GameObject LogCanvas;
    [SerializeField] private DataManager dataManager;
    //[SerializeField] private LongitudinalDesignManager longitudinalDesignManager;

    public void setTask(string task)
    {
        dataManager.task = task;
    }

    public void setEnv(string env)
    {
        dataManager.env = env;
    }

    public void setPlayerHeight() { 
        dataManager.playerHeight = centerAnchorEye.transform.position.y;
        //longitudinalDesignManager.playerHeight = centerAnchorEye.transform.position.y;
    }

    public void studyStart()
    {
        //if(longitudinalDesignManager.userID == -1)
        //{
        //    SystemLog.text = "Please Login";
        //    return;
        //}
        
        isStudyStart = true;
        dataManager.studyStartTime = Time.time;
        // longitudinalDesignManager.studyStartTime = Time.time;
        
        setPlayerHeight();
        this.gameObject.SetActive(false);

        // Fetch Data
        // longitudinalDesignManager.FetchData();
    }

    public void confrimPlayerHeight()
    {
        setPlayerHeight();
        PlayerHeightLog.text = dataManager.playerHeight.ToString();
    }
}
