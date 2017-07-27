using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Gui.Components
{
    public class SaveConfirmGui : MonoBehaviour
    {
        public Button Cancel;
        public Button Confirm;

        public void Initialize(Action onContirm, Action onCancel)
        {
            Cancel.onClick.AddListener(() => onCancel());
            Confirm.onClick.AddListener(() => onContirm());
        }

        public void SetActive(bool state)
        {
            gameObject.SetActive(state);
        }
    }
}
