using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroMenuManager : MonoBehaviour 
{
	public GameObject CreditsScreen;
	public GameObject LogoLeft;
	
	public void NewGameButton()
	{
		Application.LoadLevel("ZenGarden");
	}
	
	public void ExitGame()
	{
		Application.Quit();
	}

	public void Credits()
	{
		CreditsScreen.SetActive(true);
		LogoLeft.SetActive(false);
	}
}
