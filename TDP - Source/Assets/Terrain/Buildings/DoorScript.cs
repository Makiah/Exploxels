using UnityEngine;
using System.Collections;
using System;

public class DoorScript : MonoBehaviour {

	void OnEnable() {
		LevelEventManager.InitializeDoors += InitializeDoor;
	}

	void OnDisable() {
		LevelEventManager.InitializeDoors -= InitializeDoor;
	}

	[SerializeField] public int doorID;

	[HideInInspector] public string doorGUID = Guid.NewGuid().ToString();

	private Transform player;

	private DoorScript otherDoor;

	void InitializeDoor() {
		//Get the player.  
		player = CurrentLevelVariableManagement.GetPlayerReference ().transform;

		//Find the appropriate door.  
		GameObject[] doors = GameObject.FindGameObjectsWithTag ("Door");
		if (doors.Length > 0) {
			//For each of the doors.  
			for (int i = 0; i < doors.Length; i++) {
				DoorScript currentDoorScript = doors [i].GetComponent <DoorScript> ();
				if (currentDoorScript != null) {
					//Make sure that the other door is different, but has the same ID.  
					if (currentDoorScript.doorID == doorID && currentDoorScript.doorGUID.Equals(doorGUID) == false) {
						otherDoor = doors [i].GetComponent <DoorScript> ();
						Debug.Log ("Got other door");
						break;
					}
				} else {
					Debug.Log ("Door " + i + " does not have a DoorScript");
				}
			}
		} else {
			Debug.Log ("Could not find any doors with tag 'Door'");
		}

		//Start looking to make sure that the player is near the door.  
		StartCoroutine (WaitForPlayerEntry ());
	}

	//Wait for Player Entry.  
	IEnumerator WaitForPlayerEntry() {
		while (true) {
			//Get one variable for the player.  
			float distance = Vector2.Distance (player.position, transform.position);

			//Wait based on the distance the player is from the door (no point in updating every frame).  
			if (distance > 10f)
				yield return new WaitForSeconds (3);
			else if (distance > 6f)
				yield return new WaitForSeconds (1);
			else
				yield return null;

			if (distance < 2f) {
				if (Input.GetKeyDown (KeyCode.W)) {
					player.transform.position = otherDoor.transform.position;
					Debug.Log ("Player teleported");
				}
			}
		}
	}

}
