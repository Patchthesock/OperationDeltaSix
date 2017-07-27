﻿using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Gui.Components
{
    [RequireComponent(typeof(Image))]
    [RequireComponent(typeof(RectTransform))]
    public class RaycastMask : MonoBehaviour, ICanvasRaycastFilter
    {
        private Sprite _sprite;
 
        private void Start ()
        {
            _sprite = GetComponent<Image>().sprite;
        }
   
        public bool IsRaycastLocationValid(Vector2 sp, Camera eventCamera)
        {
            var rectTransform = (RectTransform)transform;
            Vector2 local;
            RectTransformUtility.ScreenPointToLocalPointInRectangle((RectTransform) transform, sp, eventCamera, out local);
            // normalize local coordinates
            var normalized = new Vector2(
                (local.x + rectTransform.pivot.x*rectTransform.rect.width)/rectTransform.rect.width,
                (local.y + rectTransform.pivot.y*rectTransform.rect.height)/rectTransform.rect.width);
            // convert to texture space
            var rect = _sprite.textureRect;
            var x = Mathf.FloorToInt(rect.x + rect.width * normalized.x);
            var y = Mathf.FloorToInt(rect.y + rect.height * normalized.y);
            // destroy component if texture import settings are wrong
            try
            {
                return _sprite.texture.GetPixel(x,y).a > 0;
            }
            catch (UnityException e)
            {
                Debug.LogError("Mask texture not readable, set your sprite to Texture Type 'Advanced' and check 'Read/Write Enabled'");
                Debug.LogError(e.Message);
                Destroy(this);
                return false;
            }
        }
    }
}
