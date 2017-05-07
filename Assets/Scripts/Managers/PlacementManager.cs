﻿using System;
using Assets.Scripts.Components;
using Assets.Scripts.Models;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Managers
{
    public class PlacementManager : ITickable
    {
        public PlacementManager(
            Menu menu,
            Settings settings,
            PlacedDominoManager placedDominoManager,
            PlacedObjectManager placedObjectManager)
        {
            _menu = menu;
            _settings = settings;
            _placedDominoManager = placedDominoManager;
            _placedObjectManager = placedObjectManager;

            SetupButtons();
        }

        public void SetActive(bool state)
        {
            _isActive = state;
            SetMenu(state);
            _removingObjects = false;
            //Destroy(_ghostObject); // TODO
        }

        public void Tick()
        {
            if (!_isActive) return;
            if (Input.GetMouseButtonUp(0)) _mouseLock = false;
            if (Input.GetKey(KeyCode.Escape)) DestroyGhost();

            if (_removingObjects)
            {
                RemoveItem();
                return;
            }
            if (_ghostObject != null) AddItem(GetPlacementPosition() + new Vector3(0, 0.6f, 0), _ghostObject, _placedDominoManager);
        }

        private void AddItem(Vector3 ghostPos, GameObject ghostObject, PlacedDominoManager dominoManager)
        {
            if (ghostPos == new Vector3() || !Functions.CanPlaceObject(dominoManager.GetPlacedDominos(), ghostPos, 0.5f))
            {
                ghostObject.transform.position = new Vector3(999, 999, 999);
                Cursor.SetCursor(_settings.cursorCantPlaceTexture, Vector2.zero, CursorMode.Auto);
                return;
            }
            ghostObject.transform.position = ghostPos;
            Cursor.SetCursor(null, Vector2.zero, _settings.normalCursor);

            ghostObject.transform.rotation = GetDefaultRotation();
            var placementPosition = GetPlacementPosition(0);
            if (placementPosition == new Vector3()) return;
            if (!Functions.CanPlaceObject(dominoManager.GetPlacedDominos(), placementPosition, 0.5f))
            {
                Cursor.SetCursor(_settings.cursorCantPlaceTexture, Vector2.zero, CursorMode.Auto);
                return;
            }

            switch (ghostObject.tag)
            {
                case "MultiDomino":
                    PlaceMutliDomino(ghostPos);
                    break;
                case "Domino":
                    PlaceDomino(ghostPos);
                    break;
                case "Object":
                    PlaceProp(ghostPos);
                    break;
            }
        }

        private void PlaceDomino(Vector3 ghostPos)
        {
            _placedDominoManager.PlaceDomino(ghostPos, GetSingleDominoRotation(ghostPos));
            _timeLastPlaced = Time.time;
        }

        private void PlaceProp(Vector3 ghostPos)
        {
            if (_mouseLock) return;
            _placedObjectManager.AddObject(_ghostObject, ghostPos, _ghostObject.transform.rotation);
            var dom = _ghostObject.GetComponent<DominoHooks>();
            if (dom == null) return;
            foreach (var o in dom.Dominos) _placedDominoManager.PlaceDomino(o.transform.position, o.transform.rotation);
            _mouseLock = true;
        }

        private void PlaceMutliDomino(Vector3 ghostPos)
        {
            if (_mouseLock) return;
            var dom = _ghostObject.GetComponent<DominoHooks>();
            if (dom == null) return;
            foreach (var o in dom.Dominos) _placedDominoManager.PlaceDomino(new Vector3(o.transform.position.x, ghostPos.y, o.transform.position.z), o.transform.rotation);
            _mouseLock = true;
        }

        private static Vector3 GetPlacementPosition(int mouseButtonNumber)
        {
            if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject()) return new Vector3();
            return !Input.GetMouseButton(mouseButtonNumber) ? new Vector3() : GetPlacementPosition();
        }

        private static Vector3 GetPlacementPosition()
        {
            RaycastHit hit;
            var ray = UnityEngine.Camera.main.ScreenPointToRay(Input.mousePosition);
            Physics.Raycast(ray, out hit, Mathf.Infinity);
            if (hit.collider == null) return new Vector3();
            return hit.collider.gameObject.tag != "Ground" ? new Vector3() : hit.point;
        }

        private Quaternion GetSingleDominoRotation(Vector3 pos)
        {
            if (Time.time - _timeLastPlaced <= _settings.TimeToLine)
            {
                var rot = Quaternion.LookRotation(_positionLastPlaced - pos);
                _positionLastPlaced = pos;
                return rot;
            }
            _positionLastPlaced = pos;
            return GetDefaultRotation();
        }

        private static Quaternion GetDefaultRotation()
        {
            var rotation = Camera.Instance.TheCamera.transform.rotation.eulerAngles;
            rotation = new Vector3(0, rotation.y, rotation.z);
            return Quaternion.Euler(rotation);
        }

        private void SetMenu(bool state)
        {
            _menu.DominoMenuBtn.gameObject.SetActive(state);
            _menu.PropMenuBtn.gameObject.SetActive(state);
        }

        private void SetObject(GameObject model)
        {
            _removingObjects = false;
            //_ghostObject = Instantiate(model); // Ghost the Object // TODO
            if (_ghostObject.GetComponent<DominoHooks>()) foreach (var o in _ghostObject.GetComponent<DominoHooks>().Dominos) Functions.TurnOffGameObjectPhysics(o);
            else Functions.TurnOffGameObjectPhysics(_ghostObject);
        }

        private void RemoveObjects()
        {
            _removingObjects = true;
        }

        private void DestroyGhost()
        {
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
            if (_ghostObject == null) return;
            //Destroy(_ghostObject); TODO
        }

        private void RemoveItem()
        {
            if (!Input.GetMouseButtonDown(0)) return;
            RaycastHit hit;
            var ray = UnityEngine.Camera.main.ScreenPointToRay(Input.mousePosition);
            Physics.Raycast(ray, out hit, Mathf.Infinity);
            if (hit.collider == null) return;

            switch (hit.collider.gameObject.tag)
            {
                case "Ground":
                    return;
                case "Domino":
                    _placedDominoManager.RemoveDomino(hit.collider.gameObject);
                    break;
                case "Object":
                    _placedObjectManager.RemoveObject(hit.collider.gameObject);
                    break;
            }
        }

        private void OnCreateButtonClick(GameObject model)
        {
            DestroyGhost();
            TurnOffMenuUi();
            SetObject(model);
        }

        private void TurnOffMenuUi()
        {
            _radialManager.InventoryOpen = false;
            _radialManager.MainUI.SetActive(false);
            _radialManager.ActiveInventory.SetActive(false);
        }

        private void SetupButtons()
        {
            _menu.DominoTenBtn.onClick.AddListener(() => { OnCreateButtonClick(_settings.TenDomino); });
            _menu.DominoFiveBtn.onClick.AddListener(() => { OnCreateButtonClick(_settings.FiveDomino); });
            _menu.PropBridgeBtn.onClick.AddListener(() => { OnCreateButtonClick(_settings.BridgeProp); });
            _menu.DominoOneBtn.onClick.AddListener(() => { OnCreateButtonClick(_settings.SingleDomino); });
            _menu.DominoNintyLeftBtn.onClick.AddListener(() => { OnCreateButtonClick(_settings.NintyLeft); });
            _menu.DominoTwentyBtn.onClick.AddListener(() => { OnCreateButtonClick(_settings.TwentyDomino); });
            _menu.PropStepSlideBtn.onClick.AddListener(() => { OnCreateButtonClick(_settings.StepSlideProp); });
            _menu.DominoNintyRightBtn.onClick.AddListener(() => { OnCreateButtonClick(_settings.NintyRight); });
            _menu.DominoOneEightyTurnBtn.onClick.AddListener(() => { OnCreateButtonClick(_settings.OneEightyTurn); });

            _menu.ClearDominos.onClick.AddListener(() =>
            {
                DestroyGhost();
                TurnOffMenuUi();
                _placedDominoManager.RemoveDomino();
            });

            _menu.RemoveBtn.onClick.AddListener(() =>
            {
                DestroyGhost();
                RemoveObjects();
            });
        }

        private bool _mouseLock;
        private bool _removingObjects;
        private bool _isActive = true;
        private float _timeLastPlaced;
        private Vector3 _positionLastPlaced;
        private GameObject _ghostObject;
        private RadialManager _radialManager;
        
        private readonly Menu _menu;
        private readonly Settings _settings;
        private readonly PlacedObjectManager _placedObjectManager;
        private readonly PlacedDominoManager _placedDominoManager;

        [Serializable]
        public class Settings
        {
            // Dominos
            public GameObject SingleDomino;
            public GameObject FiveDomino;
            public GameObject TenDomino;
            public GameObject TwentyDomino;
            public GameObject NintyLeft;
            public GameObject NintyRight;
            public GameObject OneEightyTurn;

            // Props
            public GameObject BridgeProp;
            public GameObject StepSlideProp;

            // Settings
            public float TimeToLine;
            public CursorMode normalCursor;
            public Texture2D cursorCantPlaceTexture;
        }
    }
}
