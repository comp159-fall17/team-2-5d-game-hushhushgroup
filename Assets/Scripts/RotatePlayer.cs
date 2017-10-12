﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatePlayer : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}


	// Update is called once per frame
	void Update () {
		float moveHorizontal = Input.GetAxis("Horizontal");
		float moveVertical = Input.GetAxis("Vertical");

		// TODO: Change to mouse position
		Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
		this.transform.rotation = Quaternion.LookRotation(movement);

	}
		
}