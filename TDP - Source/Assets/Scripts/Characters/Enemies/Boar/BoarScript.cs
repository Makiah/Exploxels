
/*
 * Author: Makiah Bennett
 * Created 27 September 2015
 * Last edited: 27 September 2015
 * 
 * 
 */


using UnityEngine;
using System.Collections;

public class BoarScript : EnemyBaseActionClass {

	protected override void SetReferences() {
		Debug.Log ("Boar completed");
		characterSpriteObject = transform.FindChild("FlippingItem").FindChild ("Boar");
		base.SetReferences ();
	}

	protected override void Attack() {

	}


}
