using UnityEngine;
using System.Collections;

public class BackgroundScroller : MonoBehaviour {

	void OnEnable() {
		EventManager.InitializeBackgroundScroller += InitializeBackgroundElements;
	}

	void OnDisable() {
		EventManager.InitializeBackgroundScroller -= InitializeBackgroundElements;
	}

	public float scrollSpeed;

	public Sprite[] segments;

	public GameObject backgroundPanel;


	void InitializeBackgroundElements() {
		VariableManagement variableManagement = GameObject.Find ("ManagementFrameworks").transform.FindChild ("GameVariables").GetComponent <VariableManagement> ();
		float terrainXLength = variableManagement.GetLevelLengthX ();
		float backgroundXLength = terrainXLength * (scrollSpeed * 10);
		int maxBackgroundSegments = (int) (backgroundXLength / segments[0].bounds.size.x + 1);
		for (int i = 0; i < maxBackgroundSegments; i++) {
			GameObject createdPanel = (GameObject) (Instantiate(backgroundPanel, Vector3.zero, Quaternion.identity));
			createdPanel.transform.SetParent(transform);
			createdPanel.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
			createdPanel.transform.localPosition = new Vector3(10.24f * createdPanel.transform.localScale.x * i, 0, 0);
			createdPanel.GetComponent <SpriteRenderer> ().sprite = ChooseRandomBackgroundTile();
		}
	}

	Sprite ChooseRandomBackgroundTile() {
		return segments[Random.Range(0, segments.Length)];
	}

	/// Called by player on each frame (FixedUpdate).  
	public void Movement(float h) {
		transform.localPosition += new Vector3 (-1 * h * scrollSpeed, 0, 0);
	}

}
