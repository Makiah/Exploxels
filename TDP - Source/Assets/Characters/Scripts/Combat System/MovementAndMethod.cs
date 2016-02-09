using UnityEngine;
using System.Collections;

public class MovementAndMethod {
	//Attacking method classes and enumerations.  
	[SerializeField]
	public enum PossibleTriggers
	{
		LeftMouseClick, 
		RightMouseClick, 
		ShiftPlusLeftClick, 
		ShiftPlusRightClick
	}
	[SerializeField]
	public enum PossibleMovements 
	{
		OverheadSlice, 
		Stab, 
		ShootBow, 
		CreatePhysicalItem, 
		GroundPound, 
		SpinningSlash, 
		AirSlash
	}

	//Specifies the required delegate.  
	private delegate bool MethodToAction ();
	private MethodToAction actionTrigger;

	//Attacking stuff.  
	private PossibleMovements attackEnumVal;
	private string attackKey;

	private bool canBeUsedWhileMidair = false;

	bool CheckLeftClick() {
		return Input.GetKey(KeyCode.LeftShift) == false && Input.GetMouseButtonDown(0);
	}

	bool CheckRightClick() {
		return Input.GetKey(KeyCode.LeftShift) == false && Input.GetMouseButtonDown (1);
	}

	bool CheckShiftPlusLeftClick() {
		return Input.GetMouseButtonDown (0) && Input.GetKey (KeyCode.LeftShift);
	}

	bool CheckShiftPlusRightClick() {
		return Input.GetMouseButtonDown (1) && Input.GetKey (KeyCode.LeftShift);
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
		case PossibleTriggers.ShiftPlusLeftClick: 
			actionTrigger = new MethodToAction (CheckShiftPlusLeftClick);
			break;
		case PossibleTriggers.ShiftPlusRightClick: 
			actionTrigger = new MethodToAction (CheckShiftPlusRightClick);
			break;
		default: 
			Debug.LogError ("Invalid trigger");
			break;
		}

		//Define the movement.  (Has to be used for animations)
		attackEnumVal = ctorMovement;
		//Used for extraneous cases.  
		attackKey = attackEnumVal.ToString();

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
