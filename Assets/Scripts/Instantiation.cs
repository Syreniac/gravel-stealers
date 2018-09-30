using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Instantiation : MonoBehaviour {

	private const float CAMERA_SPEED_MULTI = 2.0f;

	private const float MIN_Y = 0.0f, MIN_X = 0.0f, MIN_Z = 0.0f;

	private const float MAX_Y = 5.0f, MAX_X = 5.0f, MAX_Z = 5.0f;

	private int blockX, blockY, blockZ;

	public Transform block;

	private GameObject[,,] blocks = new GameObject[5,5,5]; 
	
	void Start () {
		// Create a 5x5x5 block of cubes
		for (int x = 0; x < 5; x++)
		{
			for (int y = 0; y < 5; y++)
			{
				for (int z = 0; z < 5; z++)
				{
					Transform temp = Instantiate(block, new Vector3(x, y, z), Quaternion.identity);
					blocks[x, y, z] = temp.gameObject;
				}
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
			blockX = newBlockX;
			blockY = newBlockY;
			blockZ = newBlockZ;
			changeHiddenBlocks();
		}
		transform.position = position;

	}

	private void changeHiddenBlocks()
	{
		for (int x = 0; x < 5; x++)
		{
			for (int y = 0; y < 5; y++)
			{
				for (int z = 0; z < blockZ; z++)
				{
					blocks[x, y, z].GetComponent<MeshRenderer>().enabled = false;
				}
				for (int z = blockZ; z < 5; z++)
				{
					blocks[x, y, z].GetComponent<MeshRenderer>().enabled = true;
				}
			}
		}
	}
}
