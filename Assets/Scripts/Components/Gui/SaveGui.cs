using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Components.Gui
{
    public class SaveGui : MonoBehaviour
    {
        public Button SaveBtn;
        public Button CloseBtn;
        public InputField SaveTxt;
        public Transform Container;
        public GameObject SaveOption;

        public void SetActive(bool state)
        {
            SaveTxt.text = "";
            gameObject.SetActive(state);
            if (state) return;
            ClearSaveOptionList();
        }

        public void SetSaveList(IEnumerable<string> saveList)
        {
            foreach (var s in saveList) AddSaveOption(s);
        }

        private void AddSaveOption(string saveName)
        {
            var option = _nonActiveSaveOptions.Count > 0 ? GetExistingSaveOption() : GetNewSaveOption();
            _activeSaveOptions.Add(option);
            option.Initialize(saveName, OnSaveOptionClick);
        }

        private SaveOption GetNewSaveOption()
        {
            var o = Instantiate(SaveOption);
            o.transform.SetParent(Container);
            o.transform.localScale = new Vector3(1,1,1);
            return o.GetComponent<SaveOption>();
        }

        private SaveOption GetExistingSaveOption()
        {
            var i = _nonActiveSaveOptions.First();
            _nonActiveSaveOptions.Remove(i);
            return i;
        }

        private void OnSaveOptionClick(string saveName)
        {
            SaveTxt.text = saveName;
        }

        private void ClearSaveOptionList()
        {
            foreach (var s in _activeSaveOptions)
            {
                s.SetActive(false);
                _nonActiveSaveOptions.Add(s);
            }
            _activeSaveOptions.Clear();
        }

        private readonly List<SaveOption> _activeSaveOptions = new List<SaveOption>();
        private readonly List<SaveOption> _nonActiveSaveOptions = new List<SaveOption>();
    }
}
