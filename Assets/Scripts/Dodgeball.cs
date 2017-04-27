using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dodgeball : MonoBehaviour
{

    public float throwSpeed;
    public float hitCount;
    public bool isLive;
    public AudioClip hitClip;
    public int ID;

    Rigidbody rb;
    AudioSource audioSource;
    TrailRenderer tail;


    void Start()
    {
        Init();
    }
    void Init()
    {
        if (!rb)
            rb = gameObject.GetComponent<Rigidbody>();
        if (!tail)
            tail = gameObject.GetComponent<TrailRenderer>();
        if (!audioSource)
            audioSource = gameObject.GetComponent<AudioSource>();

        tail.enabled = false;
        transform.gameObject.tag = "Dodgeball";
    }
    void OnCollisionEnter(Collision col)
    {
        if (isLive && hitCount < 3)
        {
            hitCount = hitCount + 1;
            audioSource.PlayOneShot(hitClip);
        }
        else if (isLive && hitCount >= 3)
        {
            isLive = false;
            tail.enabled = false;
        }
    }
    public void ThrowBall(Vector3 direction)
    {
        Init();
        hitCount = 0;
        StartCoroutine(GoLive());
		//tail.enabled = true;
        rb.AddForce(direction * throwSpeed * 10);
    }
    IEnumerator GoLive()
    {
        yield return new WaitForSeconds(0.2f);
        isLive = true;
        
    }
}
