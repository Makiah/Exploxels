using UnityEngine;
using System.Collections;

public interface IModifyEvents {

	void OnEnable();
	void OnDisable();

}

public interface ISetAFrameworkReference {

	void SetManagementFrameworkReference();

}
