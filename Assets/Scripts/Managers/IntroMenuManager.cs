using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroMenuManager : MonoBehaviour 
{
	public GameObject CreditsScreen;
	public GameObject LogoLeft;
	
	public void NewGameButton()
	{
        SceneManager.LoadScene("ZenGarden");
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
