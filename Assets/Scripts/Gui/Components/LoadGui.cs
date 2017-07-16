using System.Collections.Generic;
using Assets.Scripts.Gui.Services;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Gui.Components
{
    public class LoadGui : MonoBehaviour
    {
        public Button LoadBtn;
        public Button CloseBtn;
        public InputField LoadTxt;
        public Transform Container;
        public GameObject SaveOption;

        public void Initialize(BtnOptionFactory btnOptionFactory)
        {
            _btnOptionFactory = btnOptionFactory;
        }

        public void SetActive(bool state)
        {
            LoadTxt.text = "";
            gameObject.SetActive(state);
        }

        public void SetSaveList(IEnumerable<string> saveList)
        {
            foreach (var s in saveList) _btnOptionFactory.Create("save").Initialize(s, OnBtnOptionClick, Container);
        }

        private void OnBtnOptionClick(string saveName)
        {
            LoadTxt.text = saveName;
        }

        private void ClearBtnOptionList()
        {
            _btnOptionFactory.Clear("save");
        }

        private BtnOptionFactory _btnOptionFactory;
    }
}
