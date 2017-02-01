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
            _menuController.Initialize();
            _menuController.SubscribeToOnItemDropped(OnMenuItemRemoved);
            _menuController.SubscribeToOnItemSelected(OnMenuItemSelected);
            _ghostController.SubscribeToOnItemPlaced(OnItemPlaced);
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
            _dominoController.PlaceDomino(model.Position, model.Rotation);
        }

        private void OnItemRemoved(string name)
        {
            
        }

        private readonly MenuController _menuController;
        private readonly GhostController _ghostController;
        private readonly DominoController _dominoController;
        private readonly RemovalController _removalController;
    }
}
