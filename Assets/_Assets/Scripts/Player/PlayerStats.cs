using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PlayerStats : MonoBehaviour
{
	public int life = 100;
	public int attackforce = 20;
	public int fuelTokens = 0;
	public int ammoTokens = 0;
	public int partTokens = 0;
	
	//UI
	public Text life_txt;
	public Text attackForce_txt;
	public Text fuelTokens_txt;
	public Text ammoTokens_txt;
	public Text partTokens_txt;
	
	//position to instantiate Shot fx and hit effect
	public Transform  shot_pos;
	public Transform  hit_pos;
	public GameObject shot_prefab;
	public GameObject hit_prefab;
	
	//UI
	
	public Transform spawnPoint_fuel;
	public Transform spawnPoint_Ammo;
	public Transform spawnPoint_Part;
	
	private List<GameObject> fuelTokens_UI = new List<GameObject>();
	private List<GameObject> PartTokens_UI = new List<GameObject>();
	private List<GameObject> AmmoTokens_UI = new List<GameObject>();
	//UI Prefab
	public GameObject prefab_fuel;
	public GameObject prefab_Ammo;
	public GameObject prefab_Part;
	//UI temp
	private int temp_fuel = 0;
	private int temp_Ammo = 0;
	private int temp_Part = 0;
	
	void Start(){
		life = PlayerPrefs.GetInt("Life",99);
		fuelTokens = PlayerPrefs.GetInt("fueltokens",0);
		ammoTokens = PlayerPrefs.GetInt("ammotokens",0);
		partTokens = PlayerPrefs.GetInt("parttokens",0);
	}
	void Update(){
		if(life <= 0) SceneManager.LoadScene("GameOver");
		if(temp_fuel != fuelTokens)UpdateFuelToken();
		if(temp_Ammo != ammoTokens)UpdateAmmoToken();
		if(temp_Part != partTokens)UpdatePartToken();
		
		life_txt.text = ""+life;
		attackForce_txt.text = "Attack Power: "+attackforce;
		fuelTokens_txt.text = ""+ fuelTokens;
		ammoTokens_txt.text = ""+ ammoTokens;
		partTokens_txt.text = ""+ partTokens;
		
		 if(Input.GetKeyDown(KeyCode.Escape)){
			 Application.Quit();
		 }
	}
   void UpdateFuelToken(){
	   //clear all current Token
	   foreach(GameObject t in fuelTokens_UI){
		   Destroy(t);
	   }
	   int count = 1;
	   while(count < fuelTokens){
		    Vector3 pos = new Vector3(0,count*10f ,0);
		   GameObject  temp = Instantiate(prefab_fuel,spawnPoint_fuel.position,spawnPoint_fuel.rotation);
		   temp.transform.parent = spawnPoint_fuel;
		   temp.transform.localPosition = pos;
		   temp.transform.localScale = new Vector3(1f,1f,1f);
		   fuelTokens_UI.Add(temp);
		   count++;
	   }
	   temp_fuel = fuelTokens;
	    PlayerPrefs.SetInt("fueltokens",temp_fuel);
   }
   void UpdatePartToken(){
	   foreach(GameObject t in PartTokens_UI){
		   Destroy(t);
	   }
	    int count = 1;
	   while(count < partTokens){
		   Vector3 pos = new Vector3(0,count*10f ,0);
		   GameObject  temp = Instantiate(prefab_Part,spawnPoint_Part.position,spawnPoint_Part.rotation);
		   temp.transform.parent = spawnPoint_Part;
		   temp.transform.localPosition = pos;
		    temp.transform.localScale = new Vector3(1f,1f,1f);
		   PartTokens_UI.Add(temp);
		   count++;
	   }
	   temp_Part = partTokens;
	    PlayerPrefs.SetInt("parttokens",temp_Part);
   }
   void UpdateAmmoToken(){
	    foreach(GameObject t in AmmoTokens_UI){
		   Destroy(t);
	   }
	   int count = 1;
	   while(count < ammoTokens){
		    Vector3 pos = new Vector3(0,count*10f ,0);
		   GameObject  temp = Instantiate(prefab_Ammo,spawnPoint_Ammo.position,spawnPoint_Ammo.rotation);
		   temp.transform.parent = spawnPoint_Ammo;
		   temp.transform.localPosition = pos;
		    temp.transform.localScale = new Vector3(1f,1f,1f);
		   AmmoTokens_UI.Add(temp);
		   count++;
	   }
	   temp_Ammo = ammoTokens;
	    PlayerPrefs.SetInt("ammotokens",temp_Ammo);
   }
   public void receiveDamage(int damage){
	   Instantiate(hit_prefab,hit_pos.position,hit_pos.rotation);
	   life -= attackforce;
	   PlayerPrefs.SetInt("Life",life);
   }
   public int getAttack(){
	   Instantiate(shot_prefab,shot_pos.position,shot_pos.rotation);
	   return attackforce;
   }
}
