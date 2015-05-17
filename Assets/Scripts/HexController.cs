using UnityEngine;
using System.Collections;

public class HexController : MonoBehaviour 
{
	public GridController gridConteroller;
	public new Renderer renderer;

	public int ownerID;
	public int nArmies;
	public bool isUnderFog = false;
	public int x;
	public int y;

	public bool send;
	public bool recieve;
	public bool accessible;

	Color[] color = {Color.gray, Color.red, Color.green, Color.blue, Color.yellow};

	void Start () 
	{
		renderer = GetComponent<Renderer> ();
	}

	void Update () 
	{
		renderer.material.color = color [ownerID];

		if (accessible)
			renderer.material.color = Color.white;
	}

	void OnMouseDown ()
	{
		Debug.Log (x.ToString() + " " + y.ToString() + " " + ownerID.ToString() + " " + nArmies.ToString());
		gridConteroller.SelectCell (x, y);
	}


}
