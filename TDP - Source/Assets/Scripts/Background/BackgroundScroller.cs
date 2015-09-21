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

	float pos;

	Transform mainCamera;
	Transform backgroundImage;

	Vector3 previousCameraPosition;
	float backgroundImageAnchorPosX = 0f;

	void InitializeBackgroundElements() {
		mainCamera = transform.parent;
		backgroundImage = transform.GetChild (0);

		SpriteRenderer[] backgroundImages = new SpriteRenderer[transform.childCount];
		for (int i = 0; i < transform.childCount; i++) {
			backgroundImages[i] = transform.GetChild(i).GetComponent <SpriteRenderer> ();
		}

		StartCoroutine ("ScrollBackground");

	}

	IEnumerator ScrollBackground() {
		while (true) {
			pos += scrollSpeed;
			if (pos > 1.0f) 
				pos -=1.0f;

			yield return null;
		}
	}

}
