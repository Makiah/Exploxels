/*
 * Author: Makiah Bennett
 * Date Created: 13 November 2015
 * 
 * Description: This script controls the variables that will be controlled during the remainder of the game.  
 * 
 */


using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameData : MonoBehaviour {

	//This will be the data that the game later receives from the UI.  
	[HideInInspector] public int chosenGender;
	[HideInInspector] public string specifiedPlayerName;
	[HideInInspector] public Profession chosenProfession;
	[HideInInspector] public int currentPlayerMoney;
	[HideInInspector] public ResourceReferenceWithStack[] currentPlayerItems;

	//Accessed by the DistrictDisplayer and set by GameControl.  
	[HideInInspector] public int currentLevel = 0;

}
