using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dodgeballManager : MonoBehaviour
{
    public GameObject dodgeballPrefab;
    public NetworkManager network;
    List<Dodgeball> DodgeBalls = new List<Dodgeball>();

    void Start()
    {
        network = gameObject.transform.parent.GetComponentInChildren<NetworkManager>();
    }

    public void ThrowBall(Vector3 position, Vector3 direction)
    {
        Dodgeball newBall = ThrowNewBall(position, direction);
        network.ThrowBall(newBall.ID, position, direction);
    }
    public void onNetworkThrowBall(Vector3 position, Vector3 direction)
    {
        Dodgeball newBall = ThrowNewBall(position, direction);

    }
    Dodgeball ThrowNewBall(Vector3 position, Vector3 direction)
    {
        Dodgeball newBall = GameObject.Instantiate(dodgeballPrefab, position, Quaternion.identity).GetComponent<Dodgeball>();
        newBall.transform.parent = this.transform;
        newBall.ID = DodgeBalls.Count;
		newBall.isLive = true;
        DodgeBalls.Add(newBall);
        newBall.ThrowBall(direction);
        return newBall;
    }

}
