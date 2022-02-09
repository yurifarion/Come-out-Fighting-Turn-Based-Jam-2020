using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMove : MapMove
{
	//UI Elements
	public Text fuelnecessaryUI;
	public Text fuelcurrentUI;
	public Text AlertUI;
	public Text noFuel_text;
	public GameObject panel;
	
	public bool clickMapEnable = true;
	//animation
	public Animator _playerAnimator;

    // Start is called before the first frame update
    void Start()
    {
		currentFuel = PlayerPrefs.GetInt("currentFuel",0);
		
    }

    // Update is called once per frame
    void Update()
    {
		Init();
		fuelcurrentUI.text = currentFuel+"";
		_playerAnimator.SetBool("IsMoving",moving);
		
		if(!moving)FindSelectableTiles();
		else Move();
		
		//Mouse Controller
		//if(!panel.activeSelf)
			if(clickMapEnable){
				MouseCommands();
			}
    }
	public void EnableMovement(){
		
			if(currentFuel >= CalculateFuel()){
				PlayerPrefs.SetInt("currentFuel",currentFuel - CalculateFuel());
				useFuel();
				FMODUnity.RuntimeManager.PlayOneShot("event:/SOUNDFX/sfx_m4sherman_move");
				
			}
			else{
				noFuel_text.gameObject.SetActive(true);
				noFuel_text.text = "You don't have enought Fuel to this !";
				StartCoroutine(closeAlert());
			}
	}
	IEnumerator closeAlert(){
		yield return new WaitForSeconds(2f);
		noFuel_text.gameObject.SetActive(false);
	}
	void MouseCommands(){
		if(Input.GetMouseButtonUp(0)){
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			
			RaycastHit hit;
			if(Physics.Raycast(ray, out hit)){
				if(hit.collider.tag == "Tile"){
					Tile t = hit.collider.GetComponent<Tile>();
					
					if(t.selectable){
						MoveToTile(t);
						fuelnecessaryUI.text = "This action will cost "+CalculateFuel()+" of Fuel";
						FMODUnity.RuntimeManager.PlayOneShot("event:/SOUNDFX/sfx_selectMove");
						clickMapEnable = false;
						panel.SetActive(true);
					}
					
				}
				
			}
		}
		
		
	}
}
