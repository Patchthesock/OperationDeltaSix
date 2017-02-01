using UnityEngine;

namespace Assets.Scripts.Models
{
    public static class ModelFactory
    {
        public static ObjectPlacementModel CreateObjectPlacementModel(
            GameObject gameObject,
            Vector3 position,
            Vector3 rotation)
        {
            return new ObjectPlacementModel
            {
                Object = gameObject,
                Position = position,
                Rotation = rotation
            };
        }
    }
}
