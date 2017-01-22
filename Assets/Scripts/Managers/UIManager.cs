using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
	public GameObject DominoesInventory;
	public GameObject PropsInventory;
	public GameObject MainUI;
	public bool InventoryOpen;
	
	public GameObject ActiveInventory;
	
	public void DominoesButton()
	{
		DominoesInventory.SetActive(true);
		ActiveInventory = DominoesInventory;
		foreach(Transform child in MainUI.transform)
		{
			child.GetComponent<Button>().interactable = false;
		}
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
			child.GetComponent<Button>().interactable = true;
			InventoryOpen = false;
			ActiveInventory.SetActive(false);
		}
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
	}	
}