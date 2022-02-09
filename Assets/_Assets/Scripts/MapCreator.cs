using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCreator : MonoBehaviour
{
	//HEYYY
	public int[,] map = new int[10,10];
	public GameObject _tilePrefab;
	public bool isMapRandom = false;
	
	public bool isMapCreated = false;
    // Start is called before the first frame update
	void initMap_random(){
		for(int i = 0; i < 10 ; i++){
			for(int j = 0; j < 10 ; j++){
				map[i,j] =  Random.Range(4,7);
				
			}
			
		}
	}
    void Start()
    {
		if(isMapRandom)initMap_random();
		
        CreateMap();
    }
	void DefineType(Tile t,int index){
		if(t != null){
					
					 switch (index){
						 case 0:
						t._type = Tile.TileType.City;
						 break;
						 case 1:
						 t._type = Tile.TileType.Village;
						 break;
						 case 2:
						 t._type = Tile.TileType.Warzone;
						 break;
						 case 3:
						 t._type = Tile.TileType.Forest;
						 break;
						 case 4:
						  t._type = Tile.TileType.Mud;
						 break;
						 case 5:
						  t._type = Tile.TileType.Grass;
						 break;
						 case 6:
						 t._type = Tile.TileType.Street;
						 break;
						 case 7:
						 t._type = Tile.TileType.Undefined;
						 break;
						 default:
						  t._type = Tile.TileType.Undefined;
						 break;
						 
					 }
				 }
	}
	void CreateMap(){
		
		for(int i = 0; i < 10 ; i++){
			
			for(int j = 0; j < 10 ; j++){
				
				Vector3 pos = new Vector3(transform.position.x + i,transform.position.y, transform.position.z + j); // 6 is the wicth and length of the block
				 GameObject g = Instantiate(_tilePrefab,pos, Quaternion.identity);
				 int tileTypeIndex = map[i,j];
				 DefineType(g.GetComponent<Tile>(),tileTypeIndex);
			}
			
		}
		isMapCreated = true;
	}
}
