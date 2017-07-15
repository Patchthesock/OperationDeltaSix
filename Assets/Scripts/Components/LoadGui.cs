using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Components
{
    public class LoadGui : MonoBehaviour
    {
        public Text LoadTxt;
        public Button LoadBtn;

        public void SetActive(bool state)
        {
            gameObject.SetActive(state);
        }
    }
}
