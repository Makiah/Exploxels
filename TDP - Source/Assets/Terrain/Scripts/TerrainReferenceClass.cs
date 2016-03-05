using UnityEngine;
using System.Collections;

public class TerrainReferenceClass {

	public Transform[] layer1;
	public Transform[] layer2;
	public Transform[] layer3;

	public TerrainReferenceClass(int levelLength) {
		layer1 = new Transform[levelLength];
		layer2 = new Transform[levelLength];
		layer3 = new Transform[levelLength];
	}

}
