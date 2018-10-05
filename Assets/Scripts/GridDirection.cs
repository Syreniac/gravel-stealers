using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public enum GridDirection
{
	// Deliberately specified so that we can use them as bit set indexes!
	X_DOWN = 1,
	X_UP = 2,
	Y_DOWN = 4,
	Y_UP = 8,
	Z_DOWN = 16,
	Z_UP = 32
}

public static class GridDirectionExtension
{
	// Enum-like entries to help abstract away some of the maths
	// Don't hate me, this is all compiler legal
	public static readonly GridDirection ALL = (GridDirection.X_DOWN | GridDirection.X_UP | GridDirection.Y_DOWN | GridDirection.Y_UP | GridDirection.Z_DOWN | GridDirection.Z_UP);

	public static readonly GridDirection NONE = 0;

	public static readonly GridDirection LOOP_START = GridDirection.X_DOWN;

	public static readonly GridDirection LOOP_END = (GridDirection) ( (int) GridDirection.Z_UP * 2);

	/**
	 * Adjusts x/y/z as we moved in that direction.
	 */
	public static void navigateForwards(this GridDirection gridDirection, ref int x, ref int y, ref int z)
	{
		switch (gridDirection)
		{
			case GridDirection.X_DOWN:
				x -= 1;
				break;
			case GridDirection.X_UP:
				x += 1;
				break;
			case GridDirection.Y_DOWN:
				y -= 1;
				break;
			case GridDirection.Y_UP:
				y += 1;
				break;
			case GridDirection.Z_DOWN:
				z -= 1;
				break;
			case GridDirection.Z_UP:
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
				x += 1;
				break;
			case GridDirection.X_UP:
				x -= 1;
				break;
			case GridDirection.Y_DOWN:
				y += 1;
				break;
			case GridDirection.Y_UP:
				y -= 1;
				break;
			case GridDirection.Z_DOWN:
				z += 1;
				break;
			case GridDirection.Z_UP:
				z -= 1;
				break;
		}
	}

	/**
	 * Returns the next grid direction in the iteration order
	 */
	public static GridDirection next(this GridDirection gridDirection)
	{
		return (GridDirection)((int) gridDirection * 2);
	}

	/**
	 * Validates if we can move in the given direction from the current x/y/z
	 */ 
	public static bool validate(this GridDirection gridDirection, int x, int y, int z)
	{
		switch (gridDirection)
		{
			case GridDirection.X_DOWN:
				return x > 0;
			case GridDirection.X_UP:
				return x < (SceneRoot.X_DIM - 1);
			case GridDirection.Y_DOWN:
				return y > 0;
			case GridDirection.Y_UP:
				return y < (SceneRoot.Y_DIM - 1);
			case GridDirection.Z_DOWN:
				return z > 0;
			case GridDirection.Z_UP:
				return z < (SceneRoot.Z_DIM - 1);
		}
		throw new Exception();
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