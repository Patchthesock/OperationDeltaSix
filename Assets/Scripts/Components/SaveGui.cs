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
    }
}
