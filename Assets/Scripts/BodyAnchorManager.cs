using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyAnchorManager : MonoBehaviour
{
    [SerializeField] private AdjustmentManager adjustmentManager;
    // Start is called before the first frame update
    

    // Update is called once per frame
    void Update()
    {
        if (adjustmentManager.isSetting)
        {
            adjustmentManager.turnAhead();
        }
    }
}
