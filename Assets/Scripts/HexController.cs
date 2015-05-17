using UnityEngine;
using System.Collections;

public class HexController : MonoBehaviour 
{
	public GridController gridConteroller;

	public int ownerID;
	public int nArmiesTotal;
	public int nArmiesSend;
	public int nArmiesAvailable;
	public int x;
	public int y;

	public bool send;
	public bool recieve;
	public bool accessible;
	public bool isUnderFog = false;

	new Renderer renderer;
	Color[] color = {Color.gray, Color.red, Color.green, Color.blue, Color.yellow};

	void Start () 
	{
		renderer = GetComponent<Renderer> ();
	}

	void Update () 
	{
		if (nArmiesTotal == 0) 
			ownerID = 0;

		nArmiesAvailable = nArmiesTotal;

		renderer.material.color = color [ownerID];

		if (accessible)
			renderer.material.color += Color.grey;

		if (ownerID != 0) 
			GetComponentInChildren<TextMesh>().text = nArmiesTotal.ToString();
		else
			GetComponentInChildren<TextMesh>().text = null;
	}

	void OnMouseDown ()
	{
		//Debug.Log (x.ToString() + " " + y.ToString() + " " + ownerID.ToString() + " " + nArmiesTotal.ToString());
		if (ownerID != 0)
			gridConteroller.SelectCell (x, y);
	}


}
