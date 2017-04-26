using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

	Transform target; 
	public Dodgeball dodgeball; 
	public GameObject dodgeballSpawn;

	// Use this for initialization
	void Start () {
		InvokeRepeating ("Shoot", 3f, 6f);  
	}
	
	// Update is called once per frame
	void Update () {
		target = GameObject.FindGameObjectWithTag ("Player").transform; 
			
		LookAtTarget (); 

	}

	void OnCollisionEnter(Collision col){
		if (col.transform.gameObject.tag == "Live_Dodgeball") {
			Destroy (gameObject);
		}
	}

	void LookAtTarget(){
		transform.LookAt (target);
	}

	void Shoot(){
		Instantiate(dodgeball, dodgeballSpawn.transform.position, dodgeballSpawn.transform.rotation);
	}
}
	