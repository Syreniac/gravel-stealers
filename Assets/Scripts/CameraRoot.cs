using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRoot : MonoBehaviour {

	private enum CameraBehavior
	{
		RTS,
		SIDE_N,
		SIDE_E,
		SIDE_S,
		SIDE_W
	}

	private const float CAMERA_SPEED_MULTI = 2.0f;

	private const int X_DIM = 25, Y_DIM = 25, Z_DIM = 5;

	private const float MIN_Y = 0.0f, MIN_X = 0.0f, MIN_Z = 0.0f;

	private const float MAX_Y = Y_DIM, MAX_X = X_DIM, MAX_Z = Z_DIM - 0.1f;

	private int blockX, blockY, blockZ;

	private CameraBehavior cameraBehavior;

	public GameObject block;

	private GameObject[,,] blocks = new GameObject[X_DIM,Y_DIM,Z_DIM];
	

	void Start () {
		// Create a XxYxZ block of cubes
		for (int x = 0; x < X_DIM; x++)
		{
			for (int y = 0; y < Y_DIM; y++)
			{
				blocks[x, y, 0] = Instantiate(block, new Vector3(x, y, 0), Quaternion.identity);
				if ((x == X_DIM - 1 || x == 0) || (y == Y_DIM - 1 || y == 0))
				{
					for (int z = 1; z < Z_DIM - 1; z++)
					{
						blocks[x, y, z] = Instantiate(block, new Vector3(x, y, z), Quaternion.identity);
					}
				}
				blocks[x, y, Z_DIM - 1] = Instantiate(block, new Vector3(x, y, Z_DIM - 1), Quaternion.identity);
			}
		}
		
		blockX = 0;
		blockY = 0;
		blockZ = 0;
		transform.position = new Vector3(0, 0, 0);
		changeHiddenBlocks();
	}

	void Update()
	{
		Vector3 position = transform.position;
		if (Input.GetButton("Down"))
		{
			position.y -= Time.deltaTime * CAMERA_SPEED_MULTI;
			if (position.y < MIN_Y)
			{
				position.y = MIN_Y;
			}
		}
		else if (Input.GetButton("Up"))
		{
			position.y += Time.deltaTime * CAMERA_SPEED_MULTI;
			if (position.y > MAX_Y)
			{
				position.y = MAX_Y;
			}
		}

		if (Input.GetButton("Left"))
		{
			position.x -= Time.deltaTime * CAMERA_SPEED_MULTI;
			if (position.x < MIN_X)
			{
				position.x = MIN_X;
			}
		}
		else if (Input.GetButton("Right"))
		{
			position.x += Time.deltaTime * CAMERA_SPEED_MULTI;
			if (position.x > MAX_X)
			{
				position.x = MAX_X;
			}
		}
		if (Input.GetButton("In"))
		{
			position.z -= Time.deltaTime * CAMERA_SPEED_MULTI;
			if (position.z < MIN_Z)
			{
				position.z = MIN_Z;
			}
		}
		else if (Input.GetButton("Out"))
		{
			position.z += Time.deltaTime * CAMERA_SPEED_MULTI;
			if (position.z > MAX_Z)
			{
				position.z = MAX_Z;
			}
		}
		int newBlockX = (int) position.x;
		int newBlockY = (int) position.y;
		int newBlockZ = (int) position.z;
		if (newBlockY != blockY || newBlockX != blockX || newBlockZ != blockZ)
		{
			Debug.Log(newBlockX+", "+newBlockY+", "+newBlockZ);
			blockX = newBlockX;
			blockY = newBlockY;
			blockZ = newBlockZ;
			changeHiddenBlocks();
		}
		transform.position = position;

	}

	private void changeHiddenBlocks()
	{
		switch (cameraBehavior)
		{
			case CameraBehavior.RTS:
				for (int x = 0; x < X_DIM; x++)
				{
					for (int y = 0; y < Y_DIM; y++)
					{
						if (blocks[x, y, 0] != null) {
							blocks[x, y, 0].GetComponent<MeshRenderer>().enabled = (0 >= blockZ);
						}
						for (int z = 1; z < Z_DIM; z++)
						{
							if (blocks[x, y, z] != null)
							{
								blocks[x, y, z].GetComponent<MeshRenderer>().enabled = (z >= blockZ) || !(blocks[x, y, z - 1] == null || !blocks[x,y,z - 1].GetComponent<MeshRenderer>().enabled) ;
							}
						}
					}
				}
				break;
			case CameraBehavior.SIDE_N:

				break;
		}
	}
}
