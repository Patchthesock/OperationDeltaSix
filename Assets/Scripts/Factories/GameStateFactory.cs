using System;
using Assets.Scripts.States.Game;
using Zenject;

namespace Assets.Scripts.Factories
{
    public class GameStateFactory
    {
        public GameStateFactory(DiContainer container)
        {
            _container = container;
        }

        public GameState Create(GameStates state, params object[] constructorArgs)
        {
            switch (state)
            {
                case GameStates.Play:
                    return _container.Instantiate<GameStatePlay>(constructorArgs);

                case GameStates.Build:
                    return _container.Instantiate<GameStateBuild>(constructorArgs);

                default:
                    throw new ArgumentException();
            }
        }

        public enum GameStates
        {
            Play,
            Build
        }

        private readonly DiContainer _container;
    }
}
