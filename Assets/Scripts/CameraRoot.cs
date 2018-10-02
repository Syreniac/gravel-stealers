using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

public class CameraRoot : MonoBehaviour {

	private enum CameraBehavior
	{
		RTS
	}

	private const float CAMERA_SPEED_MULTI = 2.0f;

	private const float MIN_Y = 0.0f, MIN_X = 0.0f, MIN_Z = 0.0f;

	private const float MAX_Y = SceneRoot.Y_DIM, MAX_X = SceneRoot.X_DIM, MAX_Z = SceneRoot.Z_DIM - 0.1f;

	private static int XDOWN_BITFLAG = BitVector32.CreateMask();

	private static int XUP_BITFLAG = BitVector32.CreateMask(XDOWN_BITFLAG);

	private static int YDOWN_BITFLAG = BitVector32.CreateMask(XUP_BITFLAG);

	private static int YUP_BITFLAG = BitVector32.CreateMask(YDOWN_BITFLAG);

	private static int ZDOWN_BITFLAG = BitVector32.CreateMask(YUP_BITFLAG);

	private static int ZUP_BITFLAG = BitVector32.CreateMask(ZDOWN_BITFLAG);

	private int blockX = 0, blockY = 0, blockZ = 0;

	private int checkFlag = 1;

	private CameraBehavior cameraBehavior;

	private SceneRoot parent;
	
	void Start () {
		parent = transform.GetComponentInParent<SceneRoot>();
		blockX = 0;
		blockY = 0;
		blockZ = 0;
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
		if (newBlockX != blockX || newBlockY != blockY || newBlockZ != blockZ)
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
		// If we're hovering over a block that isn't renderable and the checkflag isn't
		// part of the last run, then we need to do a new run to pick it and any
		// associated empty spaces.
		if ((parent.getBlocks()[blockX, blockY, blockZ].checkFlag != checkFlag && 
			!parent.getBlocks()[blockX, blockY, blockZ].renderable))
		{
			// Otherwise we can return early for speed (as there is no need to do a full run here)
			Debug.Log("Increasing check count");
			checkFlag = 1 + checkFlag % 1000;
			int totalSteps = floodVisibilityFrom(blockX, blockY, blockZ);
		}
		for (int x = 0; x < SceneRoot.X_DIM; x++)
		{
			for (int y = 0; y < SceneRoot.Y_DIM; y++)
			{
				for (int z = 0; z < SceneRoot.Z_DIM; z++)
				{
					if (parent.getBlocks()[x, y, z].checkFlag == checkFlag && z >= blockZ)
					{
						parent.getBlocks()[x, y, z].visible = Block.Visibility.VISIBLE;
					}
					else
					{
						parent.getBlocks()[x, y, z].visible = Block.Visibility.HIDDEN;
					}
				}
			}
		}
	}

	private int floodVisibilityFrom(int x, int y, int z)
	{
		Block[,,] blocks = parent.getBlocks();
		int currentX = x, currentY = y, currentZ = z;

		blocks[x, y, z].checkedFrom = 0;

		int stepCount = 0;

		while(true)
		{
			stepCount++;
			Block block = blocks[currentX, currentY, currentZ];
			if (block.checkFlag != checkFlag)
			{
				// We haven't checked that block this cycle
				// So we need to set up the bit flags for tiles we can check
				block.setCheckDirectionBit(XDOWN_BITFLAG, currentX > 0);
				block.setCheckDirectionBit(XUP_BITFLAG, currentX < (SceneRoot.X_DIM - 1));
				block.setCheckDirectionBit(YDOWN_BITFLAG, currentY > 0);
				block.setCheckDirectionBit(YUP_BITFLAG, currentY < (SceneRoot.Y_DIM - 1));
				block.setCheckDirectionBit(ZDOWN_BITFLAG, currentZ > 0);
				block.setCheckDirectionBit(ZUP_BITFLAG, currentZ < (SceneRoot.Z_DIM - 1));
				block.checkFlag = checkFlag;
			}
			if (block.renderable)
			{
				// If this block is renderable, then we can continue
				// because this block will block its neighbours
				switch (block.checkedFrom)
				{
					// The checkedFrom flag indicates the block we came
					// from in this flood search, so we can just reverse it
					case 1:
						currentX += 1;
						break;
					case 2:
						currentX -= 1;
						break;
					case 3:
						currentY += 1;
						break;
					case 4:
						currentY -= 1;
						break;
					case 5:
						currentZ += 1;
						break;
					case 6:
						currentZ -= 1;
						break;
					default:
						return stepCount;
				}
				continue;
			}
			
			// Try to find the first block we haven't already checked that's also in the array
			// The control flow here is a bit weird I'll admit
			if (block.getCheckDirectionBit(XDOWN_BITFLAG))
			{
				block.setCheckDirectionBit(XDOWN_BITFLAG, false);
				if (blocks[currentX - 1, currentY, currentZ].checkFlag != checkFlag)
				{
					currentX -= 1;
					blocks[currentX, currentY, currentZ].checkedFrom = 1;
					continue;
				}
			}
			if (block.getCheckDirectionBit(XUP_BITFLAG))
			{
				block.setCheckDirectionBit(XUP_BITFLAG, false);
				if (blocks[currentX + 1, currentY, currentZ].checkFlag != checkFlag)
				{
					currentX += 1;
					blocks[currentX, currentY, currentZ].checkedFrom = 2;
					continue;
				}
			}
			if (block.getCheckDirectionBit(YDOWN_BITFLAG))
			{
				block.setCheckDirectionBit(YDOWN_BITFLAG, false);
				if (blocks[currentX, currentY - 1, currentZ].checkFlag != checkFlag)
				{
					currentY -= 1;
					blocks[currentX, currentY, currentZ].checkedFrom = 3;
					continue;
				}
			}
			if (block.getCheckDirectionBit(YUP_BITFLAG))
			{
				block.setCheckDirectionBit(YUP_BITFLAG, false);
				if (blocks[currentX, currentY + 1, currentZ].checkFlag != checkFlag)
				{
					currentY += 1;
					blocks[currentX, currentY, currentZ].checkedFrom = 4;
					continue;
				}
			}
			if (block.getCheckDirectionBit(ZDOWN_BITFLAG))
			{
				block.setCheckDirectionBit(ZDOWN_BITFLAG, false);
				if (blocks[currentX, currentY, currentZ - 1].checkFlag != checkFlag)
				{
					currentZ -= 1;
					blocks[currentX, currentY, currentZ].checkedFrom = 5;
					continue;
				}
			}
			if (block.getCheckDirectionBit(ZUP_BITFLAG))
			{
				block.setCheckDirectionBit(ZUP_BITFLAG, false);
				if (blocks[currentX, currentY, currentZ + 1].checkFlag != checkFlag)
				{
					currentZ += 1;
					blocks[currentX, currentY, currentZ].checkedFrom = 6;
					continue;
				}
			}

			// Else we can go back to where we came from!
			switch (block.checkedFrom)
			{
				case 1:
					currentX += 1;
					break;
				case 2:
					currentX -= 1;
					break;
				case 3:
					currentY += 1;
					break;
				case 4:
					currentY -= 1;
					break;
				case 5:
					currentZ += 1;
					break;
				case 6:
					currentZ -= 1;
					break;
				default:
					// If we have no direction, we started the run from here.
					return stepCount;
			}

		}
		
	}
}
