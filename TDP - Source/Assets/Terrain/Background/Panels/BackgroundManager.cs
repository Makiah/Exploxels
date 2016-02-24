using UnityEngine;
using System.Collections;

public class BackgroundManager : MonoBehaviour {
	//Initialization
	void OnEnable() {
		LevelEventManager.InitializeBackgroundManager += CreateBackground;
	}

	void OnDisable() {
		LevelEventManager.InitializeBackgroundManager -= CreateBackground;
	}

	//Script
	[System.Serializable] private class SpriteScrollSpeedPair {public Sprite[] backgrounds = null; public float scrollSpeed = 0;}
	[SerializeField] private SpriteScrollSpeedPair layer1 = null, layer2 = null, clouds = null, background = null;

	[SerializeField] private GameObject backgroundPanel = null;

	private Transform layer1Parent, layer2Parent, cloudLayerParent, backgroundParent;

	void CreateBackground() {
		float terrainXLength = CurrentLevelVariableManagement.GetLevelLengthX ();
		//Layer 1
		layer1Parent = transform.FindChild("Hill Layer 1");
		int layer1SegmentsToCreate = (int) (terrainXLength * layer1.scrollSpeed) + 4;
		//Instantiate the segments.  
		float currentXValue = -layer1.backgrounds[0].bounds.size.x * 2;
		for (int i = 0; i < layer1SegmentsToCreate; i++) {
			//Instantiate the panel and set the sprite sorting order.  
			GameObject createdPanel = (GameObject) (Instantiate(backgroundPanel, Vector3.zero, Quaternion.identity));
			createdPanel.transform.parent = layer1Parent;
			createdPanel.GetComponent <SpriteRenderer> ().sortingOrder = 0;
			//Choose a sprite and add it to the SpriteRenderer component.  
			Sprite chosenSprite = layer1.backgrounds [Random.Range (0, layer1.backgrounds.Length)];
			createdPanel.GetComponent <SpriteRenderer> ().sprite = chosenSprite;
			//Position the sprite accordingly.  
			if (i > 0) currentXValue += chosenSprite.bounds.size.x / 2f; //Only do this on future loops, not the first one.  
			createdPanel.transform.localPosition = new Vector3 (currentXValue, 0, 0);
			currentXValue += chosenSprite.bounds.size.x / 2f;
		}

		//Layer 2
		layer2Parent = transform.FindChild("Hill Layer 2");
		int layer2SegmentsToCreate = (int) (terrainXLength * layer2.scrollSpeed) + 4;
		//Instantiate the segments.  
		currentXValue = -layer2.backgrounds[0].bounds.size.x * 2;
		for (int i = 0; i < layer2SegmentsToCreate; i++) {
			//Instantiate the panel and set the sprite sorting order.  
			GameObject createdPanel = (GameObject) (Instantiate(backgroundPanel, Vector3.zero, Quaternion.identity));
			createdPanel.transform.parent = layer2Parent;
			createdPanel.GetComponent <SpriteRenderer> ().sortingOrder = -1;
			//Choose a sprite and add it to the SpriteRenderer component.  
			Sprite chosenSprite = layer2.backgrounds [Random.Range (0, layer2.backgrounds.Length)];
			createdPanel.GetComponent <SpriteRenderer> ().sprite = chosenSprite;
			//Position the sprite accordingly.  
			if (i > 0) currentXValue += chosenSprite.bounds.size.x / 2f; //Only do this on future loops, not the first one.  
			createdPanel.transform.localPosition = new Vector3 (currentXValue, 0, 0);
			currentXValue += chosenSprite.bounds.size.x / 2f;
		}

		//Clouds
		cloudLayerParent = transform.FindChild("Clouds");
		int cloudLayerSegmentsToCreate = (int) (terrainXLength * clouds.scrollSpeed) + 4;
		//Instantiate the segments.  
		currentXValue = -clouds.backgrounds[0].bounds.size.x * 2;
		for (int i = 0; i < cloudLayerSegmentsToCreate; i++) {
			//Instantiate the panel and set the sprite sorting order.  
			GameObject createdPanel = (GameObject) (Instantiate(backgroundPanel, Vector3.zero, Quaternion.identity));
			createdPanel.transform.parent = cloudLayerParent;
			createdPanel.GetComponent <SpriteRenderer> ().sortingOrder = -2;
			//Choose a sprite and add it to the SpriteRenderer component.  
			Sprite chosenSprite = clouds.backgrounds [Random.Range (0, clouds.backgrounds.Length)];
			createdPanel.GetComponent <SpriteRenderer> ().sprite = chosenSprite;
			//Position the sprite accordingly.  
			if (i > 0) currentXValue += chosenSprite.bounds.size.x / 2f; //Only do this on future loops, not the first one.  
			createdPanel.transform.localPosition = new Vector3 (currentXValue, 0, 0);
			currentXValue += chosenSprite.bounds.size.x / 2f;
		}

		//Background Sky
		backgroundParent = transform.FindChild("Background");
		int backgroundSegmentsToCreate = (int) (terrainXLength * background.scrollSpeed) + 8;
		//Instantiate the segments.  
		currentXValue = -background.backgrounds[0].bounds.size.x * 2;
		for (int i = 0; i < backgroundSegmentsToCreate; i++) {
			//Instantiate the panel and set the sprite sorting order.  
			GameObject createdPanel = (GameObject) (Instantiate(backgroundPanel, Vector3.zero, Quaternion.identity));
			createdPanel.transform.parent = backgroundParent;
			//Has to go at -4 instead of -3 because the sun has to go behind this layer.  
			createdPanel.GetComponent <SpriteRenderer> ().sortingOrder = -4;
			//Choose a sprite and add it to the SpriteRenderer component.  
			Sprite chosenSprite = background.backgrounds [Random.Range (0, background.backgrounds.Length)];
			createdPanel.GetComponent <SpriteRenderer> ().sprite = chosenSprite;
			//Position the sprite accordingly.  
			if (i > 0) currentXValue += chosenSprite.bounds.size.x / 2f; //Only do this on future loops, not the first one.  
			createdPanel.transform.localPosition = new Vector3 (currentXValue, 0, 0);
			currentXValue += chosenSprite.bounds.size.x / 2f;
		}
	}

	//Used to move the background.  
	public void MoveBackground(float speed) {
		layer1Parent.transform.localPosition += new Vector3 (-1 * speed * layer1.scrollSpeed, 0, 0);
		layer2Parent.transform.localPosition += new Vector3 (-1 * speed * layer2.scrollSpeed, 0, 0);
		cloudLayerParent.transform.localPosition += new Vector3 (-1 * speed * clouds.scrollSpeed, 0, 0);
		backgroundParent.transform.localPosition += new Vector3 (-1 * speed * background.scrollSpeed, 0, 0);
	}

}
