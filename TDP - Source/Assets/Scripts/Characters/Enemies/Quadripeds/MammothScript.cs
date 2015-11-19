
/*
 * Author: Makiah Bennett
 * Created 27 September 2015
 * Last edited: 27 September 2015
 * 
 * 
 */


using UnityEngine;
using System.Collections;

public class MammothScript : EnemyBaseActionClass {

	protected override void SetReferences() {
		characterSpriteObject = transform.FindChild("FlippingItem").FindChild ("Mammoth");
		base.SetReferences ();
	}

	protected override void Attack() {

	}


}
