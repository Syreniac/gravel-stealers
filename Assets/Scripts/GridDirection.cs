using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public enum GridDirection : ulong
{
	// Deliberately specified so that we can use them as bit set indexes!
	X_DOWN = 1, // (-1,0,0)
	X_UP = 2, // (1,0,0)
	Y_DOWN = 4, // (0,-1,0);
	Y_UP = 8, // (0,1,0);
	Z_DOWN = 16, // (0,0,-1)
	Z_UP = 32, // (0,0,1)

    X_DOWN_Y_DOWN = 64, // (-1,-1,0)
    X_DOWN_Y_UP = 128, // (-1,1,0)
    X_UP_Y_DOWN = 256, // (1,-1,0)
    X_UP_Y_UP = 512, // (1,1,0)

    X_DOWN_Z_DOWN = 1024, // (-1,0,-1)
    X_DOWN_Z_UP = 2048, // (-1,0,1)
    X_UP_Z_DOWN = 4096, // (1,0,-1)
    X_UP_Z_UP = 8192, // (1,0,1)

    Y_DOWN_Z_DOWN = 16384, // (0,-1,-1)
    Y_DOWN_Z_UP = 32768, // (0,-1,1)
    Y_UP_Z_DOWN = 65536, // (0,1,-1)
    Y_UP_Z_UP = 131072, // (0,1,1)

    X_DOWN_Y_DOWN_Z_DOWN = 262144, // (-1,-1,-1)
    X_DOWN_Y_DOWN_Z_UP = 524288, // (-1,-1,1)
    X_DOWN_Y_UP_Z_DOWN = 1048576, // (-1, 1, -1)
    X_DOWN_Y_UP_Z_UP = 2097152, // (-1, 1, 1)
    X_UP_Y_DOWN_Z_DOWN = 4194304, // (1, -1, -1)
    X_UP_Y_DOWN_Z_UP = 8388608, // (1, -1, 1)
    X_UP_Y_UP_Z_DOWN = 16777216, // (1, 1, -1)
    X_UP_Y_UP_Z_UP = 33554432 // (1,1,1)
}

public static class GridDirectionExtension
{
	// Enum-like entries to help abstract away some of the maths
	// Don't hate me, this is all compiler legal

	public static readonly GridDirection NONE = 0;

	public static readonly GridDirection LOOP_START = GridDirection.X_DOWN;

	public static readonly GridDirection LOOP_END = (GridDirection) ( (ulong) GridDirection.X_UP_Y_UP_Z_UP * 2);

	/**
	 * Adjusts x/y/z as we moved in that direction.
	 */
	public static void navigateForwards(this GridDirection gridDirection, ref int x, ref int y, ref int z)
	{
		switch (gridDirection)
		{
			case GridDirection.X_DOWN:

            case GridDirection.X_DOWN_Y_DOWN:
            case GridDirection.X_DOWN_Y_UP:
            case GridDirection.X_DOWN_Z_DOWN:
            case GridDirection.X_DOWN_Z_UP:

            case GridDirection.X_DOWN_Y_DOWN_Z_DOWN:
            case GridDirection.X_DOWN_Y_DOWN_Z_UP:
            case GridDirection.X_DOWN_Y_UP_Z_DOWN:
            case GridDirection.X_DOWN_Y_UP_Z_UP:
                x -= 1;
                break;
            case GridDirection.X_UP:

            case GridDirection.X_UP_Y_DOWN:
            case GridDirection.X_UP_Y_UP:
            case GridDirection.X_UP_Z_DOWN:
            case GridDirection.X_UP_Z_UP:

            case GridDirection.X_UP_Y_DOWN_Z_DOWN:
            case GridDirection.X_UP_Y_DOWN_Z_UP:
            case GridDirection.X_UP_Y_UP_Z_DOWN:
            case GridDirection.X_UP_Y_UP_Z_UP:
                x += 1;
				break;
		}
        switch (gridDirection)
        {

            case GridDirection.Y_DOWN:

            case GridDirection.X_DOWN_Y_DOWN:
            case GridDirection.X_UP_Y_DOWN:
            case GridDirection.Y_DOWN_Z_DOWN:
            case GridDirection.Y_DOWN_Z_UP:

            case GridDirection.X_DOWN_Y_DOWN_Z_DOWN:
            case GridDirection.X_DOWN_Y_DOWN_Z_UP:
            case GridDirection.X_UP_Y_DOWN_Z_DOWN:
            case GridDirection.X_UP_Y_DOWN_Z_UP:
                y -= 1;
                break;
            case GridDirection.Y_UP:

            case GridDirection.X_DOWN_Y_UP:
            case GridDirection.X_UP_Y_UP:
            case GridDirection.Y_UP_Z_DOWN:
            case GridDirection.Y_UP_Z_UP:

            case GridDirection.X_DOWN_Y_UP_Z_DOWN:
            case GridDirection.X_DOWN_Y_UP_Z_UP:
            case GridDirection.X_UP_Y_UP_Z_DOWN:
            case GridDirection.X_UP_Y_UP_Z_UP:
                y += 1;
                break;
        }
        switch (gridDirection)
        {

            case GridDirection.Z_DOWN:

            case GridDirection.X_DOWN_Z_DOWN:
            case GridDirection.X_UP_Z_DOWN:
            case GridDirection.Y_DOWN_Z_DOWN:
            case GridDirection.Y_UP_Z_DOWN:

            case GridDirection.X_DOWN_Y_DOWN_Z_DOWN:
            case GridDirection.X_DOWN_Y_UP_Z_DOWN:
            case GridDirection.X_UP_Y_DOWN_Z_DOWN:
            case GridDirection.X_UP_Y_UP_Z_DOWN:
                z -= 1;
                break;
            case GridDirection.Z_UP:

            case GridDirection.X_DOWN_Z_UP:
            case GridDirection.X_UP_Z_UP:
            case GridDirection.Y_DOWN_Z_UP:
            case GridDirection.Y_UP_Z_UP:

            case GridDirection.X_DOWN_Y_DOWN_Z_UP:
            case GridDirection.X_DOWN_Y_UP_Z_UP:
            case GridDirection.X_UP_Y_DOWN_Z_UP:
            case GridDirection.X_UP_Y_UP_Z_UP:
                z += 1;
                break;
        }
	}

