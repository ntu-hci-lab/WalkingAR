using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPos2D : MonoBehaviour
{
    [SerializeField]
    private GameObject target;
    [System.Serializable]
    public struct obj
    {
        public Transform trans;
        public Vector3 pos;
        public Vector3 rot;
        public bool rotOn;
    }
    public List<obj> list = new List<obj>();
    private Vector3 pos;
    private Vector3 rot;
    void  Update()
    {
        pos = target.transform.position;
        rot = target.transform.eulerAngles;
        for (int i = 0; i < list.Count; i++)
        {
            list[i].trans.position = new Vector3(pos.x + list[i].pos.x, list[i].pos.y, pos.z + list[i].pos.z);
            if(list[i].rotOn) list[i].trans.eulerAngles = new Vector3(list[i].trans.eulerAngles.x, rot.y + list[i].rot.y, list[i].trans.eulerAngles.z);
            //print(pos.z + " " + list[i].trans.position.z + " " +rot + " "+ list[i].trans.eulerAngles);
        }
    }
}
