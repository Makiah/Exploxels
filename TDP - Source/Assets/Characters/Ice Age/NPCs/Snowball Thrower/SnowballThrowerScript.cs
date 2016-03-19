using UnityEngine;
using System.Collections;

public class SnowballThrowerScript : NPCBaseScript {

	[SerializeField] private GameObject snowball = null;
	[SerializeField] private Sprite snowballHeld = null;
	private SpriteRenderer item;

	private Vector2 initialPosition = Vector2.zero;

	protected override void InitializeNPC() {
		//Snowball thrower initialization.  
		npcName = "Snowball Thrower";
		string[] dialogue = new string[] {
			"You seen any snowballs down there?", 
			"I be testing down force!", 
			"I in the elite weapons division."
		};
		GetComponent <NPCPanelController> ().SetCharacterDialogue (dialogue);

		initialPosition = transform.position;

		//Item fetch.  
		item = transform.FindChild ("FlippingItem").FindChild ("Character").FindChild ("Hands").FindChild ("HoldingHand").FindChild ("HoldingItem").GetComponent <SpriteRenderer> ();

		//Start dropping snowballs.  
		StartCoroutine (DropSnowballs ());
	}

	//The main coroutines that drops snowballs onto the player.  
	IEnumerator DropSnowballs() {
		while (true) {
			//Walk to the snowball cache.  
			yield return StartCoroutine(SetTargetPosition(initialPosition + new Vector2(1.8f, 0), .3f, 20, 1));
			//Place the item in the hand.  
			item.sprite = snowballHeld;
			yield return new WaitForSeconds (1f);

			//Make the snowball thrower walk to the end of the ledge.  
			yield return StartCoroutine (SetTargetPosition (initialPosition - new Vector2(2f, 0), .3f, 20, 1));
			rb2d.velocity = Vector2.zero;
			yield return new WaitForSeconds (1f);

			//Bend over and drop the snowball.  
			//anim.SetTrigger("Drop");
			GameObject droppedSnowball = (GameObject)(Instantiate (snowball, transform.position + new Vector3 (1f, 0, 0) * GetFacingDirection (), Quaternion.identity));
			item.sprite = null;
			rb2d.velocity = Vector2.zero;
			droppedSnowball.GetComponent <SnowballScript> ().Initialize (new Vector2 (3 * GetFacingDirection(), 0));
			rb2d.velocity = Vector2.zero;
			yield return new WaitForSeconds (3f);
		}
	}

	protected override IEnumerator WalkAround() {
		yield return null;
	}

	public override void NPCActionBeforeSpeaking() {
	}

	public override void NPCActionAfterSpeaking() {
	}

	public override void NPCActionOnPlayerWalkAway(){
	}
}
