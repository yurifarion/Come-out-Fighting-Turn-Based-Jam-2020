using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndLine : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(this.transform.position.z >= 8.6){
			if(Application.loadedLevelName == "Level1"){
				Application.LoadLevel("Level2");
			}
			else if(Application.loadedLevelName == "Level2"){
				Application.LoadLevel("credits_scene");
			}
		}
    }
	
	
}
