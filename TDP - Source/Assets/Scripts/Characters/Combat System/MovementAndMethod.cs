using UnityEngine;
using System.Collections;

public class MovementAndMethod {
	//Attacking method classes and enumerations.  
	[SerializeField]
	public enum PossibleTriggers
	{
		LeftMouseClick, 
		RightMouseClick
	}
	[SerializeField]
	public enum PossibleMovements 
	{
		OverheadSlice, 
		Stab, 
		ShootBow, 
		CreatePhysicalItem
	}

	//Specifies the required delegate.  
	private delegate bool MethodToAction ();
	private MethodToAction actionTrigger;

	//Attacking stuff.  
	private PossibleMovements attackEnumVal;
	private string attackKey;

	private bool canBeUsedWhileMidair = false;

	bool CheckLeftClick() {
		return Input.GetMouseButtonDown(0);
	}

	bool CheckRightClick() {
		return Input.GetMouseButtonDown (1);
	}

	//This constructor converts the enums into delegates.  
	public MovementAndMethod(PossibleMovements ctorMovement, PossibleTriggers ctorTrigger, bool canBeUsedInMidair) {
		//Define the trigger
		switch (ctorTrigger) {
		case PossibleTriggers.LeftMouseClick: 
			actionTrigger = new MethodToAction(CheckLeftClick);
			break;
		case PossibleTriggers.RightMouseClick: 
			actionTrigger = new MethodToAction (CheckRightClick);
			break;
		default: 
			Debug.LogError ("Invalid trigger");
			break;
		}

		//Define the movement.  
		attackEnumVal = ctorMovement;
		switch (ctorMovement) {
		case PossibleMovements.OverheadSlice: 
			attackKey = "OverheadSlice";
			break;
		case PossibleMovements.Stab: 
			attackKey = "Stab";
			break;
		default: 
			Debug.LogError ("Invalid action");
			break;
		}

		//Midair thing.  
		canBeUsedWhileMidair = canBeUsedInMidair;
	}

	//Getter methods
	public bool GetTriggerHasOccurred() {
		return actionTrigger ();
	}

	public string GetActionKey() {
		return attackKey;
	}

	public PossibleMovements GetActionEnum() {
		return attackEnumVal;
	}

	public bool GetCanBeUsedWhileMidair() {
		return canBeUsedWhileMidair;
	}
}
