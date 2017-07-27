using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Gui.Components
{
    public class MainMenuGui : MonoBehaviour
    {
        public Button PlayBtn;

        // Sub Menu
        public Button PropBtn;
        public Button PatternBtn;

        // Clear
        public Button RemoveBtn;
        public Button ClearDominosBtn;

        // Save & Load
        public Button SaveGuiBtn;
        public Button LoadGuiBtn;

        public List<RadialBtn> PropBtns;
        public List<RadialBtn> PatternBtns;

        public void SetActive(bool state)
        {
            gameObject.SetActive(state);
            PlayBtn.interactable = state;
            PropBtn.interactable = state;
            RemoveBtn.interactable = state;
            PatternBtn.interactable = state;
            SaveGuiBtn.interactable = state;
            LoadGuiBtn.interactable = state;
            ClearDominosBtn.interactable = state;
            if (state) return;
            SetPropMenuActive(false);
            SetPatternMenuActive(false);
        }

        public void SetPropMenuActive(bool state)
        {
            foreach (var b in PropBtns) b.SetActive(state);
        }

        public void SetPatternMenuActive(bool state)
        {
            foreach (var b in PatternBtns) b.SetActive(state);
        }
    }
}
