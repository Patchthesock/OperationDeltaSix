using System;
using System.Collections.Generic;
using Zenject;

namespace Assets.Scripts.Controllers
{
    public class RemovalController : ITickable
    {

        public void Remove(bool state)
        {
            _removeing = state;
        }

        public void SubscribeToOnItemRemoved(Action<string> onItemRemoved)
        {
            if (!_onItemRemovedListeners.Contains(onItemRemoved)) return;
            _onItemRemovedListeners.Add(onItemRemoved);
        }

        public void Tick()
        {
            if (!_removeing) return;

        }

        private void OnItemRemoved(string name)
        {
            foreach (var l in _onItemRemovedListeners)
            {
                l(name);
            }
        }

        private bool _removeing;
        private List<Action<string>> _onItemRemovedListeners = new List<Action<string>>();
    }
}
