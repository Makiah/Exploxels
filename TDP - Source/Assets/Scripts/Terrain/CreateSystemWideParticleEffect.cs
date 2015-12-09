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

	//Actual script
	void InitializeSystemWideParticleEffect() {
		if (CurrentLevelVariableManagement.GetMainGameData ().currentLevel == 0) {
			//Get level length
			float currentLevelLength = CurrentLevelVariableManagement.GetLevelLengthX();

			//Set snow particle system size.  
			transform.localScale = new Vector3 (1.2f * currentLevelLength, 1, 1);
			transform.position = new Vector3 (currentLevelLength / 2, 30, 0);

			//Quick bug fix.  
			gameObject.SetActive (false);
			gameObject.SetActive (true);

		}
	}

}
