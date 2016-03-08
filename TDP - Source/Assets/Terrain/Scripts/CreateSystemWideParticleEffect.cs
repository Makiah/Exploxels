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

	//This variable holds the number of segments (sub-particle effects) that will be used for any given interval [x, 100+x]
	[SerializeField] private int numberOfSegmentsToCreatePer100 = 0;
	[SerializeField] private GameObject snowParticleEffect = null;

	//Actual script
	void InitializeSystemWideParticleEffect() {
		//Check to make sure that this is the Ice Age.  
		if (GameData.GetLevel() == 0) {
			//Create the particle effect thing.  
			GameObject createdParticleEffectParent = new GameObject("Level Particle Effect");
			createdParticleEffectParent.transform.SetParent (GameObject.Find ("Maze").transform);

			//Get level length
			float levelLength = CurrentLevelVariableManagement.GetLevelLengthX();
			float amountOfAreaToCover = 1.2f * levelLength;

			//Set the position of the particle effect parent.  
			createdParticleEffectParent.transform.localPosition = new Vector3 (levelLength / 2, 45, 0);
		
			//The scale of the particle effect.  
			float particleEffectScale = 100f / numberOfSegmentsToCreatePer100;

			//For all of the sub-100 segments.  (i.e., 700 width, 7 times).  
			for (int i = 0; i < (int)(amountOfAreaToCover / 100f); i++) {
				//For all of the particle effects that should be created for each interval of 100.  
				for (int j = 0; j < numberOfSegmentsToCreatePer100; j++) {
					//Create a particle effect at some point.  
					//Formula is half the level length (counteract the local position of the parent) + i * numberOfSegmentsToCreatePer100
					float xPosition = (-amountOfAreaToCover / 2) + (i * numberOfSegmentsToCreatePer100 + j) * (particleEffectScale);

					//Create the particle effect
					GameObject createdParticleEffectSegment = (GameObject)(Instantiate (snowParticleEffect, Vector3.zero, snowParticleEffect.transform.localRotation));
					createdParticleEffectSegment.transform.SetParent (createdParticleEffectParent.transform);
					createdParticleEffectSegment.transform.localPosition = new Vector3 (xPosition, 0, 0);
					createdParticleEffectSegment.transform.localScale = new Vector3 (particleEffectScale, 1, 1);

					//Set the emission rate of the particle effect.  
					ParticleSystem.EmissionModule em = createdParticleEffectSegment.GetComponent <ParticleSystem> ().emission;
					em.rate = new ParticleSystem.MinMaxCurve(particleEffectScale);
					//Initialize the particle effect.  
					createdParticleEffectSegment.GetComponent <ActivateParticleEffectDependingOnPlayerDistance> ().StartPlayerDistanceChecking ();
				}
			}

		}
	}

}
