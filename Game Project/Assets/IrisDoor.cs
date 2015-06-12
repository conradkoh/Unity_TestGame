using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]

public class IrisDoor : MonoBehaviour {
	public int _verticeCount = 30;
	public float _innerRadius = 0f;
	public float _outerRadius = 20.0f;
	public float doorSize = 20.0f;
	public bool renderOuters = true;
	public bool irisShape = false;
	
	private Mesh CoreMesh;
	private bool isFirstInit = true;
	private float currentPhase = 0;
	private float phaseVelocity = Mathf.PI * 2 / 1000;
	private bool isOpen = false;
	
	void Start () {
		BuildMesh (_verticeCount, _innerRadius, _outerRadius);
		
	}
	void Update(){
		BuildMesh(_verticeCount, _innerRadius, _outerRadius);
	}

	public void ToggleDoorStatus(){
		if(isOpen){
			isOpen = false;
			CloseDoor ();
		}
		else if(!isOpen){
			isOpen = true;
			OpenDoor ();
		}
	}
	#region DoorFunctions
	void OpenDoor(){
		_innerRadius = doorSize;
		_outerRadius = doorSize + 2;
	}

	void CloseDoor(){
		_innerRadius = 0;
		_outerRadius = doorSize + 2;
	}

	#endregion
	#region Distortions
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
	#endregion
	#region BuildMesh
	void BuildMesh(int numVerts, float innerRadius, float outerRadius){
		numVerts = (numVerts/2 * 2);
		
		//TEST
		//Declaration of parameters
		int numTris = ((numVerts / 2));
		float interval = (2 * Mathf.PI) / (numTris);
		
		
		Vector3[] vertices = new Vector3[numVerts];
		Vector3[] normals = new Vector3[numVerts];
		Vector2[] uv = new Vector2[numVerts];
		int[] triangles = new int[numTris * 3 * 2 * 2]; // 3 = number of vertices per triangle, 2 = inner + outer circle, 2 = front + back
		
		
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
		//Side 1
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

		//Side 2

		for(int i = 0; i < numVerts; i+=2, verticeIdx += 3){
			triangles[verticeIdx] = (i)%numVerts;
			triangles[verticeIdx + 1] = (i + 2)%numVerts;
			triangles[verticeIdx + 2] = (i + 1)%numVerts;
		}
		if(renderOuters && !irisShape){
			for( int i = 1; i < numVerts; i+=2, verticeIdx += 3){
				triangles[verticeIdx] = (i)%numVerts;
				triangles[verticeIdx + 1] = (i + 1)%numVerts;
				triangles[verticeIdx + 2] = (i + 2)%numVerts;
			}
		}
		else if(renderOuters && irisShape){
			for( int i = 1; i < numVerts; i+=2, verticeIdx += 3){
				triangles[verticeIdx] = (i)%numVerts;
				triangles[verticeIdx + 1] = (i + 3)%numVerts;
				triangles[verticeIdx + 2] = (i + 2)%numVerts;
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
	#endregion
}
