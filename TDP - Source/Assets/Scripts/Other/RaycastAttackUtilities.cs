using UnityEngine;
using System.Collections;

public class RaycastAttackUtilities : MonoBehaviour {

	public static CharacterHealthPanelManager LookForEnemyViaLinecast (Vector2 origin, float distToEnemyOffset, float yOffsetToEnemy, float enemyWithinAreaBound, int characterFacingDirection, LayerMask layers) {

		//Pretty much all of this is calculation for the eventual linecast.  
		Vector2 enemyWithinAreaVectorBound = new Vector3 (enemyWithinAreaBound, 0);
		Vector2 distToEnemyVectorLength = new Vector3 (distToEnemyOffset, 0);
		Vector2 yOffsetVector = new Vector3 (0, yOffsetToEnemy);

		//This sets the size of the eventual linecast.  
		Vector2 startRaycastParameter = origin - enemyWithinAreaVectorBound;
		Vector2 endRaycastParameter = origin + enemyWithinAreaVectorBound;

		//This sets the actual position of the eventual linecast.  
		Vector2 actualStartRaycastParameter = startRaycastParameter + distToEnemyVectorLength * characterFacingDirection + yOffsetVector;
		Vector2 actualEndRaycastParameter = endRaycastParameter + distToEnemyVectorLength * characterFacingDirection + yOffsetVector;

		//Linecast, looking for items on the layer "Fighting"
		RaycastHit2D[] linecastResult = Physics2D.LinecastAll (actualStartRaycastParameter, actualEndRaycastParameter, layers);

		Debug.DrawLine (actualStartRaycastParameter, actualEndRaycastParameter, Color.green, 3f);
		
		//Go through all linecast results and look for the health panel.  
		if (linecastResult.Length != 0) {
			for (int i = 0; i < linecastResult.Length; i++) {
				//Check to make sure that the collider has a second parent.  
				//If the collider has a parent and not a second parent, short-circuiting prevents an error.  
				if (linecastResult[i].collider.transform.parent != null && linecastResult[i].collider.transform.parent.parent != null) {
					Transform healthPanelTransform = linecastResult[i].collider.transform.parent.parent;
					if (healthPanelTransform.GetComponent <CharacterHealthPanelManager> () != null) {
						//Return the health panel manager if it exists.  
						Debug.Log("Chose " + healthPanelTransform.gameObject.name);
						return healthPanelTransform.GetComponent <CharacterHealthPanelManager> ();
					}
				}
			}
		}
		
		//In the event that none of the results had a health panel manager.  
		return null;
	}

}
