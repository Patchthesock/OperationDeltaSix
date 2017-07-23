using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Gui.Components
{
    public class MainUI : MonoBehaviour
    {
        public List<RadialBtn> PropBtns;
        public List<RadialBtn> PatternBtns;

        public void SetActive(bool state)
        {
            gameObject.SetActive(state);
            if (state) return;
            foreach (var b in PropBtns) b.SetActive(false);
            foreach (var b in PatternBtns) b.SetActive(false);
        }
    }
}
