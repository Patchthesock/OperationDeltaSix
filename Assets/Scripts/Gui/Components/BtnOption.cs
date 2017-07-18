using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Gui.Components
{
    public class BtnOption : MonoBehaviour
    {
        public Text Text;
        public Button Btn;

        public void Initialize(string optionName, Action<string> onClick, Transform container)
        {
            SetActive(true);
            Text.text = optionName;
            transform.SetParent(container);
            transform.localScale = new Vector3(1, 1, 1);
            Btn.onClick.AddListener(() => { onClick(optionName); });
        }

        public void SetActive(bool state)
        {
            gameObject.SetActive(state);
        }
    }
}
