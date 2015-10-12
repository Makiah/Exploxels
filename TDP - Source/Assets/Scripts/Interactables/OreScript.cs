using UnityEngine;
using System.Collections;

public class OreScript : DropsItems {

	public int typeOfOre;

	public Sprite[] possibleImages;

	SpriteRenderer attachedSpriteRenderer;

	public int hitsUntilDrop = 2;
	private int currentHits;

	protected override void MakeReferences() {
		base.MakeReferences ();

		experienceToDrop = 1;

		attachedSpriteRenderer = GetComponent <SpriteRenderer> ();

		if (possibleImages.Length != 0) {
			attachedSpriteRenderer.sprite = possibleImages [Random.Range (0, possibleImages.Length)];
		} else {
			Debug.LogError("No attached image sprites on " + gameObject.name + " (OreScript)");
			Destroy(gameObject);
		}

		DropReferenceClass oreDrop = new DropReferenceClass (ResourceDatabase.GetItemByParameter (ResourceReference.ItemType.Ore, typeOfOre), 1, 1, 1);
		drops = new DropReferenceClass[]{oreDrop};

	}

	public void OnOreChipped() {
		currentHits++;
		if (currentHits >= hitsUntilDrop) {
			DropItems();
			Destroy(this.gameObject);
		}
	}

}
