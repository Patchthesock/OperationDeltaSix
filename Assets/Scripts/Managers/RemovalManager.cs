using UnityEngine;
using Zenject;

namespace Assets.Scripts.Managers
{
    public class RemovalManager : ITickable
    {
        public RemovalManager(
            PlacedDominoManager placedDominoManager,
            PlacedObjectManager placedObjectManager)
        {
            _isActive = false;
            _placedDominoManager = placedDominoManager;
            _placedObjectManager = placedObjectManager;
        }

        public void SetActive(bool isActive)
        {
            _isActive = isActive;
        }

        public void Tick()
        {
            if (_isActive) RemoveItem();
        }

        private void RemoveItem()
        {
            if (!Input.GetMouseButtonDown(0)) return;
            RaycastHit hit;
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Physics.Raycast(ray, out hit, Mathf.Infinity);
            if (hit.collider == null) return;

            switch (hit.collider.gameObject.tag)
            {
                case "Ground":
                    return;
                case "Domino":
                    _placedDominoManager.RemoveDomino(hit.collider.gameObject);
                    return;
                case "Object":
                    _placedObjectManager.RemoveObject(hit.collider.gameObject);
                    return;
            }
        }

        private bool _isActive;
        private readonly PlacedDominoManager _placedDominoManager;
        private readonly PlacedObjectManager _placedObjectManager;
    }
}
