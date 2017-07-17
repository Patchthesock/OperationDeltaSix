using UnityEngine;
using UnityEngine.UI;

namespace Assets.UIElements.RadialUI
{
    public class RadialManager : MonoBehaviour
    {
        public GameObject DominoesInventory;
        public GameObject PropsInventory;
        public GameObject MainUI;
        public bool InventoryOpen;
	    public GameObject ActiveInventory;
	
        public void DominoesButton()
        {
            DominoesInventory.SetActive(true);
            PropsInventory.SetActive(false);
            ActiveInventory = DominoesInventory;
            InventoryOpen = true;
        }
	
        public void PropsButton()
        {
            PropsInventory.SetActive(true);
            DominoesInventory.SetActive(false);
            ActiveInventory = PropsInventory;
            InventoryOpen = true;
        }

        public void ExitButton()
        {
            foreach(Transform child in MainUI.transform)
            {
                InventoryOpen = false;
                ActiveInventory.SetActive(false);
                MainUI.SetActive(false);
            }
        }
	
        public void ExitGame()
        {
            Application.Quit();
        }
	
        private void Update()
        {
            if (InventoryOpen == true)
            {
                if(Input.GetKeyUp(KeyCode.Escape))
                {
                    foreach(Transform child in MainUI.transform)
                    {
                        InventoryOpen = false;
                        ActiveInventory.SetActive(false);
                    }
                }
            }

            if (!Input.GetKeyDown(KeyCode.Space)) return;
            MainUI.transform.position = Input.mousePosition;
            MainUI.SetActive(true);
            InventoryOpen = true;
        }	
    }
}