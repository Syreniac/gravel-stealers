using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneRoot : MonoBehaviour
{

	public const int X_DIM = 25, Y_DIM = 25, Z_DIM = 5;

	public Block block;

	private Block[,,] blocks = new Block[X_DIM, Y_DIM, Z_DIM];

	// Use this for initialization
	void Start()
	{
		Color[] colors = { new Color(255, 0, 0), new Color(0, 255, 0), new Color(0, 0, 255), new Color(255, 255, 0), new Color(255, 0, 255), new Color(0, 255, 255), new Color(255, 255, 255), new Color(0, 0, 0) };
		int colorIndex = 0;
		// Create a XxYxZ block of cubes
		for (int x = 0; x < X_DIM; x++)
		{
			for (int y = 0; y < Y_DIM; y++)
			{
				blocks[x, y, 0] = Instantiate(block, new Vector3(x, y, 0), Quaternion.identity);
				blocks[x, y, 0].GetComponent<MeshRenderer>().material.color = colors[colorIndex++ % colors.Length];
					for (int z = 1; z < Z_DIM - 1; z++)
					{
						blocks[x, y, z] = Instantiate(block, new Vector3(x, y, z), Quaternion.identity);
						blocks[x, y, z].GetComponent<MeshRenderer>().material.color = colors[colorIndex++ % colors.Length];
						blocks[x, y, z].renderable = y == 13 || x == 13 || x == 0 || y == 0 || x == (SceneRoot.X_DIM - 1) || y == (SceneRoot.Y_DIM - 1);
					}
				blocks[x, y, Z_DIM - 1] = Instantiate(block, new Vector3(x, y, Z_DIM - 1), Quaternion.identity);
				blocks[x, y, Z_DIM - 1].GetComponent<MeshRenderer>().material.color = colors[colorIndex++ % colors.Length];
				colorIndex += x + y + 1;
			}
		}
	}

	public Block[,,] getBlocks()
	{
		return blocks;
	}
}
