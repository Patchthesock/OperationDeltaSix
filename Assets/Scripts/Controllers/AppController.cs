using Zenject;

namespace Assets.Scripts.Controllers
{
    public class AppController : IInitializable
    {
        public AppController(GameController gameController)
        {
            _gameController = gameController;
        }

        public void Initialize()
        {
            _gameController.Initialize();
        }


        private readonly GameController _gameController;
    }
}
