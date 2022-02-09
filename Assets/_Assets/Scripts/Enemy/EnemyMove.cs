using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MapMove
{
	public bool playerSeen = false;//controls if enemy saw or not the player
	private GameObject _player;
	public float visibleRay = 5.0f;
	public Animator _EnemyAnimator;
    // Start is called before the first frame update
    void Start()
    {
		
         _player = GameObject.FindGameObjectWithTag("Player");
		 
    }
	
    // Update is called once per frame
    void Update()
    {
		
		_EnemyAnimator.SetBool("IsMoving",moving);
		Init();
		if(!moving)FindSelectableTiles();
		else Move();
		
		
		CheckPlayer();
		 EnemyMoveNow();
    }
	//check if player is visible or not into the ray;
	void CheckPlayer(){
		if(Vector3.Distance(_player.transform.position,transform.position) < visibleRay) playerSeen = true;
		
	}
	public void EnemyMoveNow(){
		FMODUnity.RuntimeManager.PlayOneShot("event:/SOUNDFX/sfx_panzerIV_move");
		//Create a list of available Tile to go
		List<Tile> availableTiles = new List<Tile>();
		foreach(GameObject t in allTiles){
			if(t.GetComponent<Tile>() != null && t.GetComponent<Tile>().selectable == true){
				availableTiles.Add(t.GetComponent<Tile>());
			}
		}
		
		if(availableTiles.Count > 0 && !moving && loadedTiles){
			//check if the enemy is seeing the player if yes then it goes to the closest tile (selectable) to he can to the player
			if(playerSeen){
				GameObject closestTile = availableTiles[0].gameObject;
				foreach(Tile t in availableTiles){
					if( Vector3.Distance(_player.transform.position,t.gameObject.transform.position) < Vector3.Distance(_player.transform.position,closestTile.transform.position)){
						closestTile = t.gameObject;
					}
				}
				MoveToTile(closestTile.GetComponent<Tile>());
			}
			//check if the enemy is seeing the player if no then it goes to random tile
			else{
				int rand = Random.Range(0,availableTiles.Count);
				Debug.Log("Enemy move"+rand+"Count"+availableTiles.Count);
				MoveToTile(availableTiles[rand]);
			}
			useFuel();
		}
	}
	//Area where the enemy is seeing 
	private void OnDrawGizmos()
	{
		float radius = visibleRay;
		Gizmos.color = Color.white;
		float theta = 0;
		float x = radius * Mathf.Cos(theta);
		float y = radius * Mathf.Sin(theta);
		Vector3 pos = transform.position + new Vector3(x, 0, y);
		Vector3 newPos = pos;
		Vector3 lastPos = pos;
		
		for (theta = 0.1f; theta < Mathf.PI * 2; theta += 0.1f)
		{
			x = radius * Mathf.Cos(theta);
			y = radius * Mathf.Sin(theta);
			newPos = transform.position + new Vector3(x, 0, y);
			Gizmos.DrawLine(pos, newPos);
			pos = newPos;
		}
		Gizmos.DrawLine(pos, lastPos);
	}
	
}
