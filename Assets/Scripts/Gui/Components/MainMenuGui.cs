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
        public Button ExitBtn;
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
            if (state)
            {
                transform.position = GetPosition(Input.mousePosition);
                return;
            }
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

        private void Awake()
        {
            var rect = gameObject.GetComponent<RectTransform>();
            _minX = rect.sizeDelta.x;
            _minY = rect.sizeDelta.y;
            _maxX = Screen.width - _minX;
            _maxY = Screen.height - _minY;
        }

        private Vector3 GetPosition(Vector3 mousePosition)
        {
            if (mousePosition.x > _maxX) mousePosition.x = _maxX;
            if (mousePosition.x < _minX) mousePosition.x = _minX;
            if (mousePosition.y > _maxY) mousePosition.y = _maxY;
            if (mousePosition.y < _minY) mousePosition.y = _minY;
            return mousePosition;
        }

        private float _minX;
        private float _minY;
        private float _maxX;
        private float _maxY;
    }
}
