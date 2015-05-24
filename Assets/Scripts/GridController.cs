using UnityEngine;
using System.Collections;

public class GridController : MonoBehaviour 
{
	public RectTransform menu;
	
	public GameObject hex;
	public float r;
	public float margin;

	public Board board;

	GameObject[,] grid;

	int w;
	int h;

	idx startCell;
	idx currentCell;

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

		if (!board.IsOnBoard (currentCell)) {
			return;
		}		

		if (startCell == null) {
			startCell = currentCell;
			ToggleReachArea (startCell.x, startCell.y, true);
			ShowMenu (grid [startCell.x, startCell.y]);
		} else {
			if (board.IsMovePossible (startCell, currentCell)) {
				// TODO: board.MakeMove(startCell, currentCell, amount)
				board.MakeMove(startCell, currentCell);
			}

			ToggleReachArea (startCell.x, startCell.y, false);
			startCell = null;
			HideMenu ();
		}
	}

	void CreateGrid()
	{
		float cell_h  = r*0.8660254f; // sqrt(3) / 2
		float xOffset = (2*cell_h + margin)*0.8660254f;
		float yOffset =  2*cell_h + margin;

		int boardEdgeSize = 3;
		board = new Board (boardEdgeSize);
		
		h = board.ArraySize;
		w = board.ArraySize;

		grid = new GameObject[h, w];

		Debug.Log ("Start Debug");

		for (int i = 0; i < h; i++) {
			for (int j = 0; j < w; j++) {
				if (board.IsOnBoard(i, j)) {
					grid[i,j] = Instantiate (hex, new Vector3(j*xOffset, (0.5f*j - i)*yOffset, 0), Quaternion.identity) as GameObject;

					HexController cellController = grid[i,j].GetComponent<HexController>();
					cellController.x = i;
					cellController.y = j;
					cellController.gridConteroller = this;
					cellController.cellInfo = board.cells[i, j];
			//		cellController.ownerID = 1;
			//		cellController.nArmiesTotal = 1;
				}
			}
		}
	}

	void ToggleReachArea(int x, int y, bool toggle)
	{
		foreach (idx neib in board.GetCellNeighbours(new idx(x, y))) {
			grid[neib.x, neib.y].GetComponent<HexController>().accessible = toggle;
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
