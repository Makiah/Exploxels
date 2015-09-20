using UnityEngine;
using System.Collections;

public class BackgroundScroller : MonoBehaviour {

	void OnEnable() {
		EventManager.InitializeBackgroundScroller += InitializeBackgroundElements;
	}

	void OnDisable() {
		EventManager.InitializeBackgroundScroller -= InitializeBackgroundElements;
	}

	public float scrollRatio;

	Transform mainCamera;
	Transform backgroundImage;

	Vector3 previousCameraPosition;
	float backgroundImageAnchorPosX = 0f;

	void InitializeBackgroundElements() {
		mainCamera = transform.parent;
		backgroundImage = transform.GetChild (0);
		StartCoroutine ("ScrollBackground");
	}

	IEnumerator ScrollBackground() {
		while (true) {
			Vector3 deltaCameraPosition = mainCamera.position - previousCameraPosition;
			Vector3 backgroundTargetPosition = deltaCameraPosition / scrollRatio;
			backgroundImage.transform.position = backgroundImage.transform.position - backgroundTargetPosition;
			Debug.Log(backgroundImage.transform.position);
			previousCameraPosition = mainCamera.position;
			if ((backgroundImage.transform.position.x - backgroundImageAnchorPosX) > 10.24f) {
				backgroundImageAnchorPosX += 20.4f;
				backgroundImage.transform.position = new Vector3(backgroundImageAnchorPosX + 10.24f, backgroundImage.transform.position.y, backgroundImage.transform.position.z);
			} else if ((backgroundImage.transform.position.x - backgroundImageAnchorPosX) < -10.24f) {
				backgroundImageAnchorPosX -= 20.4f;
				backgroundImage.transform.position = new Vector3(backgroundImageAnchorPosX - 10.24f, backgroundImage.transform.position.y, backgroundImage.transform.position.z);
			}

			yield return null;
		}
	}

}
