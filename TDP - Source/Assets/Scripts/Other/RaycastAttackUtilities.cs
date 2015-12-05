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

		if (linecastResult.Length != 0) {
			for (int i = 0; i < linecastResult.Length; i++) {
				if (linecastResult [i].collider.gameObject.GetComponent <CharacterHealthPanelManager> () != null) {
					return linecastResult[i].collider.gameObject.GetComponent <CharacterHealthPanelManager> ();
				}
			}
		}

		return null;
	}

}
