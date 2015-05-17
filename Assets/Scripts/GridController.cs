using UnityEngine;
using System.Collections;

public class GridController : MonoBehaviour 
{
	class idx
	{
		public int x;
		public int y;
	}

	public GameObject hex;
	public float r;
	public float margin;

	GameObject[,] grid;

	int w;
	int h;

	idx startCell;
	idx currentCell;

	int[,] mask = 
		{{1, 1, 1, 0, 0},
		 {1, 1, 1, 1, 0},
		 {1, 1, 1, 1, 1},
		 {0, 1, 1, 1, 1},
		 {0, 0, 1, 1, 1}};


	void Start () 
	{
		CreateGrid ();
	}

	void Update () 
	{

	}

	public void SelectCell(int x, int y)
	{
		currentCell = new idx ();	
		currentCell.x = x;
		currentCell.y = y;

		if (startCell == null) 
		{
			startCell = currentCell;
			ToggleReachArea (startCell.x, startCell.y, true);
		} 
		else if (startCell == currentCell || !grid[x,y].GetComponent<HexController>().accessible) 
		{
			ToggleReachArea (startCell.x, startCell.y, false);
			startCell = null;
		} 
		else if (grid[x,y].GetComponent<HexController>().accessible)
		{
			HexController startHex = grid[startCell.x, startCell.y].GetComponent<HexController>();
			HexController endHex   = grid[x,y].GetComponent<HexController>();

			if (startHex.ownerID == endHex.ownerID) 
			{
				endHex.nArmies += startHex.nArmies;
			} 
			else 
			{
				if(endHex.nArmies >= startHex.nArmies) 
				{
					endHex.nArmies -= startHex.nArmies;
				} 
				else 
				{
					endHex.nArmies = startHex.nArmies - endHex.nArmies;
					endHex.ownerID = startHex.ownerID;
				}
			}
			startHex.ownerID = 0;
			startHex.nArmies = 0;

			ToggleReachArea(startCell.x, startCell.y, false);
			startCell = null;
		}
	}

	void CreateGrid()
	{
		float cell_h  = r*0.8660254f; // sqrt(3) / 2
		float xOffset = (2*cell_h + margin)*0.8660254f;
		float yOffset =  2*cell_h + margin;

		h = mask.GetUpperBound (0) + 1;
		w = mask.GetUpperBound (1) + 1;

		grid = new GameObject[5,5];

		Debug.Log ("Start Debug");

		for (int i = 0; i < h; i++)
		{
			for (int j = 0; j < w; j++)
			{
				if (mask[i,j] != 0)
				{
					grid[i,j] = Instantiate (hex, new Vector3(j*xOffset, (0.5f*j - i)*yOffset, 0), Quaternion.identity) as GameObject;

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

	void ToggleReachArea(int x, int y, bool toggle)
	{
		for (int i = -1; i < 2; i++) 
		{
			for (int j = -1; j < 2; j++)
			{
				if (j == -i) 
					continue;

				if (x + i > -1 && x + i < h && y + j > -1 && y + j < w)
				{
					if (mask[x+i, y+j] == 1)
						grid [x+i, y+j].GetComponent<HexController>().accessible = toggle;
				}
			}
		}
	}
}
