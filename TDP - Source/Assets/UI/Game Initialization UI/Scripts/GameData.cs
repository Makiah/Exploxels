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
	public enum Gender {
		MALE, 
		FEMALE
	}
	private static Gender gender;
	public static void SetChosenGender(Gender gender1) {
		gender = gender1;
	}

	public static Gender GetChosenGender() {
		return gender;
	}

	private static string playerName;
	public static void SetPlayerName(string playerName1) {
		playerName = playerName1;
	}

	public static string GetPlayerName() {
		return playerName;
	}

	private static Profession playerProfession;
	public static void SetPlayerProfession(Profession playerProfession1) {
		playerProfession = playerProfession1;
	}

	public static Profession GetPlayerProfession() {
		return playerProfession;
	}

	private static int money;
	public static void SetPlayerMoney(int money1) {
		money = money1;
	}

	public static int GetPlayerMoney() {
		return money;
	}

	private static ResourceReferenceWithStack[] items;
	public static void SetPlayerItems(ResourceReferenceWithStack[] items1) {
		items = items1;
	}

	public static ResourceReferenceWithStack[] GetPlayerItems() {
		return items;
	}

	//Accessed by the DistrictDisplayer and set by GameControl.  
	private static int level = 0;
	public static void SetLevel(int level1) {
		level = level1;
	}

	public static int GetLevel() {
		return level;
	}

}
