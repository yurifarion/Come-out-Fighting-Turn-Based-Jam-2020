using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PlayerMapAction : MapAction
{
	private PlayerStats _stats;
	public Animator _playerAnimator;
	public bool isAbleToAttack = true;
	public Text alertUI;
	
    // Start is called before the first frame update
    void Start()
    {
        _stats = this.gameObject.GetComponent<PlayerStats>();
		
    }

    // Update is called once per frame
    void Update()
    {
		_playerAnimator.SetBool("IsMoving",false);
        Init();
		if(isAbleToAttack){
			
				MouseCommands();
			
			
		}
		if(!attacking)FindSelectableTiles();
    }
	IEnumerator EndTurn(float delay){
		yield return new  WaitForSeconds(delay);
		_turnManager.EndTurn();
	}
	IEnumerator alertUIdelay(){
		yield return new WaitForSeconds(2f);
		alertUI.gameObject.SetActive(false);
	}
	void MouseCommands(){
	if(_stats.ammoTokens > 0){
		if(Input.GetMouseButtonUp(0)){
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			
			RaycastHit hit;
			if(Physics.Raycast(ray, out hit)){
				if(hit.collider.tag == "Tile"){ // if you click the Tile you use Tile game object
					Tile t = hit.collider.GetComponent<Tile>();
					
					if(t.selectable){
						GameObject _gb = AttackTile(t);
						
						if(_gb.tag == "Enemy"){
							
							EnemyStats _eStats = hit.transform.gameObject.GetComponent<EnemyStats>();
							LookAtTarget(hit.transform);
							RaycastHit sideCheck;
									if(Physics.Raycast(transform.position,transform.forward, out sideCheck)){
										Debug.Log(sideCheck.transform.gameObject.tag);
									}
							_eStats.receiveDamage(_stats.getAttack());
							_playerAnimator.SetTrigger("IsFiring");
							isAbleToAttack = false;
							StartCoroutine(EndTurn(1f));
						}
						
						t.target= true;
					}
					
				}
				if(hit.collider.tag == "Enemy" ||hit.collider.tag == "sideTank" || hit.collider.tag == "backTank" || hit.collider.tag == "frontTank" ){ // if you click the Tile you use Enemy game object
				
					 //pick the Enemy object if you click on the parts
					GameObject enemy = null;
					int Attack = 1;
					if(hit.collider.tag == "Enemy"){
						enemy = hit.transform.gameObject;
					}
					else if(hit.collider.tag == "sideTank"){
						enemy =  hit.transform.parent.gameObject;
						Attack = 2;
					}
					else if(hit.collider.tag == "backTank"){
						enemy =  hit.transform.parent.gameObject;
						Attack = 3;
					}
					else if(hit.collider.tag == "frontTank"){
					   enemy =  hit.transform.parent.gameObject;
					}
				   
					RaycastHit hit_enemy;//the raycast is going from the enemy to down
					
					if(Physics.Raycast(hit.transform.position,-Vector3.up, out hit_enemy, 1)){
						
						if(hit_enemy.transform.gameObject.GetComponent<Tile>() != null){
							
							Tile t = hit_enemy.transform.gameObject.GetComponent<Tile>();
							
							if(t.selectable){
								//if(AttackTile(t).tag == "Enemy" ||AttackTile(t).tag == "sideTank" ||AttackTile(t).tag == "backTank"||AttackTile(t).tag == "frontTank"){
								//make raycast to see which side the attack is happening	
									
									
								EnemyStats _eStats = enemy.GetComponent<EnemyStats>();
								
								LookAtTarget(enemy.transform);
									RaycastHit sideCheck;
									
									if(Physics.Raycast(transform.position,transform.forward, out sideCheck)){
										Debug.Log(sideCheck.transform.gameObject.tag);
									}
									_eStats.receiveDamage(_stats.getAttack() * Attack);
									_stats.ammoTokens --;
									FMODUnity.RuntimeManager.PlayOneShot("event:/SOUNDFX/sfx_m4sherman_attack");
									FMODUnity.RuntimeManager.PlayOneShot("event:/SOUNDFX/sfx_panzerIV_damage");
								_playerAnimator.SetTrigger("IsFiring");
								isAbleToAttack = false;
								StartCoroutine(EndTurn(1f));
								//}
								t.target= true;
						    }
						}
					}
					
					
					
				}
				
			}
		}
		
		
	}
	
	else{
				alertUI.gameObject.SetActive(true);
				alertUI.text = "You are out of ammo";
				StartCoroutine(alertUIdelay());
			}
	}
}
