using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAction : MonoBehaviour
{
    playerNetwork PlayerNetwork;


    // Use this for initialization
    void Start()
    {
        PlayerNetwork = GetComponent<playerNetwork>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKey)
            checkForInput();
    }

    void checkForInput()
    {
        string key = Input.inputString;
        switch (key)
        {
            
        }

    }
}
