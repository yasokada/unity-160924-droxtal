using UnityEngine;
using System.Collections;

/*
 * v0.1 Sep. 24, 2016
 *   - add CameraRotation script on [Main Camera]
 *   - put 20 particles randomly
 */

public class makeDroxtal : MonoBehaviour {

	public GameObject my_obj;

	void Start () {
		arrayParticles ();	
	}

	void arrayParticles() {
		for (int idx = 0; idx < 20; idx++) {
			GameObject instance = (GameObject)Instantiate (my_obj);	
			instance.transform.position = my_obj.transform.position;

			Vector3 pos = new Vector3 ();
			pos.x = Random.Range (0, 2f) - 1f;
			pos.y = Random.Range (0, 2f) - 1f;
			pos.z = Random.Range (0, 2f) - 1f;
			instance.transform.position = pos;
		}
	}
	
	void Update () {

	}
}
