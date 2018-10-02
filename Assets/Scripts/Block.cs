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
		HIDDEN
	}

	public int checkFlag { get; set; }

	public short checkedFrom { get; set; }

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

	public bool getCheckDirectionBit(int flag)
	{
		return checkDirection[flag];
	}

	public void setCheckDirectionBit(int flag, bool bit)
	{
		checkDirection[flag] = bit;
	}

	private void recalculateRendering() {
		if (!_renderable)
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
				case Visibility.BLOCKED:
				case Visibility.HIDDEN:
					gameObject.GetComponent<MeshRenderer>().enabled = false;
					break;
			}
		}
	}

}
