using Oculus.Platform.Samples.VrHoops;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LazyFollowManager : MonoBehaviour
{
    public GameObject player;
    public float rotationSpeed = 10f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Calculate the direction vector from window to player
        Vector3 directionToPlayer = player.transform.position - transform.position;

        // Smoothly rotate the window to face the player
        Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
    }
}
