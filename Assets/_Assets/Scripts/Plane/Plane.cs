using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plane : MonoBehaviour
{
	public float speed = 2f;
	public float timeToAutoDestroy = 10f;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(autoDestroy(timeToAutoDestroy));
    }

    // Update is called once per frame
    void Update()
    {
        // straight line movement
		transform.position += new Vector3(0,0,0.01f * speed);
    }
	IEnumerator autoDestroy(float sec){
		yield return  new WaitForSeconds(sec);
		Destroy(this.gameObject);
	}
}
