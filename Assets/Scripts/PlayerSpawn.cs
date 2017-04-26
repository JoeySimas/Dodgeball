using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawn : MonoBehaviour {

	public GameObject player; 

	// Use this for initialization
	void Start () {
		Instantiate (player, transform.position, transform.rotation); 
		GameObject.FindGameObjectWithTag ("Player");
	}
	
	// Update is called once per frame
	void Update () {
		if(GameObject.FindGameObjectWithTag("Player")==null){
			Instantiate (player, transform.position, transform.rotation); 
		}
	}
}
