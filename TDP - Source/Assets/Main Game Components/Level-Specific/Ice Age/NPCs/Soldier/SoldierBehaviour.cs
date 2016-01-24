using UnityEngine;
using System.Collections;

public class SoldierBehaviour : NPCBaseScript, ICanHoldItems {

	//Interacting with player stuff.  
	protected override void InitializeNPC() {
		npcName = "Soldier";
		//Sets the initial dialogue for the player.  
		string[] dialogue = new string[] {
			"We protect great Thuk!"
		};
		GetComponent <NPCPanelController> ().SetCharacterDialogue (dialogue);
	}

	public override void NPCActionBeforeSpeaking() {}

	public override void NPCActionAfterSpeaking() {}

	/************************************************ ITEM STUFF *************************************************************/
	//This dictionary contains the possible weapon moves for the player.  The first entry contains the required action to trigger the action, and the second
	//includes a string of the method.  
	private MovementAndMethod[] possibleWeaponMoves;

	private ItemBase itemInUseByCharacter;

	//This will be called by the item management part of the costume manager script
	public void OnRefreshCurrentWeaponMoves(ItemBase ctorItemInUseByCharacter) {
		itemInUseByCharacter = ctorItemInUseByCharacter;
		if (ctorItemInUseByCharacter != null) {
			possibleWeaponMoves = itemInUseByCharacter.GetPossibleActionsForItem ();
		} else {
			possibleWeaponMoves = null;
		}
	}

	//Has to be public for the interface
	public void AttackAction(MovementAndMethod someAttack) {
		if (!currentlyInAttackAnimation) {
			anim.SetTrigger (someAttack.GetActionKey());
			itemInUseByCharacter.InfluenceEnvironment (someAttack.GetActionEnum());
			if (someAttack.GetActionEnum() != MovementAndMethod.PossibleMovements.CreatePhysicalItem)
				currentlyInAttackAnimation = true;
		} else {
			Debug.Log("Was in attack animation, did not attack");
		}
	}

	//Used for weapons.  
	public void ExternalJumpAction (int num) {
		InitializeJump (num);
	}

}
