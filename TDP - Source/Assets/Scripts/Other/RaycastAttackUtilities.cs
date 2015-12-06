using UnityEngine;
using System.Collections;

public class RaycastAttackUtilities : MonoBehaviour {

	public static CharacterHealthPanelManager LookForEnemyViaLinecast (Vector3 origin, float distToEnemyOffset, float enemyWithinAreaBound, int characterFacingDirection, bool isPlayer) {
		//Pretty much all of this is calculation for the eventual linecast.  
		Vector3 enemyWithinAreaVectorBound = new Vector3 (enemyWithinAreaBound, 0, 0);
		Vector3 distToEnemyVectorLength = new Vector3 (distToEnemyOffset, 0, 0);
		
		int playerFacingDirection = characterFacingDirection;
		
		Vector3 startRaycastParameter = origin - enemyWithinAreaVectorBound;
		Vector3 endRaycastParameter = origin + enemyWithinAreaVectorBound;
		
		Vector3 actualStartRaycastParameter = startRaycastParameter + distToEnemyVectorLength * playerFacingDirection;
		Vector3 actualEndRaycastParameter = endRaycastParameter + distToEnemyVectorLength * playerFacingDirection;

		//If the current attached input is the player, attack enemies.  Otherwise, attack the player.  
		string lookForItemsOnLayer = isPlayer ? "Enemies" : "Player";

		RaycastHit2D[] linecastResult = Physics2D.LinecastAll (actualStartRaycastParameter, actualEndRaycastParameter, 1 << LayerMask.NameToLayer (lookForItemsOnLayer));
		
		if (lookForItemsOnLayer == "Player") {
			Debug.DrawLine (actualStartRaycastParameter, actualEndRaycastParameter, Color.red, 3f);
		} else {
			Debug.DrawLine (actualStartRaycastParameter, actualEndRaycastParameter, Color.green, 3f);
		}

		//Go through all linecast results and look for the health panel.  
		if (linecastResult.Length != 0) {
			for (int i = 0; i < linecastResult.Length; i++) {
				//Check to make sure that the collider has a second parent.  
				//If the collider has a parent and not a second parent, short-circuiting prevents an error.  
				if (linecastResult[i].collider.transform.parent != null && linecastResult[i].collider.transform.parent.parent != null) {
					Transform healthPanelTransform = linecastResult[i].collider.transform.parent.parent;
					if (healthPanelTransform.GetComponent <CharacterHealthPanelManager> () != null) {
						//Return the health panel manager if it exists.  
						return healthPanelTransform.GetComponent <CharacterHealthPanelManager> ();
					}
				}
			}
		}

		//In the event that none of the results had a health panel manager.  
		return null;
	}

	public static CharacterHealthPanelManager LookForEnemyViaLinecast (Vector2 origin, float distToEnemyOffset, float yOffsetToEnemy, float enemyWithinAreaBound, int characterFacingDirection, bool isPlayer) {
		//Pretty much all of this is calculation for the eventual linecast.  
		Vector2 enemyWithinAreaVectorBound = new Vector3 (enemyWithinAreaBound, 0);
		Vector2 distToEnemyVectorLength = new Vector3 (distToEnemyOffset, 0);
		Vector2 yOffsetVector = new Vector3 (0, yOffsetToEnemy);

		Debug.Log ("Dist to enemy vector length is " + distToEnemyVectorLength);

		int playerFacingDirection = characterFacingDirection;

		//This sets the size of the eventual linecast.  
		Vector2 startRaycastParameter = origin - enemyWithinAreaVectorBound;
		Vector2 endRaycastParameter = origin + enemyWithinAreaVectorBound;

		//This sets the actual position of the eventual linecast.  
		Vector2 actualStartRaycastParameter = startRaycastParameter + distToEnemyVectorLength * playerFacingDirection + yOffsetVector;
		Vector2 actualEndRaycastParameter = endRaycastParameter + distToEnemyVectorLength * playerFacingDirection + yOffsetVector;
		
		//If the current attached input is the player, attack enemies.  Otherwise, attack the player.  
		string lookForItemsOnLayer = isPlayer ? "Enemies" : "Player";
		
		RaycastHit2D[] linecastResult = Physics2D.LinecastAll (actualStartRaycastParameter, actualEndRaycastParameter, 1 << LayerMask.NameToLayer (lookForItemsOnLayer));
		
		if (lookForItemsOnLayer == "Player") {
			Debug.DrawLine (actualStartRaycastParameter, actualEndRaycastParameter, Color.red, 3f);
		} else {
			Debug.DrawLine (actualStartRaycastParameter, actualEndRaycastParameter, Color.green, 3f);
		}
		
		//Go through all linecast results and look for the health panel.  
		if (linecastResult.Length != 0) {
			for (int i = 0; i < linecastResult.Length; i++) {
				//Check to make sure that the collider has a second parent.  
				//If the collider has a parent and not a second parent, short-circuiting prevents an error.  
				if (linecastResult[i].collider.transform.parent != null && linecastResult[i].collider.transform.parent.parent != null) {
					Transform healthPanelTransform = linecastResult[i].collider.transform.parent.parent;
					if (healthPanelTransform.GetComponent <CharacterHealthPanelManager> () != null) {
						//Return the health panel manager if it exists.  
						return healthPanelTransform.GetComponent <CharacterHealthPanelManager> ();
					}
				}
			}
		}
		
		//In the event that none of the results had a health panel manager.  
		return null;
	}

}
