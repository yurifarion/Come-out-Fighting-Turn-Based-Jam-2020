using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerActions : MonoBehaviour
{
	private PlayerStats _playerStats;
	private PlayerMove _playerMove;
	private TurnManager _turnManager;
	public GameObject scavengeTXT;
	public GameObject actions_btn;
	public GameObject cancel_Attack_btn;
	public GameObject confirmation_box;
	public GameObject skip_gameobject;
	public Text AlertUI;
	public Text fuelcurrentUI;
    // Start is called before the first frame update
    void Start()
    {
        _playerStats = GetComponent<PlayerStats>();
		_playerMove = GetComponent<PlayerMove>();
		_turnManager = GameObject.FindGameObjectWithTag("TurnManager").GetComponent<TurnManager>();
    }
	
	public void Repair(){
		actions_btn.SetActive(false);
		FMODUnity.RuntimeManager.PlayOneShot("event:/SOUNDFX/sfx_repairing");
		int temp = 0;
		if((_playerStats.life + _playerStats.partTokens * 5) > 100){
			
			 temp = 100 - _playerStats.life;
			
			_playerStats.life += temp;
			_playerStats.partTokens -= (temp/5);
			
		}
		else{
			 temp = _playerStats.partTokens * 5;
			_playerStats.life += temp;
			_playerStats.partTokens -= (temp / 5);
		}
		confirmation_box.SetActive(true);
		AlertUI.text = "You repair your Tank with "+temp+" parts.";
		//StartCoroutine(EndTurn(0.5f));
	}
	public void Scavenge(){
		actions_btn.SetActive(false);
		FMODUnity.RuntimeManager.PlayOneShot("event:/SOUNDFX/sfx_scavengeResult");
		//Fuel
		int rand = Random.Range(1,101);
		
		int amountOfFuelFound = 0;
		if(rand <= 3) amountOfFuelFound = 6;
		else if(rand > 3 && rand <= 7) amountOfFuelFound = 5;
		else if(rand > 7 && rand <= 15) amountOfFuelFound = 4;
		else if(rand > 15 && rand <= 25) amountOfFuelFound = 3;
		else if(rand > 25 && rand <= 40) amountOfFuelFound = 2;
		else if(rand > 40 && rand <= 75) amountOfFuelFound = 1;
		else if(rand > 75 ) amountOfFuelFound = 0;
			
		//AMMO
		rand = Random.Range(1,101);
		
		int amountOfAmmoFound = 0;
        if(rand <= 10) amountOfAmmoFound = 3;
        else if(rand > 10 && rand <= 21) amountOfAmmoFound = 2;
        else if(rand > 21 && rand <= 50) amountOfAmmoFound = 1;
        else if(rand > 51 )amountOfAmmoFound = 0;
		
		//PART
		rand = Random.Range(1,101);
		
		int amountOfPartsFound = 0;
		if(rand <= 21) amountOfPartsFound = 4;
		else if(rand > 21 && rand <= 50) amountOfPartsFound = 3;
		else if(rand > 50 && rand <= 75) amountOfPartsFound = 2;
		else if(rand > 75 && rand <= 100) amountOfPartsFound = 1;
	
		confirmation_box.SetActive(true);
		AlertUI.text = "Scavenge: You found "+amountOfFuelFound+" FuelTokens,"+ amountOfAmmoFound +" AmmoTokens and "+ amountOfPartsFound +" PartTokens";
		_playerStats.fuelTokens += amountOfFuelFound;
		_playerStats.ammoTokens += amountOfAmmoFound;
		_playerStats.partTokens += amountOfPartsFound;
		//StartCoroutine(EndTurn(2f));
	}
	public void Refuel(){
		actions_btn.SetActive(false);
		FMODUnity.RuntimeManager.PlayOneShot("event:/SOUNDFX/sfx_refuel");
		int temp = 0;
		if((_playerMove.currentFuel + _playerStats.fuelTokens) > 10){
			
			 temp = 10 - _playerMove.currentFuel;
			
			_playerMove.currentFuel += temp;
			_playerStats.fuelTokens -= temp;
			
			PlayerPrefs.SetInt("currentFuel",_playerMove.currentFuel);
		}
		else{
			 temp = _playerStats.fuelTokens;
			_playerMove.currentFuel += temp;
			_playerStats.fuelTokens -= temp;
			PlayerPrefs.SetInt("currentFuel",_playerMove.currentFuel);
		}
		fuelcurrentUI.text = _playerMove.currentFuel+"";
		confirmation_box.SetActive(true);
		AlertUI.text = "You refuel your Tank with "+temp+" fuel tokens.";
		//StartCoroutine(EndTurn(0.5f));
	}
	public void Attack(){
		GetComponent<PlayerMapAction>().enabled = true;
		actions_btn.SetActive(false);
		cancel_Attack_btn.SetActive(true);
	}
	public void CancelAttack(){
		GetComponent<PlayerMapAction>().enabled = false;
		actions_btn.SetActive(true);
		cancel_Attack_btn.SetActive(false);
	}
	public void CloseUI(){
		if(_turnManager.turnResponsable == "Player" && _turnManager.typeOfTurn == "Move"){
			confirmation_box.SetActive(false);
			StartCoroutine(EnableMovementDelay(0.02f));
		}
		else{
			confirmation_box.SetActive(false);
			StartCoroutine(EndTurn(0.01f));
		}			
	}
	public void Confirm_UI(){
		if(_turnManager.turnResponsable == "Player" && _turnManager.typeOfTurn == "Move"){
			
			_playerMove.EnableMovement();
			CloseUI();
			//skip_gameobject.SetActive(false);
			
		}else {
			CloseUI();
			
		}
	}
	IEnumerator EndTurn(float delay){
		yield return new  WaitForSeconds(delay);
		scavengeTXT.SetActive(false);
		_turnManager.EndTurn();
	}
	IEnumerator EnableMovementDelay(float delay){
		yield return new  WaitForSeconds(delay);
		_playerMove.clickMapEnable = true;
	}
}
