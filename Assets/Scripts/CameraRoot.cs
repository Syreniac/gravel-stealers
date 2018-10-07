using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

public class CameraRoot : MonoBehaviour
{
    private enum CameraBehavior
    {
        RTS_1
    }

    private const float CAMERA_MOVE_SPEED_MULTI = 4.0f;

    private const float CAMERA_ROTATION_SPEED_MULTI = 45f;

    private const float MIN_Y = 0.1f, MIN_X = 0.1f, MIN_Z = 0.1f;

    private const float MAX_Y = SceneRoot.Y_DIM - 0.1f, MAX_X = SceneRoot.X_DIM - 0.1f, MAX_Z = SceneRoot.Z_DIM - 1.1f;

    private int blockX = 0, blockY = 0, blockZ = 0;

    private int checkFlag = 1;

    private CameraBehavior cameraBehavior = CameraBehavior.RTS_1;

    private SceneRoot parent;

    public Camera camera;

    void Start() {
        parent = transform.GetComponentInParent<SceneRoot>();
        //changeHiddenBlocks();
        camera.transform.LookAt(transform.position, Vector3.back);
    }

    void Update()
    {
        Vector3 position = transform.position;
        if (Input.GetButton("Down"))
        {
            position.x += Time.deltaTime * CAMERA_MOVE_SPEED_MULTI * (Mathf.Sin(Mathf.Deg2Rad * (-transform.rotation.eulerAngles.z)));
            position.y += Time.deltaTime * CAMERA_MOVE_SPEED_MULTI * (Mathf.Cos(Mathf.Deg2Rad * (-transform.rotation.eulerAngles.z)));
        }
        else if (Input.GetButton("Up"))
        {
            position.x -= Time.deltaTime * CAMERA_MOVE_SPEED_MULTI * (Mathf.Sin(Mathf.Deg2Rad * (-transform.rotation.eulerAngles.z)));
            position.y -= Time.deltaTime * CAMERA_MOVE_SPEED_MULTI * (Mathf.Cos(Mathf.Deg2Rad * (-transform.rotation.eulerAngles.z)));
        }

        if (Input.GetButton("Left"))
        {
            position.x += Time.deltaTime * CAMERA_MOVE_SPEED_MULTI * (Mathf.Cos(Mathf.Deg2Rad * (transform.rotation.eulerAngles.z)));
            position.y += Time.deltaTime * CAMERA_MOVE_SPEED_MULTI * (Mathf.Sin(Mathf.Deg2Rad * (transform.rotation.eulerAngles.z)));
        }
        else if (Input.GetButton("Right"))
        {
            position.x -= Time.deltaTime * CAMERA_MOVE_SPEED_MULTI * (Mathf.Cos(Mathf.Deg2Rad * (transform.rotation.eulerAngles.z)));
            position.y -= Time.deltaTime * CAMERA_MOVE_SPEED_MULTI * (Mathf.Sin(Mathf.Deg2Rad * (transform.rotation.eulerAngles.z)));
        }
        if (Input.GetButton("In"))
        {
            position.z -= Time.deltaTime * CAMERA_MOVE_SPEED_MULTI;
        }
        else if (Input.GetButton("Out"))
        {
            position.z += Time.deltaTime * CAMERA_MOVE_SPEED_MULTI;
        }
        if (position.x < MIN_X)
        {
            position.x = MIN_X;
        }
        else if (position.x > MAX_X)
        {
            position.x = MAX_X;
        }
        if (position.y < MIN_Y)
        {
            position.y = MIN_Y;
        }
        else if (position.y > MAX_Y)
        {
            position.y = MAX_Y;
        }
        if (position.z < MIN_Z)
        {
            position.z = MIN_Z;
        }
        else if (position.z > MAX_Z)
        {
            position.z = MAX_Z;
        }

        adjustCameraAngle();
        int newBlockX = (int)transform.position.x;
        int newBlockY = (int)transform.position.y;
        int newBlockZ = (int)transform.position.z;
        if (newBlockX != blockX || newBlockY != blockY || newBlockZ != blockZ)
        {
            blockX = newBlockX;
            blockY = newBlockY;
            blockZ = newBlockZ;
            changeHiddenBlocks();
        }
        transform.position = position;

    }

