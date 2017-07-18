﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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
            return !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject() && Input.GetMouseButton(mouseButtonNumber);
        }
    }
}
