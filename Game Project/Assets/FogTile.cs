using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]

public class FogTile : MonoBehaviour {
	public float position_x;
	public float position_y;
	public float position_z;

	void Start () {
		BuildMesh (100, 100, 1.0f);
	}

	void BuildMesh(int size_x, int size_z, float tileSize){
		int numTiles = size_x * size_z;
		int numTris = numTiles * 4;
		
		int vsize_x = size_x + 1;
		int vsize_z = size_z + 1;
		int numVerts = vsize_x * vsize_z;
		//numVerts += numTiles; // added vertices
		
		// Generate the mesh data
		Vector3[] vertices = new Vector3[numVerts];
		Vector3[] normals = new Vector3[numVerts];
		Vector2[] uv = new Vector2[numVerts];
		
		int[] triangles = new int[numTris * 3];
		
		int x, z;
        for (z = 0; z < vsize_z; z++)
        {
            for (x = 0; x < vsize_x; x++)
            {
                vertices[z * vsize_x + x] = new Vector3(x * tileSize, 0, z * tileSize);
                normals[z * vsize_x + x] = Vector3.up;
                uv[z * vsize_x + x] = new Vector2((float)x / vsize_x, (float)z / vsize_z);
            }
        }


		Debug.Log("Done Verts!");


        for (z = 0; z < size_z; z++)
        {
            for (x = 0; x < size_x; x++)
            {
                int squareIndex = z * size_x + x;
                int triOffset = squareIndex * 6 * 2;
                triangles[triOffset + 0] = z * vsize_x + x + 0;
                triangles[triOffset + 1] = z * vsize_x + x + vsize_x + 0;
                triangles[triOffset + 2] = z * vsize_x + x + vsize_x + 1;

                triangles[triOffset + 3] = z * vsize_x + x + 0;
                triangles[triOffset + 4] = z * vsize_x + x + vsize_x + 1;
                triangles[triOffset + 5] = z * vsize_x + x + 1;

                triangles[triOffset + 6] = z * vsize_x + x + vsize_x + 0;
                triangles[triOffset + 7] = z * vsize_x + x + vsize_x + 1;
                triangles[triOffset + 8] = z * vsize_x + x + 1;

                triangles[triOffset + 9] = z * vsize_x + x + vsize_x + 0;
                triangles[triOffset + 10] = z * vsize_x + x + 1;
                triangles[triOffset + 11] = z * vsize_x + x + 0;

            }
        }
		
		Debug.Log("Done Triangles!");
		
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
