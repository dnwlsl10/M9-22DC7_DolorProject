using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door_01 : MonoBehaviour {

	public Animation WingLeft;
	public Animation WingRight;

	void OnTriggerEnter(Collider c) {
		
		GetComponent<AudioSource> ().Play ();
		WingLeft ["door_01_wing_left"].speed = 1;
		WingRight ["door_01_wing_right"].speed = 1;
		WingLeft.Play ();
		WingRight.Play ();
	}

	void OnTriggerExit(Collider c) {
		GetComponent<AudioSource> ().Play ();
		WingLeft ["door_01_wing_left"].time = WingLeft ["door_01_wing_left"].length;
		WingRight ["door_01_wing_right"].time = WingRight ["door_01_wing_right"].length;
		WingLeft ["door_01_wing_left"].speed = -1;
		WingRight ["door_01_wing_right"].speed = -1;
		WingLeft.Play ();
		WingRight.Play ();
	}
}
