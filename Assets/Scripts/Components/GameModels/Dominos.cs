using System.Collections.Generic;
using Assets.Scripts.Interfaces;
using UnityEngine;

namespace Assets.Scripts.Components.GameModels
{
    public class Dominos : MonoBehaviour, IPlacementable
    {
        public List<Domino> Domino;

        public GameObject GetGameObject()
        {
            return gameObject;
        }
    }
}
