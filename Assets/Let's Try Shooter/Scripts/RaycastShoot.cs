using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastShoot : MonoBehaviour {

	public int gunDamage = 1;
	public float fireRate = .25f;
	public float weaponRange = 50;
	public float hitForce = 100;
	public Transform gunEnd;


	//Get camera reference.
	private Camera fpsCam;
	//Used to determine how long the laser should be displayed on the screen.
	// Needed whenever player shoots => Coroutine.
	private WaitForSeconds shotDuration = new WaitForSeconds(.07f);
	private AudioSource gunAudio;
	//Takes an array of 2+ points in 2D space and draws a straight line between 
	// pair in the gameview. 
	private LineRenderer laserLine;
	//The time WHEN player can fire again.
	private float nextFire; 


	void Start () {
		
		laserLine = GetComponent<LineRenderer> ();
		gunAudio = GetComponent<AudioSource> ();
		// Goes up the hierarchy and returns first. 
		fpsCam = GetComponentInParent<Camera> ();
	}


	void Update () {
		
		if (Input.GetButtonDown("Fire1") && Time.time > nextFire) {
			nextFire = Time.time + fireRate;

			StartCoroutine (ShotEffect());

			//Vector3 (0.5f, 0.5f, 0) in Viewport is the middle of the screen:
			// (0,0) is lower L corner;
			// (1,1) is upper R corner.
			Vector3 rayOrigin = fpsCam.ViewportToWorldPoint (new Vector3 (0.5f, 0.5f, 0));
			RaycastHit hit; 

			laserLine.SetPosition (0, gunEnd.position);

			// If RayCast hits sth straight ahead
			if (Physics.Raycast (rayOrigin, fpsCam.transform.forward, out hit, weaponRange)) {
				// Set second line position to where it hit.
				laserLine.SetPosition (1, hit.point);

				ShootableBox health = hit.collider.GetComponent<ShootableBox> ();

				if (health != null) {
					health.Damage (gunDamage);
				}

				if (hit.rigidbody != null) {
					hit.rigidbody.AddForce (-hit.normal * hitForce);
				}

			} else {
				//Set end of line 50 units away from origin in the fw direction of camera,
				// as we still need to show the line even if it didn't hit anything. 
				laserLine.SetPosition (1, rayOrigin + (fpsCam.transform.forward * weaponRange));
			}
		}
	}


	private IEnumerator ShotEffect() {
		
		gunAudio.Play ();

		laserLine.enabled = true;
		yield return shotDuration;
		laserLine.enabled = false;

	}
}
