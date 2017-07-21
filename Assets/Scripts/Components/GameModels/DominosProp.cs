using Assets.Scripts.Interfaces;
using UnityEngine;

namespace Assets.Scripts.Components.GameModels
{
    public class DominosProp : MonoBehaviour, IPlacementable
    {
        public Dominos Dominos;
        public GameObject Prop;

        public GameObject GetGameObject()
        {
            return gameObject;
        }

        private void Start()
        {
            gameObject.name = Prop.name;
        }
    }
}
