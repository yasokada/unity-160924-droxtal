using UnityEngine;
using System.Collections;

/*
 * v0.1 Sep. 24, 2016
 *   - add calcVertices() using random position
 *   - add [kNumVerticles]
 *   - add CameraRotation script on [Main Camera]
 *   - put 20 particles randomly
 */

public class makeDroxtal : MonoBehaviour {

	public GameObject my_obj;

	public const int kNumVerticles = 20;

	Vector3[] vertices = new Vector3[kNumVerticles];

	void calcVertices() {
		for (int idx = 0; idx < kNumVerticles; idx++) {
			vertices [idx].x = Random.Range (0, 2f) - 1f;
			vertices [idx].y = Random.Range (0, 2f) - 1f;
			vertices [idx].z = Random.Range (0, 2f) - 1f;
		}
	}

	void Start () {
		calcVertices ();
		arrayParticles ();	
	}

	void arrayParticles() {
		for (int idx = 0; idx < kNumVerticles; idx++) {
			GameObject instance = (GameObject)Instantiate (my_obj);	
			instance.transform.position = vertices [idx];
		}
	}
	
	void Update () {

	}
}
