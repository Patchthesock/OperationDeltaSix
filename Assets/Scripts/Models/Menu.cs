using System;
using UnityEngine.UI;

namespace Assets.Scripts.Models
{
    [Serializable]
    public class Menu
    {
        // Domino Buttons
        public Button DominoOneBtn;
        public Button DominoFiveBtn;
        public Button DominoTenBtn;
        public Button DominoTwentyBtn;
        public Button DominoNintyLeftBtn;
        public Button DominoNintyRightBtn;
        public Button DominoOneEightyTurnBtn;

        // Prop Buttons
        public Button PropBridgeBtn;
        public Button PropStepSlideBtn;

        public Button ClearDominos;
        public Button RemoveBtn;
        public Button DominoMenuBtn;
        public Button PropMenuBtn;

        // Save Buttons
        public Button SaveBtn;
        public Button ResetBtn;

        // Operation Buttons
        public Button PlayBtn;
    }
}
