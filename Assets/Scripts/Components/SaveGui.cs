using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Components
{
    public class SaveGui : MonoBehaviour
    {
        public Text SaveTxt;
        public Button SaveBtn;

        public void SetActive(bool state)
        {
            gameObject.SetActive(state);
        }
    }
}
