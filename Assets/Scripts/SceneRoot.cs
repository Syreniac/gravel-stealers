using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneRoot : MonoBehaviour
{

	public const int X_DIM = 26, Y_DIM = 26, Z_DIM = 8;

	public Block block;

	private Block[,,] blocks = new Block[X_DIM, Y_DIM, Z_DIM];

	// Use this for initialization
	void Start()
	{
		Color[] colors = { new Color(79f/255f, 58f/255f, 56f/255f), new Color(137f/255f, 127f/255f, 117f/255f), new Color(48f/255f, 28f/255f, 30f/255f), new Color(121f/255f, 72f/255f, 51f/255f), new Color(165f/255f, 161f/255f, 152f/255f)};
		int colorIndex = 0;
        // Create a XxYxZ block of cubes
        int bumpToogle = 0;
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
                    blocks[x, y, z].renderable = y == 13 || x == 13 || x == 0 || y == 0 || x == (SceneRoot.X_DIM - 1) || y == (SceneRoot.Y_DIM - 1) || (z == (Z_DIM - 2) && (bumpToogle % 7) == 0);
                }
                blocks[x, y, Z_DIM - 1] = Instantiate(block, new Vector3(x, y, Z_DIM - 1), Quaternion.identity);
				blocks[x, y, Z_DIM - 1].GetComponent<MeshRenderer>().material.color = colors[colorIndex++ % colors.Length];
                colorIndex += 10;
                bumpToogle++;
            }
		}
	}

	public Block[,,] getBlocks()
	{
		return blocks;
	}
}
