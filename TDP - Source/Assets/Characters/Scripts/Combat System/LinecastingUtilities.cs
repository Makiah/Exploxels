using UnityEngine;
using System.Collections;

public class LinecastingUtilities : MonoBehaviour {
	//Looks for any component via linecast.  This can be easily used via generics.  
	public static CharacterHealthPanelManager FindEnemyViaLinecast (Vector2 origin, float distToEnemyOffset, float yOffsetToEnemy, float enemyWithinAreaBound, int characterFacingDirection, string attackingCharacterGUID) {
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
		RaycastHit2D[] linecastResult = Physics2D.LinecastAll (actualStartRaycastParameter, actualEndRaycastParameter, 1 << LayerMask.NameToLayer("Fighting"));

		//Create a line so that it is possible to see where the raycasts are going.  
		Debug.DrawLine (actualStartRaycastParameter, actualEndRaycastParameter, Color.green, 3f);

		//Go through all linecast results and look for the health panel.  
		if (linecastResult.Length != 0) {
			for (int i = 0; i < linecastResult.Length; i++) {
				//Check to make sure that the collider has a second parent.  
				//If the collider has a parent and not a second parent, short-circuiting prevents an error.  
				if (linecastResult[i].collider.transform.parent != null && linecastResult[i].collider.transform.parent.parent != null) {
					Transform healthPanelTransform = linecastResult[i].collider.transform.parent.parent;
					//Make sure that the character has a health panel manager and that the character is not attacking itself.  
					if (healthPanelTransform.GetComponent <CharacterHealthPanelManager> () != null && healthPanelTransform.GetComponent <ICombatant> ().GetCombatantID().Equals(attackingCharacterGUID) == false) {
						//Return the health panel manager if it exists.  
						return healthPanelTransform.GetComponent <CharacterHealthPanelManager> ();
					}
				}
			}
		}

		//In the event that none of the results had a health panel manager.  
		return null;
	}

	//Looks for any component via linecast.  This can be easily used via generics.  
	public static T FindComponentViaLinecast <T> (Vector2 origin, float distToEnemyOffset, float yOffsetToEnemy, float enemyWithinAreaBound, int characterFacingDirection) {
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
		RaycastHit2D[] linecastResult = Physics2D.LinecastAll (actualStartRaycastParameter, actualEndRaycastParameter, 1 << LayerMask.NameToLayer("Fighting"));

		//Create a line so that it is possible to see where the raycasts are going.  
		Debug.DrawLine (actualStartRaycastParameter, actualEndRaycastParameter, Color.green, 3f);

		//Go through all linecast results and look for the health panel.  
		if (linecastResult.Length != 0) {
			for (int i = 0; i < linecastResult.Length; i++) {
				//Check to make sure that the collider has a second parent.  
				//If the collider has a parent and not a second parent, short-circuiting prevents an error.  
				if (linecastResult[i].collider.transform.parent != null && linecastResult[i].collider.transform.parent.parent != null) {
					Transform healthPanelTransform = linecastResult[i].collider.transform.parent.parent;
					//Make sure that the character has a health panel manager and that the character is not attacking itself.  
					if (healthPanelTransform.GetComponent <T> () != null) {
						//Return the health panel manager if it exists.  
						return healthPanelTransform.GetComponent <T> ();
					}
				}
			}
		}

		//In the event that none of the results had a health panel manager.  
		return default (T);
	}

	public static T BasicLinecast <T> (Vector2 start, Vector2 end) {

		Debug.Log ("Ground Pound w/ " + start.ToString() + " and " + end.ToString());

		//Linecast, looking for items on the layer "Fighting"
		RaycastHit2D[] linecastResult = Physics2D.LinecastAll (start, end, 1 << LayerMask.NameToLayer("Fighting"));

		//Create a line so that it is possible to see where the raycasts are going.  
		Debug.DrawLine (start, end, Color.green, 3f);

		//Go through all linecast results and look for the health panel.  
		if (linecastResult.Length != 0) {
			for (int i = 0; i < linecastResult.Length; i++) {
				//Check to make sure that the collider has a second parent.  
				//If the collider has a parent and not a second parent, short-circuiting prevents an error.  
				if (linecastResult[i].collider.transform.parent != null && linecastResult[i].collider.transform.parent.parent != null) {
					Transform healthPanelTransform = linecastResult[i].collider.transform.parent.parent;
					//Make sure that the character has a health panel manager and that the character is not attacking itself.  
					if (healthPanelTransform.GetComponent <T> () != null) {
						//Return the health panel manager if it exists.  
						return healthPanelTransform.GetComponent <T> ();
					}
				}
			}
		}

		//In the event that none of the results had a health panel manager.  
		return default (T);
	}

	public static CharacterHealthPanelManager BasicLinecast (Vector2 start, Vector2 end, string attackingCharacterGUID) {
		//Linecast, looking for items on the layer "Fighting"
		RaycastHit2D[] linecastResult = Physics2D.LinecastAll (start, end, 1 << LayerMask.NameToLayer("Fighting"));

		//Create a line so that it is possible to see where the raycasts are going.  
		Debug.DrawLine (start, end, Color.green, 3f);

		//Go through all linecast results and look for the health panel.  
		if (linecastResult.Length != 0) {
			for (int i = 0; i < linecastResult.Length; i++) {
				//Check to make sure that the collider has a second parent.  
				//If the collider has a parent and not a second parent, short-circuiting prevents an error.  
				if (linecastResult[i].collider.transform.parent != null && linecastResult[i].collider.transform.parent.parent != null) {
					Transform healthPanelTransform = linecastResult[i].collider.transform.parent.parent;
					//Make sure that the character has a health panel manager and that the character is not attacking itself.  
					if (healthPanelTransform.GetComponent <CharacterHealthPanelManager> () != null && healthPanelTransform.GetComponent <CharacterBaseActionClass> ().GetCombatantID().Equals(attackingCharacterGUID) == false) {
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
