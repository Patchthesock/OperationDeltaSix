using Assets.Scripts.Controllers;
using UnityEngine;

namespace Assets.Scripts.States.Game
{
    public class GameStatePlay : GameState
    {
        public GameStatePlay(GameController gameController) : base(gameController)
        {
        }

        public override void Update()
        {
            Physics.gravity = new Vector3(0, -50, 0);
        }
    }
}
