using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayViewer : MonoBehaviour {

	public float weaponRange = 50f;

	private Camera fpsCam;


	void Start () {
		fpsCam = GetComponentInParent<Camera> ();
	}
	

	void Update () {
		Vector3 lineOrigin = fpsCam.ViewportToWorldPoint (new Vector3 (0.05f, 0.05f, 0));
		//fpsCam.transform.forward * weaponRange specifies the direction
		// i.e., away from the camera
		Debug.DrawRay(lineOrigin, fpsCam.transform.forward * weaponRange, Color.green);
	}
}
