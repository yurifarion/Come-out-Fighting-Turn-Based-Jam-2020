using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    public int life = 100;
	public int attackforce = 20;
	
	//UI
	
	public Text attackForce_txt;
	
	//position to instantiate Shot fx and hit effect
	public Transform  shot_pos;
	public Transform  hit_pos;
	public GameObject shot_prefab;
	public GameObject hit_prefab;
	
	//Text mesh
	public TMPro.TextMeshPro life_txt;
	
	//Explosion Death
	public GameObject explosion_Death_prefab;
   
   void Update(){
		life_txt.text = ""+life;
		if(life <= 0) die();
		attackForce_txt.text = "Attack Power: "+attackforce;
	}
   void die(){
	   Instantiate(explosion_Death_prefab,transform.position,transform.rotation);
	   Destroy(this.gameObject);
   }
   public void receiveDamage(int damage){
	   Instantiate(hit_prefab,hit_pos.position,hit_pos.rotation);
	   life -= attackforce;
   }
   public int getAttack(){
	    Instantiate(shot_prefab,shot_pos.position,shot_pos.rotation);
	   return attackforce;
   }
}