	/**
	 * Adjusts x/y/z as we moved opposite to that direction.
	 */
	public static void navigateBackwards(this GridDirection gridDirection, ref int x, ref int y, ref int z)
    {
        switch (gridDirection)
        {
            case GridDirection.X_DOWN:

            case GridDirection.X_DOWN_Y_DOWN:
            case GridDirection.X_DOWN_Y_UP:
            case GridDirection.X_DOWN_Z_DOWN:
            case GridDirection.X_DOWN_Z_UP:

            case GridDirection.X_DOWN_Y_DOWN_Z_DOWN:
            case GridDirection.X_DOWN_Y_DOWN_Z_UP:
            case GridDirection.X_DOWN_Y_UP_Z_DOWN:
            case GridDirection.X_DOWN_Y_UP_Z_UP:
                x += 1;
                break;
            case GridDirection.X_UP:

            case GridDirection.X_UP_Y_DOWN:
            case GridDirection.X_UP_Y_UP:
            case GridDirection.X_UP_Z_DOWN:
            case GridDirection.X_UP_Z_UP:

            case GridDirection.X_UP_Y_DOWN_Z_DOWN:
            case GridDirection.X_UP_Y_DOWN_Z_UP:
            case GridDirection.X_UP_Y_UP_Z_DOWN:
            case GridDirection.X_UP_Y_UP_Z_UP:
                x -= 1;
                break;
        }
        switch (gridDirection)
        {

            case GridDirection.Y_DOWN:

            case GridDirection.X_DOWN_Y_DOWN:
            case GridDirection.X_UP_Y_DOWN:
            case GridDirection.Y_DOWN_Z_DOWN:
            case GridDirection.Y_DOWN_Z_UP:

            case GridDirection.X_DOWN_Y_DOWN_Z_DOWN:
            case GridDirection.X_DOWN_Y_DOWN_Z_UP:
            case GridDirection.X_UP_Y_DOWN_Z_DOWN:
            case GridDirection.X_UP_Y_DOWN_Z_UP:
                y += 1;
                break;
            case GridDirection.Y_UP:

            case GridDirection.X_DOWN_Y_UP:
            case GridDirection.X_UP_Y_UP:
            case GridDirection.Y_UP_Z_DOWN:
            case GridDirection.Y_UP_Z_UP:

            case GridDirection.X_DOWN_Y_UP_Z_DOWN:
            case GridDirection.X_DOWN_Y_UP_Z_UP:
            case GridDirection.X_UP_Y_UP_Z_DOWN:
            case GridDirection.X_UP_Y_UP_Z_UP:
                y -= 1;
                break;
        }
        switch (gridDirection)
        {

            case GridDirection.Z_DOWN:

            case GridDirection.X_DOWN_Z_DOWN:
            case GridDirection.X_UP_Z_DOWN:
            case GridDirection.Y_DOWN_Z_DOWN:
            case GridDirection.Y_UP_Z_DOWN:

            case GridDirection.X_DOWN_Y_DOWN_Z_DOWN:
            case GridDirection.X_DOWN_Y_UP_Z_DOWN:
            case GridDirection.X_UP_Y_DOWN_Z_DOWN:
            case GridDirection.X_UP_Y_UP_Z_DOWN:
                z += 1;
                break;
            case GridDirection.Z_UP:

            case GridDirection.X_DOWN_Z_UP:
            case GridDirection.X_UP_Z_UP:
            case GridDirection.Y_DOWN_Z_UP:
            case GridDirection.Y_UP_Z_UP:

            case GridDirection.X_DOWN_Y_DOWN_Z_UP:
            case GridDirection.X_DOWN_Y_UP_Z_UP:
            case GridDirection.X_UP_Y_DOWN_Z_UP:
            case GridDirection.X_UP_Y_UP_Z_UP:
                z -= 1;
                break;
        }
    }

