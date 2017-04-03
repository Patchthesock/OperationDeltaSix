using UnityEngine;
using System.Collections;

public class Rotator : MonoBehaviour {
	public float Xspeed = 0;
	public float Yspeed = 0;
	public float Zspeed = 0;

	
	// Update is called once per frame
	void Update () {
		transform.Rotate(new Vector3(Xspeed, Yspeed, Zspeed)  * Time.deltaTime, Space.World);
	}
}
