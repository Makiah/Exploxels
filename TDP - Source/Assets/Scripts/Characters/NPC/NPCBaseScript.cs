using UnityEngine;
using System.Collections;

public abstract class NPCBaseScript : CharacterBaseActionClass {

	protected override void OnEnable() {
		LevelEventManager.InitializeNPCs += SetReferences;
	}

	protected override void OnDisable() {
		LevelEventManager.InitializeNPCs -= SetReferences;
	}

	protected Transform playerTransform;
	[SerializeField] protected float minDistanceRequiredForInteraction;

	protected override void SetReferences() {
		characterSpriteObject = transform.FindChild ("FlippingItem").FindChild ("Character");
		base.SetReferences ();
		playerTransform = VariableManagement.GetPlayerReference ().transform;
	}

}
