using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAction : MapAction
{
	private EnemyStats _stats;
		public Animator _EnemyAnimator;
    // Start is called before the first frame update
    void Start()
    {
        _stats = this.gameObject.GetComponent<EnemyStats>();
    }
	IEnumerator EndTurn(float delay){
		yield return new  WaitForSeconds(delay);
		_turnManager.EndTurn();
	}
    // Update is called once per frame
    void Update()
    {
        _EnemyAnimator.SetBool("IsMoving",false);
        Init();
		if(!attacking)FindSelectableTiles();
		EnemyAttack();
		
    }
	public void EnemyAttack(){
		
			if(!attacking){
				Debug.Log("Player DAMAGED");
				attacking = true;
				//pick available Tiles
				List<Tile> availableTiles = new List<Tile>();
				foreach(GameObject t in allTiles){
					if(t.GetComponent<Tile>() != null && t.GetComponent<Tile>().selectable == true){
						availableTiles.Add(t.GetComponent<Tile>());
					}
				}
				//See if the player is on any of them if yes attack player
				foreach(Tile t in availableTiles){
					RaycastHit hit;
					if(Physics.Raycast(t.transform.position,Vector3.up, out hit, 1)){
						
						if(hit.transform.gameObject.tag == "Player"){//if it is a enemy returns its gameobject
							LookAtTarget(hit.transform);
							PlayerStats _pStats = hit.transform.gameObject.GetComponent<PlayerStats>();
							_EnemyAnimator.SetTrigger("IsFiring");
							_pStats.receiveDamage(_stats.getAttack());
							FMODUnity.RuntimeManager.PlayOneShot("event:/SOUNDFX/sfx_panzerIV_attack");
							FMODUnity.RuntimeManager.PlayOneShot("event:/SOUNDFX/sfx_m4sherman_damage");
								
						}
					}
			}
			Debug.Log("EndTurn");
			StartCoroutine(EndTurn(1f));
		}
	}
}
