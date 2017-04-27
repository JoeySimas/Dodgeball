using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class playerNetwork : MonoBehaviour
{
    private NetworkManager network;
    private Vector3 lastPosition;
    private Quaternion lastRotation;
    public PlayerController player;

    void Start()
    {
        network = GameObject.FindGameObjectWithTag("NETWORK_CONTROLLER").GetComponent<NetworkManager>();
        lastPosition = this.transform.position;
        lastRotation = this.transform.rotation;
    }
    void Update()
    {
        UpdatePosition();
    }
    void UpdatePosition()
    {
        if (!this.transform.position.Equals(lastPosition)
        || !this.transform.rotation.Equals(lastRotation))
        {
            network.sendTransform(this.transform.position,
                                this.transform.rotation);
            lastPosition = this.transform.position;
            lastRotation = this.transform.rotation;
        }
    }
    public void pickupBall(){
        network.PickupBall();
    }
}
