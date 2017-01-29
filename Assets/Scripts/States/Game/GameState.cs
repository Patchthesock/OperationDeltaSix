using Assets.Scripts.Controllers;

namespace Assets.Scripts.States.Game
{
    public abstract class GameState
    {
        protected GameState(GameController gameController)
        {
            GameController = gameController;
        }

        public virtual void Start()
        {
            
        }

        public virtual void Stop()
        {
            
        }

        public abstract void Update();

        protected GameController GameController;
    }
}
