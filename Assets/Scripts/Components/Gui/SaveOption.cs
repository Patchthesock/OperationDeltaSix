using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Components.Gui
{
    public class SaveOption : MonoBehaviour
    {
        public Text Text;
        public Button Btn;

        public void Initialize(string option, Action<string> onClick)
        {
            SetActive(true);
            Text.text = option;
            Btn.onClick.AddListener(() =>
            {
                onClick(option);
            });
        }

        public void SetActive(bool state)
        {
            gameObject.SetActive(state);
        }
    }
}
