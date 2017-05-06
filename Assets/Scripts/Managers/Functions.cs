using System.Collections.Generic;
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
            var rand = Random.Range(0, objects.Count);
            return objects[rand];
        }

        public static bool CanPlaceObject(IEnumerable<GameObject> objects, Vector3 position, float minDistance)
        {
            return objects.All(o => !(Vector3.Distance(o.transform.position, position) < minDistance));
        }
    }
}
