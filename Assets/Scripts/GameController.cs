using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{

    public GameObject player;
    private GameObject alivePlayer;
    public Transform spawnPoint;

    // Use this for initialization
    void Start()
    {
        Spawn();
    }
    public void Spawn()
    {
        alivePlayer = Instantiate(player, spawnPoint.position, spawnPoint.rotation);
    }
    public void onDeath()
    {
        StartCoroutine(dieCycle());
    }
    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
    IEnumerator dieCycle()
    {
        alivePlayer.SetActive(false);
        yield return new WaitForSeconds(5f);
        alivePlayer.SetActive(true);
        alivePlayer.transform.position = spawnPoint.position;
    }
}
