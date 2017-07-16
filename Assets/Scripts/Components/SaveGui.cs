using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Components
{
    public class SaveGui : MonoBehaviour
    {
        public Button SaveBtn;
        public Button CloseBtn;
        public InputField SaveTxt;

        public void SetActive(bool state)
        {
            SaveTxt.text = "";
            gameObject.SetActive(state);
        }

        public void SetSaveList(IEnumerable<string> saveList)
        {
            foreach (var s in saveList) Debug.Log(s);
        }
    }
}
