using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fade : MonoBehaviour
{
	public float intervalTime = 0.05f;
	private Animator anim;
	private TurnManager _turnManager;

	void Start(){
		anim = GetComponent<Animator>();
		_turnManager = GameObject.FindGameObjectWithTag("TurnManager").GetComponent<TurnManager>();
	}
    public void FadeIn(){
		Debug.Log("Fade In");
		
		anim.SetBool("FadeOut",false);
		_turnManager.EndTurn();//Finish the interval Turn
	}
	public void FadeOut(){
		Debug.Log("Fade Out");
		anim.SetBool("FadeOut",true);

		StartCoroutine(timer());
		FMODUnity.RuntimeManager.PlayOneShot("event:/SOUNDFX/sfx_phase1Bombing");
	}
	IEnumerator timer(){
		yield return  new WaitForSeconds(intervalTime);
		FadeIn();
	}
}
