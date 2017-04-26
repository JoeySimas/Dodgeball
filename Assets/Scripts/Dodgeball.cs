using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dodgeball: MonoBehaviour {

	public float throwSpeed;
	public float hitCount;
	public bool isLive;
	public AudioClip hitClip; 

	Rigidbody rb;
	AudioSource audioSource; 

	TrailRenderer tail;


	// Use this for initialization
	void Start () {
		hitCount = 0;
		isLive = true; 
		rb = gameObject.GetComponent<Rigidbody> ();
		audioSource = gameObject.GetComponent<AudioSource> (); 
		rb.AddForce (transform.forward * throwSpeed * 10); 
		tail = gameObject.GetComponent<TrailRenderer> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (hitCount >= 3) {
			isLive = false; 
			transform.gameObject.tag = "Dodgeball";
			tail.enabled=false;
		}
	}

	void OnCollisionEnter(Collision col){
		hitCount = hitCount + 1; 
		if (isLive) {
			audioSource.PlayOneShot (hitClip);
		}
	}
}