    private void adjustCameraAngle()
    {

        // Rotate Right Click
        if (Input.GetMouseButton(1))
        {// fetch the users mouse motion for this frame
            float x = Input.GetAxis("Mouse X") * -3f;
            // now adjust cameras rotation around target player based on the mouse x,y movement
            transform.RotateAround(transform.position, Vector3.back, x);
        }
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
            case CameraBehavior.RTS_1:
                toggleVisibilityRTS_1();
                break;
        }
    }
    private void toggleVisibilityRTS_1()
	{
        int startingX = 0;
        int startingY = 0;

        startingX = Math.Max(0, Math.Min(startingX, SceneRoot.X_DIM - 2));
        startingY = Math.Max(0, Math.Min(startingY, SceneRoot.Y_DIM - 2));
        

		for (int x = 0; x < SceneRoot.X_DIM; x++)
		{
			for (int y = 0; y < SceneRoot.Y_DIM; y++)
            {
                for (int z = 0; z < SceneRoot.Z_DIM; z++)
				{
                    if (!(x < (startingX) || y < (startingY)) && parent.getBlocks()[x, y, z].checkFlag == checkFlag && z >= blockZ)
					{
						parent.getBlocks()[x, y, z].visible = Block.Visibility.VISIBLE;
					}
                    else if(!(x < (startingX) || y < (startingY)) && parent.getBlocks()[x,y,z].checkFlag != checkFlag && z == blockZ)
                    {
                        parent.getBlocks()[x, y, z].visible = Block.Visibility.BLOCKED;
                    }
					else
					{
						parent.getBlocks()[x, y, z].visible = Block.Visibility.HIDDEN;
					}
				}
			}
        }

        for (int y = startingY; y < SceneRoot.Y_DIM; y++)
        {
            int lowestZ = blockZ;
            for (int z = blockZ; z < SceneRoot.Z_DIM; z++)
            {
                int x = startingX;
                for(; x < SceneRoot.X_DIM; x++)
                {
                    if(parent.getBlocks()[x,y,z].renderable && parent.getBlocks()[x, y, z].checkFlag == checkFlag)
                    {
                        lowestZ = Math.Max(lowestZ, z);
                        break;
                    }
                }
                if(x < SceneRoot.X_DIM)
                {
                    for (; x >= startingX; x--)
                    {
                        parent.getBlocks()[x, y, z].visible = Block.Visibility.HIDDEN;
                    }
                }
            }
            for (int z = lowestZ; z >= blockZ; z--)
            {
                bool thisBlockChecked = parent.getBlocks()[startingX, y, z].checkFlag == checkFlag;
                bool neighbour1BlockBlocked = parent.getBlocks()[startingX + 1, y, z].visible == Block.Visibility.BLOCKED;
                bool neighbour1BlockRendered = parent.getBlocks()[startingX + 1, y, z].visible == Block.Visibility.VISIBLE && parent.getBlocks()[startingX+1, y, z].renderable;
                
                if ((thisBlockChecked && neighbour1BlockRendered) || (neighbour1BlockBlocked))
                {
                    
                    parent.getBlocks()[startingX, y, z].visible = Block.Visibility.BLOCKED;
                }
            }
        }


        for (int x = startingX; x < SceneRoot.X_DIM; x++)
        {
            int lowestZ = blockZ;
            for (int z = blockZ; z < SceneRoot.Z_DIM; z++)
            {
                int y = startingY;
                for (; y < SceneRoot.Y_DIM; y++)
                {
                    if (parent.getBlocks()[x, y, z].renderable && parent.getBlocks()[x,y,z].checkFlag == checkFlag)
                    {
                        lowestZ = Math.Max(lowestZ, z);
                        break;
                    }
                }
                if (y < SceneRoot.Y_DIM)
                {
                    for (; y >= startingY; y--)
                    {
                        parent.getBlocks()[x, y, z].visible = Block.Visibility.HIDDEN;
                    }
                }
            }

            for (int z = lowestZ; z >= blockZ; z--)
            {
                bool thisBlockChecked = parent.getBlocks()[x, startingY, z].checkFlag == checkFlag;
                bool neighbour1BlockBlocked = parent.getBlocks()[x, startingY + 1, z].visible == Block.Visibility.BLOCKED;
                bool neighbour1BlockRendered = parent.getBlocks()[x, startingY + 1, z].visible == Block.Visibility.VISIBLE && parent.getBlocks()[x, startingY + 1, z].renderable;

                if ((thisBlockChecked && neighbour1BlockRendered) || (neighbour1BlockBlocked))
                {
                        
                    parent.getBlocks()[x, startingY, z].visible = Block.Visibility.BLOCKED;
                }
            }
        }

    }

    private void floodVisibilityFrom(int x, int y, int z)
	{
		Block[,,] blocks = parent.getBlocks();
		int currentX = x, currentY = y, currentZ = z;

		blocks[x, y, z].checkedFrom = (ulong) GridDirectionExtension.NONE;
        

		while (true)
		{
			Block block = blocks[currentX, currentY, currentZ];
			if (block.checkFlag != checkFlag)
			{
				// We haven't checked that block this cycle
				// So we need to set up the bit flags for tiles we can check
				for (GridDirection gd = GridDirectionExtension.LOOP_START; gd != GridDirectionExtension.LOOP_END; gd = gd.next())
				{
                    bool b = gd.validate(currentX, currentY, currentZ);
					block.setCheckDirectionBit(gd, gd.validate(currentX, currentY, currentZ));
                }
				block.checkFlag = checkFlag;
			}
			if (!block.renderable && block.checkDirection != 0)
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
							blocks[currentX, currentY, currentZ].checkedFrom = (ulong) gd;
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
			if (block.checkedFrom == (ulong) GridDirectionExtension.NONE || (x == currentX && y == currentY && currentZ == z))
			{
				return;
			}
			else
			{
				// Alternatively, we just go back to our source block and continue from there
				((GridDirection) block.checkedFrom).navigateBackwards(ref currentX, ref currentY, ref currentZ);
			}
		}
		
		
	}
}
