using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ArmySelectorPanel : MonoBehaviour {

	Vector3 hangingPosition;
	RectTransform rectTransform;
	RectTransform canvas;
	Text nArmiesText;
	Slider nArmiesSlider;

	public int currentArmySize { get; private set; }
	int currentMaxArmySize;

	void Start () 
	{
		canvas 		  = GetComponentInParent<RectTransform> ();
		rectTransform = GetComponent<RectTransform> ();
		nArmiesText   = GetComponentInChildren<Text> ();
		nArmiesSlider = GetComponentInChildren<Slider> ();

		Hide ();
	}
	
	void Update () 
	{
		//Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(Camera.main, hangingPosition);		
		//rectTransform.position = screenPoint; //  anchoredPosition = screenPoint - canvas.sizeDelta ;

	}

	public void ShowAtObject (GameObject cell)
	{
		currentMaxArmySize = cell.GetComponent<HexController> ().cellInfo.armySize;
		currentArmySize = currentMaxArmySize;

	//	hangingPosition = cell.transform.position + new Vector3(0,1,-2);
		nArmiesText.text = currentArmySize.ToString ();
		nArmiesSlider.value = 1.0f;
		gameObject.SetActive (true);
	}

	public void Hide ()
	{
		gameObject.SetActive (false);
	}

	public void OnValueUpdate(float f) {
		currentArmySize = (int)System.Math.Ceiling (f * currentMaxArmySize);
		nArmiesText.text = currentArmySize.ToString ();
	}
}