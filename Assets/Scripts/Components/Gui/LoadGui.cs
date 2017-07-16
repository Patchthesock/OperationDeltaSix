using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Components.Gui
{
    public class LoadGui : MonoBehaviour
    {
        public Button LoadBtn;
        public Button CloseBtn;
        public InputField LoadTxt;
        public Transform Container;
        public GameObject SaveOption;

        public void SetActive(bool state)
        {
            LoadTxt.text = "";
            gameObject.SetActive(state);
        }

        public void SetSaveList(IEnumerable<string> saveList)
        {
            foreach (var s in saveList) AddSaveOption(s);
        }

        private void AddSaveOption(string saveName)
        {
            var o = Instantiate(SaveOption);
            //var 
            Debug.Log(saveName);
        }
    }
}
