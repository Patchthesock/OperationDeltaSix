using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.Managers.Models;

namespace Assets.Scripts.Gui.Components
{
    [RequireComponent(typeof(Button))]
    public class RadialPlaceableBtn : MonoBehaviour
    {
        public GameObject Placeable;

        public void SetActive(bool state)
        {
            gameObject.SetActive(state);
        }

        public PlaceableBtn GetPlaceableBtn()
        {
            return _placeableBtn;
        }

        private void Start()
        {
            _placeableBtn = new PlaceableBtn
            {
                Placeable = Placeable,
                Button = gameObject.GetComponent<Button>()
            };
        }
    }

    private PlaceableBtn _placeableBtn;
}