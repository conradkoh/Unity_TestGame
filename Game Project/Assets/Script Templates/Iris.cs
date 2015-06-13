using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]

public class Iris : MonoBehaviour {
	public int _verticeCount = 15;
	public float _innerRadius = 5f;
	public float _outerRadius = 7f;
	
	void Start () {
		BuildMesh (_verticeCount, _innerRadius, _outerRadius);
	}
	void Update(){
		BuildMesh(_verticeCount, _innerRadius, _outerRadius);
	}

	void UpdateMesh(){

	}


	void BuildMesh(int numVerts, float innerRadius, float outerRadius){
		numVerts = (numVerts/2 * 2);
		Debug.Log ("build mesh started");

		//TEST
        //Declaration of parameters
		int numTris = ((numVerts / 2));
		float interval = (2 * Mathf.PI) / (numTris);


		Vector3[] vertices = new Vector3[numVerts];
		Vector3[] normals = new Vector3[numVerts];
		Vector2[] uv = new Vector2[numVerts];
		int[] triangles = new int[numTris * 3 * 2];


        //Create vertices for circle
		int verticeIdx = 0;
		float angle = 0;

		for(int loopc = 0; loopc < numTris; angle += interval , loopc++){
			float x = Mathf.Sin(angle) * innerRadius;
			float z = Mathf.Cos (angle) * innerRadius;
			Vector3 vertice = new Vector3(x, 0, z);
			vertices[verticeIdx] = vertice;

			normals[verticeIdx] = Vector3.up;
			verticeIdx += 1;

			if(verticeIdx < numVerts){
				x = Mathf.Sin (angle + interval/2) * outerRadius;
				z = Mathf.Cos (angle + interval /2) * outerRadius;
				vertice = new Vector3(x, 0, z);
				vertices[verticeIdx] = vertice;
				normals[verticeIdx] = Vector3.up;
				verticeIdx+= 1;
			}

		}

        //Draw Triangles
		verticeIdx = 0;
		int outerTriangleVIndex = numVerts + 2;
		for(int i = 0; i < numVerts; i+=2, verticeIdx += 3){
			triangles[verticeIdx] = (i)%numVerts;
			triangles[verticeIdx + 1] = (i + 1)%numVerts;
			triangles[verticeIdx + 2] = (i + 2)%numVerts;
//
//			triangles[51] = 2;
//			triangles[52] = 1;
//			triangles[53] = 3;

//			triangles[outerTriangleVIndex] = (i + 1) % numVerts;
//			triangles[outerTriangleVIndex + 1] = (i + 3) % numVerts;
//			triangles[outerTriangleVIndex + 2] = (i + 2) % numVerts;
		}

		for( int i = 1; i < numVerts; i+=2, verticeIdx += 3){
			triangles[verticeIdx] = (i)%numVerts;
			triangles[verticeIdx + 1] = (i + 2)%numVerts;
			triangles[verticeIdx + 2] = (i + 3)%numVerts;
		}
		//ENDTEST


		// Create a new Mesh and populate with the data
		Mesh mesh = new Mesh();
		mesh.vertices = vertices;
		mesh.triangles = triangles;
		mesh.normals = normals;
		mesh.uv = uv;
		
		// Assign our mesh to our filter/renderer/collider
		MeshFilter mesh_filter = gameObject.GetComponent<MeshFilter>();
		MeshRenderer mesh_renderer = gameObject.GetComponent<MeshRenderer>(); ;
		MeshCollider mesh_collider = gameObject.GetComponent<MeshCollider>(); ;
		
		mesh_filter.mesh = mesh;
		mesh_collider.sharedMesh = mesh;
		Debug.Log("Done Mesh!");
		
	}

}
