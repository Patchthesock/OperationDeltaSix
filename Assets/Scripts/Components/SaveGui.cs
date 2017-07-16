using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Components
{
    public class SaveGui : MonoBehaviour
    {
        public Text SaveTxt;
        public Button SaveBtn;
        public Button CloseBtn;

        public void SetActive(bool state)
        {
            gameObject.SetActive(state);
        }

        public void SetSaveList(IEnumerable<string> saveList)
        {
            foreach (var s in saveList) Debug.Log(s);
        }
    }
}
