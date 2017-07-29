using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Components.GameModels;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Managers
{
    public static class Functions
    {
        public static void TurnOffGameObjectPhysics(GameObject o)
        {
            o.gameObject.layer = 2;
            if (o.GetComponent<Collider>() != null) o.GetComponent<Collider>().isTrigger = true;
            if (o.GetComponent<Rigidbody>() == null) return;
            o.GetComponent<Rigidbody>().isKinematic = true;
            o.GetComponent<Rigidbody>().useGravity = false;
        }

        public static GameObject PickRandomObject(IList<GameObject> objects)
        {
            return objects[Random.Range(0, objects.Count)];
        }

        public static bool CanPlaceObject(IEnumerable<GameObject> objects, Vector3 position, float minDistance)
        {
            return objects.All(o => !(Vector3.Distance(o.transform.position, position) < minDistance));
        }

        public static RaycastHit GetHit()
        {
            RaycastHit hit;
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Physics.Raycast(ray, out hit, Mathf.Infinity);
            return hit;
        }

        public static Vector3 GetPlacementPosition()
        {
            var hit = GetHit();
            if (hit.collider == null) return Vector3.zero;
            return hit.collider.gameObject.tag != "Ground" ? Vector3.zero : hit.point;
        }

        public static bool GetMouseButtonInput(int mouseButtonNumber)
        {
            return !IsPointerOverGameObject() && Input.GetMouseButton(mouseButtonNumber);
        }

        public static bool GetMouseButtonDownInput(int mouseButtonNumber)
        {
            return !IsPointerOverGameObject() && Input.GetMouseButtonDown(mouseButtonNumber);
        }

        public static Dictionary<Type, int> GetPlaceableTypeDictionary()
        {
            return new Dictionary<Type, int>
            {
                { typeof(Domino), 0 },
                { typeof(Dominos), 1 },
                { typeof(DominosProp), 2}
            };
        }

        private static bool IsPointerOverGameObject()
        {
            return UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject();
        }
    }
}
