using UnityEngine;
using System.Collections;

public class TileMap : MonoBehaviour {
	public GameObject baseTile;
	public int rows = 1;
	public int cols = 1;
	public int tileSize = 10;
	// Use this for initialization
	void Start () {
		float scale = tileSize / 10;
		for (int i = 0; i < rows; i++){
			for(int j = 0; j < cols; j++){
				//Instantiate(baseTile, new Vector3(i* tileSize , 0, j * tileSize), Quaternion.identity);
				Instantiate(baseTile, new Vector3((i * tileSize) - (tileSize * (rows - 1)/ 2), 0 , (j * tileSize) - (tileSize * (cols - 1) / 2)), Quaternion.identity);
			}

		}
		//Instantiate(baseTile, new Vector3(5, 0, 5), Quaternion.identity);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
