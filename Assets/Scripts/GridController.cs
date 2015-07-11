using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class GridController : MonoBehaviour
{
	public static GridController instance { get; private set; }

	void Awake() {
		instance = this;
	}

	public DevicePlayer currentPlayer = null;

	public RectTransform menu;
	
	public GameObject hex;
	public float r;
	public float margin;

	public Board board;

	public ArmySelectorPanel armySelectorPanel;

	System.Random rnd = new System.Random();

	GameObject[,] grid;

	int w;
	int h;

	idx startCell;


	public void SelectCellOnGrowPhase(int x, int y)
	{
		idx currentCell = new idx (x, y);	
		if (board.IsGrowPossible (currentCell)) {
			MoveInfo move = new MoveInfo(currentCell);
			if(currentPlayer != null) {
				currentPlayer.nextMove = move;
			}
		}
	}

	public void SelectCellOnMovePhase(int x, int y)
	{
		idx currentCell = new idx (x, y);

		if (startCell == null) {
			if (board.CanMoveFromCell(currentCell)) {
				startCell = currentCell;
				ToggleReachArea (startCell.x, startCell.y, true);
				Debug.Log("Cell" + grid[startCell.x, startCell.y]);
				ShowMenu (grid [startCell.x, startCell.y]);
			}
		} else {
			if (board.IsMovePossible (startCell, currentCell)) {
				//int moveAmount = System.Math.Max (1, rnd.Next(board.GetCellArmySize(startCell)));
				int moveAmount = armySelectorPanel.currentArmySize;
				MoveInfo move = new MoveInfo(startCell, currentCell, moveAmount);
				if (currentPlayer != null) {
					currentPlayer.nextMove = move;
				}
			}
			
			ToggleReachArea (startCell.x, startCell.y, false);
			startCell = null;
			HideMenu ();
		}
	}

	public void SelectCell(int x, int y)
	{
		if (board.turnPhase == Board.TurnPhase.GrowPhase) {
			SelectCellOnGrowPhase (x, y);
		} else if (board.turnPhase == Board.TurnPhase.MovePhase) {
			SelectCellOnMovePhase (x, y);
		}
	}

	public void CreateGrid()
	{
		float cell_h  = r*0.8660254f; // sqrt(3) / 2
		float xOffset = (2*cell_h + margin)*0.8660254f;
		float yOffset =  2*cell_h + margin;

		grid = new GameObject[board.ArraySize, board.ArraySize];

		Debug.Log ("Start Debug");

		for (int i = 0; i < board.ArraySize; i++) {
			for (int j = 0; j < board.ArraySize; j++) {
				if (board.IsOnBoard(i, j)) {
					grid[i,j] = Instantiate (hex, new Vector3(j*xOffset, (0.5f*j - i)*yOffset, 0), Quaternion.identity) as GameObject;

					HexController cellController = grid[i,j].GetComponent<HexController>();
					cellController.x = i;
					cellController.y = j;
					cellController.gridConteroller = this;
					cellController.cellInfo = board.cells[i, j];
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
		armySelectorPanel.ShowAtObject (cell);
		//Vector3 hangingPos = cell.transform.position + new Vector3(0,1,-2);
	}

	void HideMenu()
	{
		armySelectorPanel.Hide ();
	}

	public void SetEndTurn() {
		this.currentPlayer.SetEndTurn ();
	}
}
