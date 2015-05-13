using UnityEngine;
using System.Collections;

public class HexController : MonoBehaviour 
{
	public GridController gridConteroller;

	public int ownerID;
	public int nArmies;
	public bool isUnderFog = false;
	public int x;
	public int y;

	public bool send;
	public bool recieve;

	void Start () 
	{
	
	}

	void Update () 
	{
	/*	if (Input.GetMouseButtonDown (0)) {
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit)) {
				Debug.Log ("Name = " + hit.collider.name);
				Debug.Log ("Tag = " + hit.collider.tag);
				Debug.Log ("Hit Point = " + hit.point);
				Debug.Log ("Object position = " + hit.collider.gameObject.transform.position);
				Debug.Log ("--------------");
			}
		}*/
	}

	void OnMouseDown ()
	{
		Debug.Log (x.ToString() + " " + y.ToString() + " " + ownerID.ToString() + " " + nArmies.ToString());
		//GridController.SelectCell (x, y);

		gridConteroller.SelectCell (x, y);
	}


}
