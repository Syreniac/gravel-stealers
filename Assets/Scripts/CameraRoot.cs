using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

public class CameraRoot : MonoBehaviour
{
	private enum CameraBehavior
	{
		RTS
	}

	private const float CAMERA_SPEED_MULTI = 2.0f;

	private const float MIN_Y = 0.0f, MIN_X = 0.0f, MIN_Z = 0.0f;

	private const float MAX_Y = SceneRoot.Y_DIM, MAX_X = SceneRoot.X_DIM, MAX_Z = SceneRoot.Z_DIM - 1.1f;

	private static int XDOWN_BITFLAG = BitVector32.CreateMask();

	private static int XUP_BITFLAG = BitVector32.CreateMask(XDOWN_BITFLAG);

	private static int YDOWN_BITFLAG = BitVector32.CreateMask(XUP_BITFLAG);

	private static int YUP_BITFLAG = BitVector32.CreateMask(YDOWN_BITFLAG);

	private static int ZDOWN_BITFLAG = BitVector32.CreateMask(YUP_BITFLAG);

	private static int ZUP_BITFLAG = BitVector32.CreateMask(ZDOWN_BITFLAG);

	private int blockX = 0, blockY = 0, blockZ = 0;

	private int checkFlag = 1;

	private CameraBehavior cameraBehavior = CameraBehavior.RTS;

	private SceneRoot parent;
	
	void Start () {
		parent = transform.GetComponentInParent<SceneRoot>();
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
			checkFlag = 1 + checkFlag % 1000;
			floodVisibilityFrom(blockX, blockY, blockZ);
		}
		switch (cameraBehavior)
		{
			case CameraBehavior.RTS:
				toggleVisibilityRTS();
				break;
		}
	}

	private void toggleVisibilityRTS()
	{
		for (int x = 0; x < SceneRoot.X_DIM; x++)
		{
			for (int y = 0; y < SceneRoot.Y_DIM; y++)
			{
				for (int z = 0; z < SceneRoot.Z_DIM; z++)
				{
					// TODO: Change this to be based on the direction of the camera rather 
					// than a fixed value
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

	private void floodVisibilityFrom(int x, int y, int z)
	{
		Block[,,] blocks = parent.getBlocks();
		int currentX = x, currentY = y, currentZ = z;

		blocks[x, y, z].checkedFrom = GridDirectionExtension.NONE;
		
		while (true)
		{
			Block block = blocks[currentX, currentY, currentZ];
			if (block.checkFlag != checkFlag)
			{
				// We haven't checked that block this cycle
				// So we need to set up the bit flags for tiles we can check
				for (GridDirection gd = GridDirectionExtension.LOOP_START; gd != GridDirectionExtension.LOOP_END; gd = gd.next())
				{
					block.setCheckDirectionBit(gd, gd.validate(currentX, currentY, currentZ));
				}
				block.checkFlag = checkFlag;
			}
			if (!block.renderable && block.getCheckDirectionBit(GridDirectionExtension.NONE))
			{

				// Try to find the first block we haven't already checked that's also in the array
				// The control flow here is a bit weird I'll admit
				GridDirection gd = GridDirectionExtension.LOOP_START;
				for (; gd != GridDirectionExtension.LOOP_END; gd = gd.next())
				{
					if (block.getCheckDirectionBit(gd))
					{
						block.setCheckDirectionBit(gd, false);
						if (gd.inspectForwards(blocks, currentX, currentY, currentZ).checkFlag != checkFlag)
						{
							gd.navigateForwards(ref currentX, ref currentY, ref currentZ);
							blocks[currentX, currentY, currentZ].checkedFrom = gd;
							break;
						}
					}
				}
				// If we found a direction to move, then skip the checks and go to the next cycle
				if (gd != GridDirectionExtension.LOOP_END)
				{
					continue;
				}
			}

			// Either of these mean we've reached our original starting location, with nowhere else we can check.
			if (block.checkedFrom == GridDirectionExtension.NONE || (x == currentX && y == currentY && currentZ == z))
			{
				return;
			}
			else
			{
				// Alternatively, we just go back to our source block and continue from there
				block.checkedFrom.navigateBackwards(ref currentX, ref currentY, ref currentZ);
			}
		}
		
		
	}
}
