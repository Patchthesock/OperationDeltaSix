using Assets.Scripts.Models;
using UnityEngine;

namespace Assets.Scripts.Controllers
{
    public class PlacementController
    {
        public PlacementController(
            MenuController menuController,
            GhostController ghostController,
            DominoController dominoController,
            RemovalController removeController)
        {
            _menuController = menuController;
            _ghostController = ghostController;
            _dominoController = dominoController;
            _removalController = removeController;
        }

        public void Initialize()
        {
            _current = 0;
            _menuController.Initialize();
            _menuController.SubscribeToOnItemDropped(OnMenuItemRemoved);
            _menuController.SubscribeToOnItemSelected(OnMenuItemSelected);
            _ghostController.SubscribeToOnItemPlaced(OnItemPlaced);
            _removalController.SubscribeToOnItemRemoved(OnItemRemoved);
        }

        public void SetState(bool isActive)
        {
            _menuController.SetState(isActive);
            if (isActive) return;
            _ghostController.Drop();
            _removalController.Remove(false);
        }

        private void OnMenuItemSelected(GameObject model)
        {
            _removalController.Remove(false);
            _ghostController.Select(model);
        }

        private void OnMenuItemRemoved()
        {
            _ghostController.Drop();
            _removalController.Remove(true);
        }

        private void OnItemPlaced(ObjectPlacementModel model)
        {
            _dominoController.PlaceDomino(GenerateName(), model.Position, model.Rotation);
        }

        private void OnItemRemoved(string name)
        {
            _dominoController.RemoveDomino(name);
        }

        private string GenerateName()
        {
            _current = _current + 1;
            return _current.ToString();
        }

        private int _current;
        private readonly MenuController _menuController;
        private readonly GhostController _ghostController;
        private readonly DominoController _dominoController;
        private readonly RemovalController _removalController;
    }
}
