using UnityEngine;
using System.Collections;
using System.Collections.ObjectModel;

/*
 * v0.2 Sep. 24, 2016
 *   - fix tri_index[]
 *   - add createMeshedDroxtal()
 *   - add tri_index[]
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

	int [] tri_index = new int[] {
		// top surface
		0, 1, 5, // ABF
		1, 2, 5, // BCF
		2, 4, 5, // CEF
		2, 3, 4, // CDE
		// upper sides ---
		0, 5, 6, // AA'F
		5, 11, 6,// A'F'F
		// 
		5, 4, 11,// FEF'
		4,10, 11,// EE'F'
		//
		4, 3, 10,// DEE'
		3, 9, 10,// DD'E'
		// ABA'B'
		1, 0, 6,// BAA' (not BA'B')
		1, 6, 7,// BA'B' (not ABA')
		//
		2, 1, 8,// CBC' (not BCB')
		1, 7, 8,// BB'C' (not CB'C')
		//
		3, 2, 9,// DCD' (not CDC')
		2, 8, 9,// CC'D' (not DC'D')
		// middle sides ---
		6,11,12,// A'F'P
		11,17,12,// F'UP
		//
		11,10,17,// E'F'U
		10,16,17,// E'TU
		// 
		10,9,16,// E'D'T
		9,15,16,// D'ST
		//
		7,6,12,// A'B'P
		7,12,13,// B'PQ
		//
		8, 7,13,// B'C'Q
		8,13,14,// C'RQ
		// 
		9, 8,14,// C'D'R
		9, 14,15,// D'SR
		// lower sides ---
		17,23,18,// UU'P'
		12,17,18,// UPP'
		//
		17,16,23,// UTU'
		16,22,23,// TT'U'
		//
		16,15,22,// TST'
		15,21,22,// SS'T'
		//
		13,12,18,// PQP'
		13,18,19,// QP'Q'
		// 
		14,13,19,// QRQ'
		14,19,20,// RQ'R'
		// 
		15,14,20,// RSR'
		15,20,21,// SR'S'
		// bottom surface ---
		19,18,23,// P'Q'U'
		20,19,23,// Q'R'U'
		21,20,22,// R'S'T'
		22,20,23,// R'T'U'
	};

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
		Debug.Log (tri_index [0]);
		Lone = radius * Mathf.Cos (theta1_rad);
		Ltwo = radius * Mathf.Cos (theta2_rad);
		calcVertices ();
		// arrayParticles ();	
		createMeshedDroxtal();
	}

	void arrayParticles() {
		for (int idx = 0; idx < kNumVerticles; idx++) {
			GameObject instance = (GameObject)Instantiate (my_obj);	
			instance.transform.position = vertices [idx];
		}
	}

	void createMeshedDroxtal() {
		MeshFilter mf = GetComponent<MeshFilter> ();
		Mesh mesh = new Mesh ();
		mf.mesh = mesh;

		// Normals
		Vector3[] normals = new Vector3[kNumVerticles];
		for (int idx = 0; idx < kNumVerticles; idx++) {
			normals [idx] = -Vector3.forward;
		}

		// UVs
		Vector2[] uv = new Vector2[kNumVerticles];
		for(int idx = 0; idx < kNumVerticles; idx++) {
			uv[idx] = new Vector2 (0, 0); // not used 
		}

		mesh.vertices = vertices;
		mesh.triangles = tri_index;
		mesh.normals = normals;
		mesh.uv = uv;
		mesh.RecalculateNormals ();
		mesh.RecalculateBounds ();
	}
	
	void Update () {

	}
}
