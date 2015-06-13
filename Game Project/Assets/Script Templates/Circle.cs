using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]

public class Circle : MonoBehaviour {
	public int _verticeCount = 30;
	public float _innerRadius = 5f;
	public float _outerRadius = 7f;
	public float height = 15;
	public bool renderOuters = true;
	public bool irisShape = false;
	public Vector3 position = new Vector3();
	public Player player;

	private Mesh CoreMesh;
	private bool isFirstInit = true;
	private float currentPhase = 0;
	private float phaseVelocity = Mathf.PI * 2 / 1000;
	
	void Start () {
		BuildMesh (_verticeCount, _innerRadius, _outerRadius);

	}
	void Update(){
		BuildMesh(_verticeCount, _innerRadius, _outerRadius);
		TranslateMesh();
		//DistortMesh();
	}

	void TranslateMesh(){
		float x = player.transform.position.x;
		float y = player.transform.position.y + height;
		float z = player.transform.position.z;
		Vector3 newPosition = new Vector3(x, y, z);
		this.transform.position = newPosition;
	}
	
	void DistortMesh(){
		Mesh mesh = GetComponent<MeshFilter>().mesh;
		float pertubation_x = Mathf.Sin(currentPhase);
		float pertubation_z = Mathf.Sin((currentPhase + 0.04f) / 2);
//		Debug.Log ("x pertubation: " + pertubation_x);
//		Debug.Log("z pertubation: " + pertubation_z);
		Vector3[] newVerts = new Vector3[_verticeCount];
		Vector3[] currentVerts = CoreMesh.vertices;

		for(int i = 0; i < _verticeCount; ++i ){
			Vector3 vertex = currentVerts[i];
			float x = vertex.x;
			float y = vertex.y;
			float z = vertex.z;
//			x += 0.05f *pertubation_x;
//			z += 0.05f * pertubation_z;
			y += 0.5f * pertubation_x;

			Vector3 newVert = new Vector3(x, y, z);
			newVerts[i] = newVert;
			currentPhase += phaseVelocity;
		}

		//Update
		mesh.vertices = newVerts;

		currentPhase = currentPhase % (Mathf.PI * 2);
//		Debug.Log ("Phase: " + currentPhase);
	}

	void BuildMesh(int numVerts, float innerRadius, float outerRadius){
		numVerts = (numVerts/2 * 2);
		
		//TEST
		//Declaration of parameters
		int numTris = ((numVerts / 2));
		float interval = (2 * Mathf.PI) / (numTris);
		
		
		Vector3[] vertices = new Vector3[numVerts];
		Vector3[] normals = new Vector3[numVerts];
		Vector2[] uv = new Vector2[numVerts];
		int[] triangles = new int[numTris * 3 * 2];
		
		
		//Create vertices/normals/ for circle
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
		//int outerTriangleVIndex = numVerts + 2;
		for(int i = 0; i < numVerts; i+=2, verticeIdx += 3){
			triangles[verticeIdx] = (i)%numVerts;
			triangles[verticeIdx + 1] = (i + 1)%numVerts;
			triangles[verticeIdx + 2] = (i + 2)%numVerts;
		}
		if(renderOuters && !irisShape){
			for( int i = 1; i < numVerts; i+=2, verticeIdx += 3){
				triangles[verticeIdx] = (i)%numVerts;
				triangles[verticeIdx + 1] = (i + 2)%numVerts;
				triangles[verticeIdx + 2] = (i + 1)%numVerts;
			}
		}
		else if(renderOuters && irisShape){
			for( int i = 1; i < numVerts; i+=2, verticeIdx += 3){
				triangles[verticeIdx] = (i)%numVerts;
				triangles[verticeIdx + 1] = (i + 2)%numVerts;
				triangles[verticeIdx + 2] = (i + 3)%numVerts;
			}
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

		if(isFirstInit){
			CoreMesh = new Mesh();
			CoreMesh.vertices = vertices;
			CoreMesh.triangles = triangles;
			CoreMesh.normals = normals;
			CoreMesh.uv = uv;

			isFirstInit = false;
		}

		
	}
}
