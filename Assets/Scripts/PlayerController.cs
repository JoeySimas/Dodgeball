using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	public float moveSpeed = 2f;
	public float sensitivity = 2f;
	public float jump = 2f;
	public GameObject playerCamera; 
	public GameObject dodgeballSpawner; 
	public GameObject dodgeballHolding; 
	public Dodgeball dodgeball; 

	CharacterController player;
	float moveFB;
	float moveLR; 
	float rotationX;
	float rotationY;
	float verticalVelocity; 

	bool isGrounded; 
	bool isJumping;
	bool isDashing;
	bool hasDodgeball;


	void Start () {
		player = GetComponent<CharacterController> (); 
		isGrounded = true;
		isJumping = false;
		isDashing = false;
		hasDodgeball = false;
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = (false);
	}

	void Update () {

		if (hasDodgeball == false && Input.GetButton ("Grab")) {
			//hasDodgeball = true; 
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast (ray, out hit, 3)) {
				print ("hit");
				if (hit.transform.gameObject.tag == "Dodgeball" || hit.transform.gameObject.tag == "Live_Dodgeball") {
					Destroy (hit.transform.gameObject);
					hasDodgeball = true; 
				}
			}
				
		}

		// Movement
		moveFB = Input.GetAxis ("Vertical") * moveSpeed;
		moveLR = Input.GetAxis ("Horizontal") * moveSpeed; 

		rotationX = Input.GetAxis ("Mouse X") * sensitivity;
		rotationY -= Input.GetAxis ("Mouse Y") * sensitivity;
		rotationY = Mathf.Clamp (rotationY, -90f, 60f); 

		Vector3 movement = new Vector3 (moveLR, verticalVelocity, moveFB);
		transform.Rotate (0, rotationX, 0);
		playerCamera.transform.localRotation = Quaternion.Euler (rotationY, 0, 0); 
		movement = transform.rotation * movement; 
		player.Move (movement * Time.deltaTime);

		if (Input.GetButtonDown ("Throw")) {
			if (hasDodgeball == true) {
				Instantiate (dodgeball, dodgeballSpawner.transform.position, dodgeballSpawner.transform.rotation);
				hasDodgeball = false;
			}
		}

		// Jump & Double Jump
		if (Input.GetButtonDown ("Jump")) {
			if (isGrounded == true) {
				Debug.Log ("Jump");
				player.Move (transform.up * jump); 
				isGrounded = false;
				isJumping = true;
			} else if (isJumping == true) {
				Debug.Log ("Double Jump");
				player.Move (transform.up * jump); 
				isJumping = false;
			}
		}

		// Dash
		if (isDashing == false && Input.GetButtonDown ("Dash")) {
			Debug.Log ("Dash");
			Invoke ("Dash", 0f); 
		}

		if (hasDodgeball == false) {
			dodgeballHolding.SetActive (false);
		} else {
			dodgeballHolding.SetActive (true);
		}

	}

	void FixedUpdate(){
		if (player.isGrounded == false) {
			verticalVelocity += Physics.gravity.y * Time.deltaTime; 
		} else {
			verticalVelocity = 0f;
			isGrounded = true;
		}
	}
		
	void Dash(){
		isDashing = true; 
		moveSpeed = 36f;
		Invoke ("ResetSpeed", .1f);
		Invoke ("ResetDash", .5f);
	}
		
	void ResetSpeed(){
		moveSpeed = 6f; 
	}

	void ResetDash(){
		isDashing = false;
	}

	void OnCollisionEnter(Collision col){
		if (col.transform.gameObject.tag == "Live_Dodgeball") {
			Destroy (gameObject);
		}
	}
}
