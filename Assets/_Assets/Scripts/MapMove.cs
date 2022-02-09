using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MapMove : MonoBehaviour
{
	
	
	private TurnManager _turnManager;
	
	public  List<Tile> selectableTiles = new List<Tile>();
	public GameObject[] allTiles;
	
	Stack<Tile> path = new Stack<Tile>(); // the path to destiny
	
	Tile currentTile;// Tile where the player is in.
	
	public int move = 5;
	public float moveSpeed = 2;
	Vector3 velocity = new Vector3();
	Vector3 heading = new Vector3();
	
	public bool moving = false;
	protected bool loadedTiles = false;//this boolean controls if all the tiles around is ready to move
	
	float halfheight; // the halt height of the player so we can know when to place it on top of the tiles
	
	//Fuel to be used
	public int currentFuel = 100;
    
	protected void Init(){
		allTiles = GameObject.FindGameObjectsWithTag("Tile"); // pick all tiles in the map
		halfheight = GetComponent<Collider>().bounds.extents.y;// pick halfheight from the collider
		_turnManager = GameObject.FindGameObjectWithTag("TurnManager").GetComponent<TurnManager>();
	}
	
	public void GetCurrentTile(){
		currentTile = GetTargetTile(gameObject);
		currentTile.current = true;
	}
	
	public Tile GetTargetTile(GameObject target){
		
		RaycastHit hit;
		Tile tile = null;
		if(Physics.Raycast(target.transform.position,-Vector3.up,out hit,1)){
			tile = hit.collider.GetComponent<Tile>();
		}
		return tile;
	}
	public void Move(){
		
		if(path.Count > 0){
			Tile t = path.Peek();
			Vector3 target = t.transform.position;
			target.y += halfheight + t.GetComponent<Collider>().bounds.extents.y;
			
			if(Vector3.Distance(transform.position,target) >= 0.05f){
				
				heading = target - transform.position;
				heading.Normalize();
				
				velocity = heading * moveSpeed;
				
				transform.forward = heading;
				transform.position += velocity * Time.deltaTime;
				
			}
			else {
				transform.position = target;
				path.Pop();
			}
				
		}
		else{
			ClearSelectableTitles();
			moving = false;
			_turnManager.EndTurn();
		}
	}
	protected void useFuel(){
		
			moving = true;
			currentFuel -=  CalculateFuel();
			Debug.Log("use Fuel");
		
	}
	protected int CalculateFuel(){
		int fuel_path = 0;
		foreach(Tile t in path){
			fuel_path += t.fuelused;
		}
		
		return (fuel_path - currentTile.fuelused);
		
		
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
	public void MoveToTile(Tile tile){
		path.Clear();
		tile.target = true;
		Tile next = tile;
		while(next != null){
			path.Push(next);
			next = next.parent;
		}
	}
	public void ComputeAdjacentLists(){
		
		foreach(GameObject tile in allTiles){
			
			Tile t = tile.GetComponent<Tile>();
			t.FindNeighborsMove();
			
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
			
			
			selectableTiles.Add(t);
			t.selectable = true;
			
			
			if(t.distance < move)
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
