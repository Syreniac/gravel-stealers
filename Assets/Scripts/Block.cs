using System;
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
		// Blocked - should be rendered ignoring the visibility
		BLOCKED,

        HIDDEN_2
	}

    public int _checkFlag;

	public int checkFlag { get { return _checkFlag; } set { _checkFlag = value; } }

	public ulong checkedFrom { get; set; }

	public ulong checkDirection = 0; 

	public bool _renderable = true; 

	public bool renderable {
		get { return _renderable; }
		set
		{
			_renderable = value;
			recalculateRendering();
		}
	}

	public Visibility _visible;

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
		return (checkDirection & ((ulong) flag)) != 0;
	}

	public void setCheckDirectionBit(GridDirection flag, bool bit)
	{
		checkDirection = bit ? (checkDirection | ((ulong)flag)) : (checkDirection ^ ((ulong)flag));
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
                case Visibility.HIDDEN_2:
                case Visibility.HIDDEN:
                    gameObject.GetComponent<MeshRenderer>().enabled = false;
					break;
			}
		}
	}

}
