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
        }

        public PlaceableBtn GetPlaceableBtn()
        {
            return new PlaceableBtn
            {
                Model = Placeable,
                Btn = gameObject.GetComponent<Button>()
            };
        }
    }
}