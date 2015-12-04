
/*
 * Author: Makiah Bennett
 * Created 27 September 2015
 * Last edited: 27 September 2015
 * 
 * 
 */


using UnityEngine;
using System.Collections;

public class TigerScript : EnemyBaseActionClass {

	//Initialization stuff.  
	protected override void SetReferences() {
		characterSpriteObject = transform.FindChild("FlippingItem").FindChild ("Tiger");
		base.SetReferences ();
	}
	
	protected override void Attack() {
		AttemptToAttackAfterCompletedAnimation ();
		Debug.Log ("Tiger is attacking");
	}

	//Based off of SwordScript
	private float enemyWithinAreaBounds = 1.5f;
	private float distToEnemyLength = 1f;
	
	public float tigerAttackPower;

	void AttemptToAttackAfterCompletedAnimation () {
		ActionsAfterAnimation += AttackEnemyInFocus;
	}
	
	void AttackEnemyInFocus () {

		Debug.Log ("Starting tiger attack");
		//Pretty much all of this is calculation for the eventual linecast.  
		Vector3 enemyWithinAreaVectorBound = new Vector3 (enemyWithinAreaBounds, 0, 0);
		Vector3 distToEnemyVectorLength = new Vector3 (distToEnemyLength, 0, 0);
		
		int tigerFacingDirection = GetFacingDirection ();
		
		Vector3 startRaycastParameter = transform.position - enemyWithinAreaVectorBound;
		Vector3 endRaycastParameter = transform.position + enemyWithinAreaVectorBound;
		
		Vector3 actualStartRaycastParameter = startRaycastParameter + distToEnemyVectorLength * tigerFacingDirection;
		Vector3 actualEndRaycastParameter = endRaycastParameter + distToEnemyVectorLength * tigerFacingDirection;
		
		RaycastHit2D[] linecastResult = Physics2D.LinecastAll (actualStartRaycastParameter, actualEndRaycastParameter, 1 << LayerMask.NameToLayer ("Player"));

		Debug.DrawLine (actualStartRaycastParameter, actualEndRaycastParameter, Color.red, 3f);
		
		GameObject result = GameObjectContainsCharacterHealthPanelManager (linecastResult);
		
		if (result != null) {
			result.GetComponent <CharacterHealthPanelManager> ().YouHaveBeenAttacked (tigerAttackPower);
			Debug.Log ("Hit " + result.gameObject.name);
		} else {
			Debug.Log("Did not hit an object.");
		}
	}
	
	//Look for the GameObject that has a health panel manager.  
	GameObject GameObjectContainsCharacterHealthPanelManager(RaycastHit2D[] linecastCollisions) {
		if (linecastCollisions.Length != 0) {
			for (int i = 0; i < linecastCollisions.Length; i++) {
				if (linecastCollisions [i].collider.gameObject.GetComponent <CharacterHealthPanelManager> () != null) {
					return linecastCollisions [i].collider.gameObject;
				}
			}
		}
		
		return null;
		
	}
}
