using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ArmySelectorPanel : MonoBehaviour {

	Vector3 hangingPosition;
	RectTransform rectTransform;
	RectTransform canvas;
	Text nArmiesText;

	void Start () 
	{
		canvas 		  = GetComponentInParent<RectTransform> ();
		rectTransform = GetComponent<RectTransform> ();
		nArmiesText   = GetComponentInChildren<Text> ();
	}
	
	void Update () 
	{
		Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(Camera.main, hangingPosition);		
		rectTransform.position = screenPoint; //  anchoredPosition = screenPoint - canvas.sizeDelta ;
	}

	public void ShowAtObject (GameObject cell)
	{
		hangingPosition = cell.transform.position + new Vector3(0,1,-2);
		nArmiesText.text = cell.GetComponent<HexController> ().nArmiesAvailable.ToString ();
		gameObject.SetActive (true);
	}

	public void Hide ()
	{
		gameObject.SetActive (false);
	}
}
