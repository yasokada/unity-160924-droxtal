using UnityEngine;
using System.Collections;

/*
 * v0.1 Sep. 24, 2016
 *   - add makeMiddles()
 *   - add makeUpperOrLower()
 *   - add [radius]
 *   - add calcVertices() using random position
 *   - add [kNumVerticles]
 *   - add CameraRotation script on [Main Camera]
 *   - put 20 particles randomly
 */


/*
Reference 
  - http://www.ssec.wisc.edu/~BAUM/Papers/AO_Zhang_2004.pdf
     + Equations 1-3
*/

public class makeDroxtal : MonoBehaviour {

	public GameObject my_obj;
	public float radius = 5.0f;
	public float theta1_rad = 30f * Mathf.Deg2Rad; // zenith angle 1
	public float theta2_rad = 60f * Mathf.Deg2Rad; // zenith angle 2

	public const int kNumVerticles = 24;

	float Lone;
	float Ltwo;
	Vector3[] vertices = new Vector3[kNumVerticles];

	void makeUpperOrLower(int start, bool withPrime) {
		float phi_deg = 0f;
		float phi_rad;
		float Aone = radius * Mathf.Sin (theta1_rad);
			
		for (int idx = start; idx < (start + 6); idx++) {
			phi_rad = phi_deg * Mathf.Deg2Rad;
			vertices [idx].x = Aone * Mathf.Sin (phi_rad);
			if (withPrime) {
				vertices [idx].y = -Lone;
			} else {
				vertices [idx].y = Lone;
			}
			vertices [idx].z = Aone * Mathf.Cos (phi_rad);
			phi_deg += 60f;
		}
	}

	void makeMiddles(int start, bool withPrime) {
		float phi_deg = 0f;
		float phi_rad;
		float Atwo = radius * Mathf.Sin (theta2_rad);

		for (int idx = start; idx < (start + 6); idx++) {
			phi_rad = phi_deg * Mathf.Deg2Rad;
			vertices [idx].x = Atwo * Mathf.Sin (phi_rad);
			if (withPrime) {
				vertices [idx].y = Ltwo;
			} else {
				vertices [idx].y = -Ltwo;
			}
			vertices [idx].z = Atwo * Mathf.Cos (phi_rad);
			phi_deg += 60f;
		}
	}
		
	void calcVertices() {
		// dummy
		for (int idx = 0; idx < kNumVerticles; idx++) {
			vertices [idx].x = Random.Range (0, 2f) - 1f;
			vertices [idx].y = Random.Range (0, 2f) - 1f;
			vertices [idx].z = Random.Range (0, 2f) - 1f;
		}

		// make Droxtal
		makeUpperOrLower (/*start=*/0, /*withPrimer=*/false); // ABCDEF
		makeMiddles (/*start=*/6, /*withPrimer=*/true); // A'B'C'D'E'F'
		makeMiddles (/*start=*/12, /*withPrimer=*/false); // PQRSTU
		makeUpperOrLower (/*start=*/18, /*withPrimer=*/true); // P'Q'R'S'T'U'
	}

	void Start () {
		Lone = radius * Mathf.Cos (theta1_rad);
		Ltwo = radius * Mathf.Cos (theta2_rad);
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
