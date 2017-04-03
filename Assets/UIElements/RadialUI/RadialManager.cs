using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RadialManager : MonoBehaviour
{
	public GameObject DominoesInventory;
	public GameObject PropsInventory;
	public GameObject MainUI;
	public bool InventoryOpen;
	
	public GameObject ActiveInventory;
	public Transform GrandChild;
	
	public void DominoesButton()
	{
		DominoesInventory.SetActive(true);
		ActiveInventory = DominoesInventory;
		/* Controls the MainUI elements and turns them off on selection
		foreach(Transform child in MainUI.transform)
		{
			GrandChild = child.transform.GetChild(0);
			GrandChild.GetComponent<Button>().interactable = false;
		}
		*/
		InventoryOpen = true;
	}
	
	public void PropsButton()
	{
		PropsInventory.SetActive(true);
		ActiveInventory = PropsInventory;
		foreach(Transform child in MainUI.transform)
		{
			child.GetComponent<Button>().interactable = false;
		}
		InventoryOpen = true;
	}

	public void ExitButton()
	{
		foreach(Transform child in MainUI.transform)
		{
			//GrandChild = child.transform.GetChild(0);
			//GrandChild.GetComponent<Button>().interactable = true;
			InventoryOpen = false;
			ActiveInventory.SetActive(false);
			MainUI.SetActive(false);
		}
	}
	
	public void ExitGame()
	{
		Application.Quit();
	}
	
	void Update()
	{
		if (InventoryOpen == true)
		{
			if(Input.GetKeyUp(KeyCode.Escape))
			{
				foreach(Transform child in MainUI.transform)
				{
					child.GetComponent<Button>().interactable = true;
					InventoryOpen = false;
					ActiveInventory.SetActive(false);
				}
			}
		}
		
		if (Input.GetKeyDown(KeyCode.Space))
		{
			MainUI.transform.position = Input.mousePosition;
			MainUI.SetActive(true);
			InventoryOpen = true;
		}
	}	
}