using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Interfaces
{
    public interface IUserCameraInput
    {
        Vector2 GetRotationLookInput();
        float GetVerticalPositionInput();
        Vector2 GetHorizontalPositionInput();
    }
}
