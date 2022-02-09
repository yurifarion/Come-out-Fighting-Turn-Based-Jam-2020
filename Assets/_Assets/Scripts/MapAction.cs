using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MapAction : MonoBehaviour
{
	
	
	public TurnManager _turnManager;
	
	public  List<Tile> selectableTiles = new List<Tile>();
	public GameObject[] allTiles;
	
	Stack<Tile> path = new Stack<Tile>(); // the path to destiny
	
	Tile currentTile;// Tile where the player is in.
	
	Vector3 velocity = new Vector3();
	Vector3 heading = new Vector3();
	
	protected List<GameObject> enemiesOnView = new List<GameObject>();
	
	public bool attacking = false;
	protected bool loadedTiles = false;//this boolean controls if all the tiles around is ready to move
	
	public int attackDistance;
	
	float halfheight; // the halt height of the player so we can know when to place it on top of the tiles
	
	//position to instantiate Shot fx and hit effect

    
	protected void Init(){
		allTiles = GameObject.FindGameObjectsWithTag("Tile"); // pick all tiles in the map
		halfheight = GetComponent<Collider>().bounds.extents.y;// pick halfheight from the collider
		_turnManager = GameObject.FindGameObjectWithTag("TurnManager").GetComponent<TurnManager>();
	}
	
	public void GetCurrentTile(){
		currentTile = GetTargetTile(gameObject);
		currentTile.current = true;
	}
	public void LookAtTarget(Transform target){
		heading = target.position - transform.position;
		transform.forward = heading;
	}
	public Tile GetTargetTile(GameObject target){
		
		RaycastHit hit;
		Tile tile = null;
		if(Physics.Raycast(target.transform.position,-Vector3.up,out hit,1)){
			tile = hit.collider.GetComponent<Tile>();
		}
		return tile;
	}
	
	protected void ClearSelectableTitles(){
		loadedTiles = false;
		if(currentTile != null){
			
			currentTile.current = false;
			currentTile = null;
			
		}
		foreach(Tile tile in selectableTiles){
			tile.Reset();
		}
	}
	protected GameObject AttackTile(Tile tile){
		
		RaycastHit hit;
		 
		if(Physics.Raycast(tile.transform.position,Vector3.up, out hit, 1)){ // check if there is something above it
	
					if(hit.transform.gameObject.tag == "Enemy"){//if it is a enemy returns its gameobject
						
						return hit.transform.gameObject;
					}	
					else return null ;// if it isnot return null
		}
		else{
			return null;//if it is empty above it returns null
		}
	}
	public void ComputeAdjacentLists(){
		
		foreach(GameObject tile in allTiles){
			
			Tile t = tile.GetComponent<Tile>();
			t.FindNeighborsAttack();
			
		}
	}
	public void FindSelectableTiles(){
		
		ComputeAdjacentLists();
		GetCurrentTile();
		
		Queue<Tile> process = new Queue<Tile>();
		
		process.Enqueue(currentTile);
		currentTile.visited = true;
		
		while(process.Count > 0){
			Tile t = process.Dequeue();
			
			//it makes sure it just select the tile on the same axis ( it wont add diagonals tiles)
			if( !((Mathf.Abs((t.gameObject.transform.position.x -  currentTile.gameObject.transform.position.x)) > 0 ) && (Mathf.Abs((t.gameObject.transform.position.z -  currentTile.gameObject.transform.position.z)) > 0 ))){
				
				selectableTiles.Add(t);
				t.selectable = true;
			}
			if(t.distance < attackDistance)
			{
				foreach(Tile tile in t.adjacentList){
					
					if(!tile.visited){
						
						tile.parent = t;
						tile.visited = true;
						tile.distance = 1 + t.distance;
						process.Enqueue(tile);
					}
				}
			}
		}
		loadedTiles = true;
	}
}