	/**
	 * Returns the next grid direction in the iteration order
	 */
	public static GridDirection next(this GridDirection gridDirection)
	{
		return (GridDirection)(((ulong) gridDirection) * 2);
	}

	/**
	 * Validates if we can move in the given direction from the current x/y/z
	 */ 
	public static bool validate(this GridDirection gridDirection, int x, int y, int z)
    {
        bool returnValue = true;
        switch (gridDirection)
        {
            case GridDirection.X_DOWN:

            case GridDirection.X_DOWN_Y_DOWN:
            case GridDirection.X_DOWN_Y_UP:
            case GridDirection.X_DOWN_Z_DOWN:
            case GridDirection.X_DOWN_Z_UP:

            case GridDirection.X_DOWN_Y_DOWN_Z_DOWN:
            case GridDirection.X_DOWN_Y_DOWN_Z_UP:
            case GridDirection.X_DOWN_Y_UP_Z_DOWN:
            case GridDirection.X_DOWN_Y_UP_Z_UP:
                returnValue = returnValue && x > 0;
                break;
            case GridDirection.X_UP:

            case GridDirection.X_UP_Y_DOWN:
            case GridDirection.X_UP_Y_UP:
            case GridDirection.X_UP_Z_DOWN:
            case GridDirection.X_UP_Z_UP:

            case GridDirection.X_UP_Y_DOWN_Z_DOWN:
            case GridDirection.X_UP_Y_DOWN_Z_UP:
            case GridDirection.X_UP_Y_UP_Z_DOWN:
            case GridDirection.X_UP_Y_UP_Z_UP:
                returnValue = returnValue && (x < SceneRoot.X_DIM - 1);
                break;
        }
        switch (gridDirection)
        {

            case GridDirection.Y_DOWN:

            case GridDirection.X_DOWN_Y_DOWN:
            case GridDirection.X_UP_Y_DOWN:
            case GridDirection.Y_DOWN_Z_DOWN:
            case GridDirection.Y_DOWN_Z_UP:

            case GridDirection.X_DOWN_Y_DOWN_Z_DOWN:
            case GridDirection.X_DOWN_Y_DOWN_Z_UP:
            case GridDirection.X_UP_Y_DOWN_Z_DOWN:
            case GridDirection.X_UP_Y_DOWN_Z_UP:
                returnValue = returnValue && y > 0;
                break;
            case GridDirection.Y_UP:

            case GridDirection.X_DOWN_Y_UP:
            case GridDirection.X_UP_Y_UP:
            case GridDirection.Y_UP_Z_DOWN:
            case GridDirection.Y_UP_Z_UP:

            case GridDirection.X_DOWN_Y_UP_Z_DOWN:
            case GridDirection.X_DOWN_Y_UP_Z_UP:
            case GridDirection.X_UP_Y_UP_Z_DOWN:
            case GridDirection.X_UP_Y_UP_Z_UP:
                returnValue = returnValue && (y < SceneRoot.Y_DIM - 1);
                break;
        }
        switch (gridDirection)
        {

            case GridDirection.Z_DOWN:

            case GridDirection.X_DOWN_Z_DOWN:
            case GridDirection.X_UP_Z_DOWN:
            case GridDirection.Y_DOWN_Z_DOWN:
            case GridDirection.Y_UP_Z_DOWN:

            case GridDirection.X_DOWN_Y_DOWN_Z_DOWN:
            case GridDirection.X_DOWN_Y_UP_Z_DOWN:
            case GridDirection.X_UP_Y_DOWN_Z_DOWN:
            case GridDirection.X_UP_Y_UP_Z_DOWN:
                returnValue = returnValue && z > 0;
                break;
            case GridDirection.Z_UP:

            case GridDirection.X_DOWN_Z_UP:
            case GridDirection.X_UP_Z_UP:
            case GridDirection.Y_DOWN_Z_UP:
            case GridDirection.Y_UP_Z_UP:

            case GridDirection.X_DOWN_Y_DOWN_Z_UP:
            case GridDirection.X_DOWN_Y_UP_Z_UP:
            case GridDirection.X_UP_Y_DOWN_Z_UP:
            case GridDirection.X_UP_Y_UP_Z_UP:
                returnValue = returnValue && (z < SceneRoot.Z_DIM - 1);
                break;
        }
        return returnValue;
    }

	/**
	 * Returns a reference to the object at the space next to us in this direction.
	 */ 
	public static T inspectForwards<T>(this GridDirection gridDirection, T[,,] array, int x, int y, int z)
	{
		gridDirection.navigateForwards(ref x, ref y, ref z);
		return array[x, y, z];
	}
}