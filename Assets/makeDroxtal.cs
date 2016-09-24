﻿using UnityEngine;
using System.Collections;
using System.Collections.ObjectModel;

/*
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
		0, 6, 7, // AA'B'
		6, 7, 11,// A'B'F'
		// 
		4, 5, 11,// EFF'
		4,10, 11,// EE'F'
		//
		3, 4, 10,// DEE'
		3, 9, 10,// DD'E'
		//
		0, 1, 6,// ABA'
		1, 6, 7,// BA'B'
		//
		1, 2, 7,// BCB'
		2, 7, 8,// CB'C'
		//
		2, 3, 8,// CDC'
		3, 8, 9,// DC'D'
		// middle sides ---
		6,11,12,// A'F'P
		11,17,12,// F'UP
		//
		10,11,17,// E'F'U
		10,16,17,// E'TU
		// 
		10,9,16,// E'D'T
		9,15,16,// D'ST
		//
		6,7,12,// A'B'P
		7,12,13,// B'PQ
		//
		7, 8,13,// B'C'Q
		8,14,13,// C'RQ
		// 
		8, 9,14,// C'D'R
		9, 15,14,// D'SR
		// lower sides ---
		12,17,18,// PUP'
		17,18,23,// UP'U'
		//
		17,16,23,// UTU'
		16,22,23,// TT'U'
		//
		16,15,22,// TST'
		15,21,22,// SS'T'
		//
		12,13,18,// PQP'
		13,18,19,// QP'Q'
		// 
		13,14,19,// QRQ'
		14,20,19,// RR'Q'
		// 
		14,15,20,// RSR'
		15,21,20,// SS'R'
		// bottom surface ---
		18,19,23,// P'Q'U'
		19,20,23,// Q'R'U'
		20,22,23,// R'T'U'
		20,21,22,// R'S'T'
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
	}
	
	void Update () {

	}
}
