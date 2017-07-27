using Assets.Scripts.Models;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Gui.Components
{
    [RequireComponent(typeof(Button))]
    public class RadialBtn : MonoBehaviour
    {
        public GameObject Container;
        public GameObject Placeable;

        public void SetActive(bool state)
        {
            Container.SetActive(state);
            gameObject.SetActive(state);
            if (_placeableBtn == null) return;
            _placeableBtn.Btn.interactable = state;
        }

        public PlaceableBtn GetPlaceableBtn()
        {
            return _placeableBtn;
        }

        private void Start()
        {
            _placeableBtn = new PlaceableBtn
            {
                Model = Placeable,
                Btn = gameObject.GetComponent<Button>()
            };
        }

        private PlaceableBtn _placeableBtn;
    }
}