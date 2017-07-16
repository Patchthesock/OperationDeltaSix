using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Components
{
    public class LoadGui : MonoBehaviour
    {
        public Button LoadBtn;
        public Button CloseBtn;
        public InputField LoadTxt;

        public void SetActive(bool state)
        {
            LoadTxt.text = "";
            gameObject.SetActive(state);
        }

        public void SetSaveList(IEnumerable<string> saveList)
        {
            foreach (var s in saveList) Debug.Log(s);
        }
    }
}
