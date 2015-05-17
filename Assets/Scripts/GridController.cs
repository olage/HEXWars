using UnityEngine;
using System.Collections;

public class GridController : MonoBehaviour 
{
	class idx
	{
		public idx(int i, int j) { x = i; y = j; }
		public int x, y;
	}

	public RectTransform menu;
	
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
		currentCell = new idx (x, y);	

		if (startCell == null) 
		{
			startCell = currentCell;
			ToggleReachArea (startCell.x, startCell.y, true);
			ShowMenu (grid[startCell.x, startCell.y]);
		} 
		else if (startCell == currentCell || !grid[x,y].GetComponent<HexController>().accessible) 
		{
			ToggleReachArea (startCell.x, startCell.y, false);
			startCell = null;
			HideMenu ();
		} 
		else if (grid[x,y].GetComponent<HexController>().accessible)
		{
			HexController startHex = grid[startCell.x, startCell.y].GetComponent<HexController>();
			HexController endHex   = grid[x,y].GetComponent<HexController>();

			if (startHex.ownerID == endHex.ownerID) 
			{
				endHex.nArmiesTotal += startHex.nArmiesTotal;
			} 
			else 
			{
				if(endHex.nArmiesTotal >= startHex.nArmiesTotal) 
				{
					endHex.nArmiesTotal -= startHex.nArmiesTotal;
				} 
				else 
				{
					endHex.nArmiesTotal = startHex.nArmiesTotal - endHex.nArmiesTotal;
					endHex.ownerID = startHex.ownerID;
				}
			}
			startHex.ownerID = 0;
			startHex.nArmiesTotal = 0;

			ToggleReachArea(startCell.x, startCell.y, false);
			startCell = null;
			HideMenu();
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
					cellController.nArmiesTotal = 1;
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

	void ShowMenu(GameObject cell)
	{
		//Vector3 hangingPos = cell.transform.position + new Vector3(0,1,-2);
		menu.GetComponent<ArmySelectorPanel>().ShowAtObject (cell);
	}

	void HideMenu()
	{
		menu.GetComponent<ArmySelectorPanel> ().Hide ();
	}
}
