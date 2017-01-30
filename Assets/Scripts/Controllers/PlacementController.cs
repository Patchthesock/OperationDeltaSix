using Assets.Scripts.Models;
using UnityEngine;

namespace Assets.Scripts.Controllers
{
    public class PlacementController
    {
        public PlacementController(
            MenuController menuController,
            GhostController ghostController,
            DominoController dominoController)
        {
            _menuController = menuController;
            _ghostController = ghostController;
            _dominoController = dominoController;
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
            _ghostController.Select(model);
        }

        private void OnMenuItemRemoved()
        {
            _ghostController.Drop();
        }

        private void OnItemPlaced(ObjectPlacementModel model)
        {
            _dominoController.PlaceDomino(model.Position, model.Rotation);
        }

        private readonly MenuController _menuController;
        private readonly GhostController _ghostController;
        private readonly DominoController _dominoController;
    }
}
