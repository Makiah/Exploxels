
/*
 * Author: Makiah Bennett
 * Last edited: 13 September 2015
 * 
 * This script is a subclass of SlotScript, so the only special components that this script contains is the ability to be selected by the
 * HotbarManager script, which references each instance by name.  
 * 
 * 
 */

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HotbarSlotScript : SlotScript {

	GameObject selectionIcon;

	public HotbarManager masterHotbarManager;

	public override void ReferenceChildren() {
		base.ReferenceChildren ();
		selectionIcon = transform.FindChild ("Hotbar Indicator").gameObject;
	}

	public void SetSelectionActive() {
		selectionIcon.SetActive (true);
	}

	public void DisableSelection() {
		selectionIcon.SetActive (false);
	}

	public override void MouseItemMovementAndStackHandler() {
		base.MouseItemMovementAndStackHandler ();
		masterHotbarManager.UpdateSelectedItem ();
	}

}