using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class networkPlayerController : MonoBehaviour
{
    public GameObject DodgeBallHeld;

    void Start()
    {
        DodgeBallHeld.SetActive(false);
    }
    public void onPickupDodgeball()
    {
        DodgeBallHeld.SetActive(true);
    }
    public void onThrowDodgeball()
    {
        DodgeBallHeld.SetActive(false);
    }
    void OnCollisionEnter(Collision col)
    {
        Dodgeball ball = col.gameObject.GetComponent<Dodgeball>();
		Debug.Log(ball.isLive);
        if (ball && ball.isLive)
        {
            StartCoroutine(dieCycle());
        }
    }
    IEnumerator dieCycle()
    {
		MeshRenderer meshR = GetComponent<MeshRenderer>();
		Color defaultColor = meshR.material.color;
        meshR.material.color = Color.black;
        yield return new WaitForSeconds(5f);
        meshR.material.color = defaultColor;
    }

}
