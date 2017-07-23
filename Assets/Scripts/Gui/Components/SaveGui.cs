using System.Collections.Generic;
using Assets.Scripts.Gui.Services;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Gui.Components
{
    public class SaveGui : MonoBehaviour
    {
        public Button SaveBtn;
        public Button CloseBtn;
        public InputField SaveTxt;
        public Transform Container;

        public void Initialize(BtnOptionFactory btnOptionFactory)
        {
            _btnOptionFactory = btnOptionFactory;
        }

        public void SetActive(bool state)
        {
            SaveTxt.text = "";
            gameObject.SetActive(state);
            if (state) return;
            ClearBtnOptionList();
        }

        public void SetSaveList(IEnumerable<string> saveList)
        {
            foreach (var s in saveList) _btnOptionFactory.Create("save").Initialize(s, OnBtnOptionClick, Container);
        }
        
        private void OnBtnOptionClick(string saveName)
        {
            SaveTxt.text = saveName;
        }

        private void ClearBtnOptionList()
        {
            if (_btnOptionFactory == null) return;
            _btnOptionFactory.Clear("save");
        }

        private BtnOptionFactory _btnOptionFactory;
    }
}
