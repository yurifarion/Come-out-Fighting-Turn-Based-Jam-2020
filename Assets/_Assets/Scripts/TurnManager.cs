using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
	public string turnResponsable = "Player";
	public string typeOfTurn = "Move";
	private bool finishTurn = false;
	private GameObject _player;
	private GameObject _enemy;
	public GameObject actionUI;
	private CameraFollow  _camFollow;
	public Fade _fadeManager;
	public GameObject skip_UI;
	
	public GameObject planeprefab;
	public Transform planeSpawnPoint;
	
	//tiles of the map
	public GameObject[] allTiles;
    // Start is called before the first frame update
    void Start()
    {
		
		allTiles = GameObject.FindGameObjectsWithTag("Tile"); // pick all tiles in the map
        _player = GameObject.FindGameObjectWithTag("Player");
		_enemy = GameObject.FindGameObjectWithTag("Enemy");
		_camFollow = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraFollow>();
		StartTurn();
    }
	public void StartTurn(){
		
		if(typeOfTurn == "Move"){
			
			if(turnResponsable == "Player"){
				skip_UI.SetActive(true);
				_camFollow.target = _player;
				allfalse();
				_player.GetComponent<PlayerMove>().enabled = true;
			}
			else if(turnResponsable == "Enemy"){
				skip_UI.SetActive(false);
				_camFollow.target = _enemy;
				allfalse();
				_enemy.GetComponent<EnemyMove>().enabled = true;
				
			}
			
		}
		if(typeOfTurn == "Action"){
			if(turnResponsable == "Player"){
				skip_UI.SetActive(true);
				_camFollow.target = _player;
				allfalse();
				_player.GetComponent<PlayerActions>().enabled = true;
				_player.GetComponent<PlayerMapAction>().isAbleToAttack = true;
				actionUI.SetActive(true);
				//_player.GetComponent<PlayerMapAction>().enabled = true;
			}
			else if(turnResponsable == "Enemy"){
				skip_UI.SetActive(false);
				_camFollow.target = _enemy;
				allfalse();
				_enemy.GetComponent<EnemyAction>().enabled = true;
				
			}
		}
		if(typeOfTurn == "Interval"){
			allfalse();
			Debug.Log("Start Interval");
			_fadeManager.FadeOut();
			
		}
		
	}
	void allfalse(){
		if(_enemy != null){
		_player.GetComponent<PlayerMove>().enabled = false;
		_enemy.GetComponent<EnemyMove>().enabled = false;
		_enemy.GetComponent<EnemyAction>().enabled = false;
		_player.GetComponent<PlayerMapAction>().enabled = false;
		_player.GetComponent<PlayerActions>().enabled = false;
		actionUI.SetActive(false);
		}
		else{
		_player.GetComponent<PlayerMove>().enabled = false;
		_player.GetComponent<PlayerMapAction>().enabled = false;
		_player.GetComponent<PlayerActions>().enabled = false;
		actionUI.SetActive(false);
		}
		
	}
	public void EndTurn(){
		Debug.Log(turnResponsable+" "+typeOfTurn);
		if(typeOfTurn == "Move"){
			if(turnResponsable == "Player"){
				typeOfTurn = "Action";
				StartTurn();
				return;
			}
			else if(turnResponsable == "Enemy"){
				typeOfTurn = "Action";
				StartTurn();
				return;
			}
		}
		if(typeOfTurn == "Action"){
			if(turnResponsable == "Player"){
				if(_enemy != null){
					_player.GetComponent<PlayerActions>().CancelAttack();
					turnResponsable = "Enemy";
					typeOfTurn = "Move";
					_player.GetComponent<PlayerMapAction>().attacking = false;
					_enemy.GetComponent<EnemyAction>().attacking = false;
				}
				else{
					_player.GetComponent<PlayerActions>().CancelAttack();
					turnResponsable = "Player";
					typeOfTurn = "Interval";
					_player.GetComponent<PlayerMapAction>().attacking = false;
				}
				StartTurn();
				return;
			}
			else if(turnResponsable == "Enemy"){
				Debug.Log("EndTurn Atttack Enemy");
				turnResponsable = "Player";
				typeOfTurn = "Interval";
				_player.GetComponent<PlayerMapAction>().attacking = false;
				_enemy.GetComponent<EnemyAction>().attacking = false;
				StartTurn();
				return;
			}
		}
		//part of the turn that bombing happen
		if(typeOfTurn == "Interval"){
			// pick 3 random tiles and make they burn but first check if it is a current Tile of any of the players
			int count = 0;
			
			while(count < 3){
				int rand = Random.Range(0,allTiles.Length);
				
				if( !allTiles[rand].GetComponent<Tile>().current && allTiles[rand].GetComponent<Tile>().walkable){
					allTiles[rand].GetComponent<Tile>().walkable = false;
					allTiles[rand].GetComponent<Tile>().selectable = false;
					count++;
				}
			}
			Instantiate(planeprefab,planeSpawnPoint.position,planeSpawnPoint.rotation);
			typeOfTurn = "Move";
			StartTurn();
			return;
		}
		
	}

}
