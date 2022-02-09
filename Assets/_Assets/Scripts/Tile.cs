using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
	//Type of Tile
	public enum TileType {City, Village, Warzone, Forest,Mud,Grass,Street,Undefined};
	public TileType _type = TileType.Undefined;
	
	public bool walkable = true;
	public bool current = false;
	public bool target = false;
	public bool selectable = false;
	
	public List<Tile> adjacentList = new List<Tile>();
	
	//BFS
	public bool visited = false;
	public Tile parent = null;
	public int distance = 0;
	public int fuelused = 0;
	
	//Effect
	public GameObject Volumetric_effect;
	
	//player
	private GameObject _player;

	
	//prefabs
	public GameObject flames;
	public GameObject grass_prefab;
	public GameObject mud_prefab;
	public GameObject mud_street_prefab;
	public GameObject mud_street_curve_prefab;
    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
		flames.SetActive(false);
		ChangeTile();
    }
	// Update is called once per frame
    void Update()
    {
		if(current) walkable = true;
		if(!walkable) selectable = false;
       ChangeColor_effect();
	   ChangeTile();
    }
	
	//Change the Tile based on the type
	private void ChangeTile(){
		switch(_type){
			case TileType.City:
				//GetComponent<Renderer>().material.color = Color.grey;
				fuelused = 7;
				break;
			case TileType.Village:
				//GetComponent<Renderer>().material.color = Color.cyan;
				fuelused = 6;
				break;
			case TileType.Warzone:
				//GetComponent<Renderer>().material.color = Color.red;
				fuelused = 5;
				break;
			case TileType.Forest:
				//GetComponent<Renderer>().material.color = Color.green;
				fuelused = 4;
				break;
			case TileType.Mud:
				//GetComponent<Renderer>().material.color = Color.yellow;
				mud_prefab.SetActive(true);
				fuelused = 2;
				break;
			case TileType.Grass:
				//GetComponent<Renderer>().material.color = Color.magenta;
				grass_prefab.SetActive(true);
				fuelused = 1;
				break;
			case TileType.Street:
				//GetComponent<Renderer>().material.color = Color.white;
				mud_street_prefab.SetActive(true);
				fuelused = 0;
				break;
			default:
			_type = TileType.Undefined;
			GetComponent<Renderer>().material.color = Color.black;
			break;
		}
	}
	// Change color of the cube based on the Status
	private void ChangeColor_effect(){
		
		 if(current){
			Volumetric_effect.SetActive(true);
			Color newColor = Color.magenta;
			newColor.a = 0.5f;
			Volumetric_effect.GetComponent<Renderer>().material.color = newColor;
			
		}
		else if(target){
			Volumetric_effect.SetActive(true);
			Color newColor = Color.green;
			newColor.a = 0.5f;
			Volumetric_effect.GetComponent<Renderer>().material.color = newColor;
		}
		else if(selectable){
			Volumetric_effect.SetActive(true);
			Color newColor = Color.red;
			newColor.a = 0.3f;
			Volumetric_effect.GetComponent<Renderer>().material.color = newColor;
		}
		else if(!walkable){
			/*Volumetric_effect.SetActive(true);
			Color newColor = Color.black;
			newColor.a = 0.5f;
			Volumetric_effect.GetComponent<Renderer>().material.color =  newColor;*/
			flames.SetActive(true);
			Volumetric_effect.SetActive(false);
		}
		else{
			Volumetric_effect.SetActive(false);
		}
	}
	//Reset all values to default
    public void Reset(){
	  current = false;
	  target = false;
	  selectable = false;
	  adjacentList.Clear();
	  visited = false;
	  parent = null;
	  distance = 0;
	}
	public void FindNeighborsMove(){
		
		Reset();
		CheckTileMove(Vector3.forward); // Check tiles on forward
		CheckTileMove(-Vector3.forward); // Check tiles on back
		CheckTileMove(Vector3.right); // Check tiles on right
		CheckTileMove(-Vector3.right); // Check tiles on left
	}
	public void FindNeighborsAttack(){
		
		Reset();
		CheckTileAttack(Vector3.forward); // Check tiles on forward
		CheckTileAttack(-Vector3.forward); // Check tiles on back
		CheckTileAttack(Vector3.right); // Check tiles on right
		CheckTileAttack(-Vector3.right); // Check tiles on left
	}
	/*
	 it checks every collider in the neightbor of the this tile within a size of 0.25(25%) of the tile and
	 if they are Tiles, walkable and not being occupied then add
	 to the list of adjacentList
	*/
	private void CheckTileMove(Vector3 direction){
		Vector3 halfExtents = new Vector3(0.25f,0.25f,0.25f);
		Collider[] colliders = Physics.OverlapBox(transform.position + direction,halfExtents);
		
		foreach(Collider item in colliders){
			Tile tile = item.GetComponent<Tile>();
			
			if(tile != null && tile.walkable){ // check if it is a tile and if it is walkable
				RaycastHit hit;
				 
				if(!Physics.Raycast(tile.transform.position,Vector3.up, out hit, 1)){ // check if there is something above it
					
					adjacentList.Add(tile); // then add to the list
					
				}
				
				
			}
		}
	}
	//On attack you add the tile even if it have something above it 
	private void CheckTileAttack(Vector3 direction){
		Vector3 halfExtents = new Vector3(0.25f,0.25f,0.25f);
		Collider[] colliders = Physics.OverlapBox(transform.position + direction,halfExtents);
		
		foreach(Collider item in colliders){
			Tile tile = item.GetComponent<Tile>();
			
			if(tile != null && tile.walkable){ // check if it is a tile and if it is walkable
				RaycastHit hit;		
					adjacentList.Add(tile); // then add to the list
				
			}
		}
	}
	
	
}
