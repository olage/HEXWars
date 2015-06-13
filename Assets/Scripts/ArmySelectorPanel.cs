using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ArmySelectorPanel : MonoBehaviour {

	Vector3 hangingPosition;
	RectTransform rectTransform;
	RectTransform canvas;
	Text nArmiesText;
	Scrollbar armiesPart;

	void Start () 
	{
		canvas 		  = GetComponentInParent<RectTransform> ();
		rectTransform = GetComponent<RectTransform> ();
		nArmiesText   = GetComponentInChildren<Text> ();
		armiesPart    = GetComponentInChildren<Scrollbar> ();
		Hide ();
	}
	
	void Update () 
	{
		Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(Camera.main, hangingPosition);		
		rectTransform.position = screenPoint; //  anchoredPosition = screenPoint - canvas.sizeDelta ;

	}

	public void ShowAtObject (GameObject cell)
	{
		hangingPosition = cell.transform.position + new Vector3(0,1,-2);
		nArmiesText.text = cell.GetComponent<HexController> ().cellInfo.armySize.ToString ();
		gameObject.SetActive (true);
	}

	public void Hide ()
	{
		gameObject.SetActive (false);
	}
}