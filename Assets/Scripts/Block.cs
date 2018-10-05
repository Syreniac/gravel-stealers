using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

public class Block : MonoBehaviour {

	public enum Visibility
	{
		// Visible - should be rendered
		VISIBLE,
		// Hidden - should not be rendered. Typically only used for blocks outside of the current camera view
		HIDDEN,
		// Blocked - should be rendered, but as blank space
		BLOCKED
	}

	public int checkFlag { get; set; }

	public GridDirection checkedFrom { get; set; }

	private BitVector32 checkDirection = new BitVector32(); 

	private bool _renderable = true; 

	public bool renderable {
		get { return _renderable; }
		set
		{
			_renderable = value;
			recalculateRendering();
		}
	}

	private Visibility _visible;

	public Visibility visible {
		get { return _visible; }
		set
		{
			_visible = value;
			recalculateRendering();
		}
	}

	public Block()
	{
		checkFlag = 0;
	}

	public bool getCheckDirectionBit(GridDirection flag)
	{
		return checkDirection[(int)flag];
	}

	public void setCheckDirectionBit(GridDirection flag, bool bit)
	{
		checkDirection[(int)flag] = bit;
	}

	private void recalculateRendering() {
		if (visible == Visibility.BLOCKED)
		{
			gameObject.GetComponent<MeshRenderer>().enabled = true;

		}
		else if (!_renderable)
		{
			gameObject.GetComponent<MeshRenderer>().enabled = false;
		}
		else
		{
			switch (_visible)
			{
				case Visibility.VISIBLE:
					gameObject.GetComponent<MeshRenderer>().enabled = true;
					break;
				case Visibility.HIDDEN:
					gameObject.GetComponent<MeshRenderer>().enabled = false;
					break;
			}
		}
	}

}
