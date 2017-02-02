using Assets.Scripts.Controllers;
using UnityEngine;

namespace Assets.Scripts.States.Game
{
    public class GameStateBuild : GameState
    {
        //public GameStateBuild(
        //    DominoController dominoController)
        //{
        //    _dominoController = dominoController;
        //}

        public GameStateBuild(GameController gameController) : base(gameController)
        {
        }

        public override void Update()
        {
            Physics.gravity = new Vector3(0, 0, 0);
            _dominoController.SetPhysics(true);
        }

        private readonly DominoController _dominoController;
    }
}
