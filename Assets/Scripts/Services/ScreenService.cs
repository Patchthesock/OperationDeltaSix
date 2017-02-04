using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Services
{
    public class ScreenService
    {
        public ScreenService(AsyncProcessor asyncProcessor)
        {
            _asyncProcessor = asyncProcessor;
            _openParameterId = Animator.StringToHash(OpenTransitionName);
        }

        public void OpenPanel(Animator animator)
        {
            ClosePanel();
            if (_currentAnimator == animator) return;
            animator.gameObject.SetActive(true);
            _currentAnimator = animator;
            _currentAnimator.SetBool(_openParameterId, true);
        }

        public void ClosePanel()
        {
            if (_currentAnimator == null) return;
            _currentAnimator.SetBool(_openParameterId, false);
            _asyncProcessor.StartCoroutine(DisablePanelDelayed(_currentAnimator));
            _currentAnimator = null;
        }

        private IEnumerator DisablePanelDelayed(Animator animator)
        {
            var closedStateReached = false;
            while (!closedStateReached)
            {
                if (!animator.IsInTransition(0))
                    closedStateReached = animator.GetCurrentAnimatorStateInfo(0).IsName(ClosedTransitionName);

                yield return new WaitForEndOfFrame();
            }
            //animator.gameObject.SetActive(false);
        }

        private Animator _currentAnimator;
        private readonly int _openParameterId;
        private readonly AsyncProcessor _asyncProcessor;
        private const string OpenTransitionName = "Open";
        private const string ClosedTransitionName = "Closed";
    }
}
