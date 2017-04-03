using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour {

	public int Xspeed;
	public int Yspeed;
	public int Zspeed;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		transform.Rotate (new Vector3(Xspeed,Yspeed,Zspeed)* Time.deltaTime);
	}
}
