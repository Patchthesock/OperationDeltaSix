using System;
using UnityEngine;

namespace Assets.Scripts.Controllers
{
    public class PlacementController
    {
        public PlacementController(
            MenuController menuController,
            GhostController ghostController)
        {
            _menuController = menuController;
            _ghostController = ghostController;
        }

        public void Initialize()
        {
            _menuController.Initialize();
            _menuController.SubscribeToOnItemDropped(OnMenuItemRemoved);
            _menuController.SubscribeToOnItemSelected(OnMenuItemSelected);
        }

        private void OnMenuItemSelected(GameObject model)
        {
            _ghostController.Select(model);
        }

        private void OnMenuItemRemoved()
        {
            _ghostController.Drop();
        }

        private readonly MenuController _menuController;
        private readonly GhostController _ghostController;
    }
}
