using UnityEngine;
using System.Collections;

public class CreateSystemWideParticleEffect : MonoBehaviour {

	//Initialization
	void OnEnable() {
		LevelEventManager.InitializeSystemWideParticleEffect += InitializeSystemWideParticleEffect;
	}

	void OnDisable() {
		LevelEventManager.InitializeSystemWideParticleEffect -= InitializeSystemWideParticleEffect;
	}

	[SerializeField] private GameObject snowParticleEffect = null;

	//Actual script
	void InitializeSystemWideParticleEffect() {
		if (CurrentLevelVariableManagement.GetMainGameData ().currentLevel == 0) {
			//Get level length
			float levelLength = CurrentLevelVariableManagement.GetLevelLengthX ();
			Debug.Log ("Current level length x = " + levelLength);

			//Instantiate the particle effect into the maze.  
			GameObject createdParticleEffect = (GameObject) (Instantiate(snowParticleEffect, Vector3.zero, Quaternion.identity));
			createdParticleEffect.transform.SetParent(GameObject.Find("Maze").transform);
			//A local x of 0 means in the center of the maze.  
			createdParticleEffect.transform.localPosition = new Vector3(levelLength/2, 60, 0);
			//Set particle system size.  The particle system size is changed as the scale changes.  Add 50 so it covers the start segments as well.   
			createdParticleEffect.transform.localScale = new Vector3 (levelLength + 70, 1, 1);

			//Other setup stuff.  
			createdParticleEffect.transform.eulerAngles = new Vector3(0, 0, 180);
			//Bugfix
			createdParticleEffect.SetActive(false);
			createdParticleEffect.SetActive(true);

		}
	}

}
