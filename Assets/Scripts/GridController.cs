using UnityEngine;
using System.Collections;

public class GridController : MonoBehaviour 
{
	public GameObject hex;
	public float r;
	public float margin;

	GameObject[,] grid;
	Color[] armyColors;

	int w;
	int h;

	GameObject startCell;

	int[,] mask = 
		{{1, 1, 1, 1, 1},
		 {1, 1, 1, 1, 1},
		 {1, 1, 1, 1, 1},
		 {1, 1, 1, 1, 1},
		 {1, 1, 1, 1, 1}};


	void Start () 
	{
		CreateGrid ();
	}

	void Update () {

	}
	/*
	void MakeMove(int x1, int y1, int x2, int y2, int size) {
		BoardCell startCell = getCell (x1, y1);
		BoardCell endCell = getCell (x2, y2);
		
		startCell.armyInfo.armySize -= size;
		if (startCell.armyInfo.playerID == endCell.armyInfo.playerID) {
			endCell.armyInfo.armySize += size;
		} else if (endCell.armyInfo == null) {
			endCell.armyInfo = new BoardCell.ArmyInfo (size, startCell.armyInfo.playerID);
		} else {
			endCell.armyInfo = CalcucateBattle(startCell.armyInfo, endCell.armyInfo);
		}
	}
*/

	public void SelectCell(int x, int y)
	{
		GameObject currentCell = grid[x, y];
		if (startCell == null) {
			startCell = currentCell;
		} else if (startCell == currentCell) {
			startCell = null;
		} else {
			HexController startHex = startCell.GetComponent<HexController>();
			HexController endHex = currentCell.GetComponent<HexController>();

			if (startHex.ownerID == endHex.ownerID) {
				endHex.nArmies += startHex.nArmies;
			} else {
				if(endHex.nArmies >= startHex.nArmies) {
					endHex.nArmies -= startHex.nArmies;
				} else {
					endHex.nArmies = startHex.nArmies - endHex.nArmies;
					endHex.ownerID = startHex.ownerID;
				}
			}
			startHex.ownerID = 0;
			startHex.nArmies = 0;

			Renderer renderer = startCell.GetComponentInChildren<Renderer>();
			renderer.material.color = armyColors[startHex.ownerID];
			TextMesh cellText = startCell.GetComponentInChildren<TextMesh>();
			cellText.text = startHex.nArmies.ToString();


			renderer = currentCell.GetComponentInChildren<Renderer>();
			renderer.material.color = armyColors[endHex.ownerID];
			cellText = currentCell.GetComponentInChildren<TextMesh>();
			cellText.text = endHex.nArmies.ToString();

			startCell = null;
		}
	}

	void CreateGrid()
	{
		float h = r*0.8660254f; // sqrt(3) / 2
		float xOffset = (2*h + margin)*0.8660254f;
		float yOffset = 2*h + margin;

		h = mask.GetUpperBound (0) + 1;
		w = mask.GetUpperBound (1) + 1;
		grid = new GameObject[5,5];
		armyColors = new Color[4];
		armyColors [0] = new Color (255, 255, 255);
		armyColors [1] = new Color (255, 0, 0);
		armyColors [2] = new Color (0, 255, 0);
		armyColors [3] = new Color (0, 0, 255);

		Debug.Log ("Start Debug");
		//Debug.Log (armyColors[2].ToString());

		for (int i = 0; i < h; i++)
		{
			for (int j = 0; j < w; j++)
			{
				if (mask[i,j] != 0)
				{
					//grid[i,j] = Instantiate (hex, new Vector3 (j, i, 0f), Quaternion.identity) as GameObject;
					grid[i,j] = Instantiate (hex, new Vector3(j*xOffset, (0.5f*j - i)*yOffset, 0), Quaternion.identity) as GameObject;

					//TextMesh cellText = this.GetComponentInChildren<TextMesh>();
					//cellText.text = "1";

//					Renderer renderer = grid[i,j].GetComponentInChildren<Renderer>();
//					renderer.material.color = armyColors[0];

					//renderer.material = armyColors[1];
					//renderer.sharedMaterial = armyColors[0];

					//grid[i,j].renderer.sharedMaterial = armyColors[2];
					//MeshRenderer meshRenderer = grid[i,j].GetComponent<MeshRenderer>();
					//meshRenderer.material = armyColors[3];

					//grid[i,j] = ObjectPool.Instance.GetObject(0);
					//grid[i,j].transform.position = new Vector3(j*xOffset, 0, (0.5f*j - i)*yOffset);
					HexController cellController = grid[i,j].GetComponent<HexController>();
					cellController.x = i;
					cellController.y = j;
					cellController.gridConteroller = this;
					cellController.ownerID = 1;
					cellController.nArmies = 1;
				}
			}
		}
	}
}
