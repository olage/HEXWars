using System.Collections;
using System.Collections.Generic;

public class idx
{
	public idx(int i, int j) { x = i; y = j; }
	public readonly int x, y;

	public bool Equals(idx p)
	{
		if ((object)p == null) {
			return false;
		}

		return (x == p.x) && (y == p.y);
	}
}

public class Board {
	const int EMPTY_OWNER_ID = 0;
	
	public class CellInfo {
		public int ownerId;
		public int armySize;

		public CellInfo(int ownerId, int armySize) {
			this.ownerId = ownerId;
			this.armySize = armySize;
		}

	}

	public CellInfo[, ] cells;

	private int edgeSize;
	private int arraySize;

	public int ArraySize {
		get { return arraySize; }
	}


	public int currentPlayerId { get; private set; }
	public int numberOfPlayers { get; private set; }

	int[, ] directions = {{1, 0}, {-1, 0}, {0, 1}, {0, -1}, {1, 1}, {-1, -1}};

	public Board(int edgeSize, int numberOfPlayers) {
		this.edgeSize = edgeSize;
		this.arraySize = 2 * edgeSize - 1;
		this.cells = new CellInfo[this.arraySize, this.arraySize];

		for(int i = 0; i < this.arraySize; ++i) {
			for(int j = 0; j < this.arraySize; ++j) {
				if(IsOnBoard(i, j)) {
					this.cells[i, j] = new CellInfo(0, 0);
				}
			}
		}

		this.numberOfPlayers = numberOfPlayers;
		if (this.numberOfPlayers == 3) {
			this.cells [0, 0] = new CellInfo (1, 20);
			this.cells [arraySize - 1, edgeSize - 1] = new CellInfo (3, 20);
			this.cells [edgeSize - 1, arraySize - 1] = new CellInfo (2, 20);
		}

		if (this.numberOfPlayers == 2) {
			this.cells [0, 0] = new CellInfo (1, 20);
			this.cells [arraySize - 1, arraySize - 1] = new CellInfo (2, 20);
		}

		this.currentPlayerId = 1;
	}

	public bool IsOnBoard(int x, int y) {
		return (x >= 0 && x < this.arraySize &&
			y >= 0 && y < this.arraySize &&
			(x - y) < this.edgeSize &&
			(y - x) < this.edgeSize);		
	}

	public bool IsOnBoard(idx i) {
		return IsOnBoard (i.x, i.y);
	}

	public void NextPlayerMove() {
		this.currentPlayerId = (this.currentPlayerId % this.numberOfPlayers) + 1;
	}

	public IEnumerable<idx> GetCellNeighbours(idx start) {
		for(int i = 0; i < directions.GetLength(0); ++i) {

			idx end = new idx(start.x + directions[i, 0], start.y + directions[i, 1]);
			if (IsOnBoard(end)) {
				yield return end;
			}
		}
	}

	public bool IsMovePossible(idx start, idx end) {
		if (!IsOnBoard (start) || !IsOnBoard (end)) {
			return false;
		}

		if (this.cells [start.x, start.y].ownerId == EMPTY_OWNER_ID) {
			return false;
		}

		foreach (idx neib in GetCellNeighbours(start)) {
			if (end.Equals(neib)) {
				return true;
			}
		}

		return false;
	}

	public bool CanMoveFromCell(idx start) {
		return this.cells [start.x, start.y].ownerId == this.currentPlayerId;
	}


	public void MakeMove(idx start, idx end, int amount) {
		CellInfo startCell = this.cells [start.x, start.y];
		CellInfo endCell = this.cells [end.x, end.y];	
			
		if (startCell.ownerId == endCell.ownerId) {
			endCell.armySize += amount;
		} else  {
			if(endCell.armySize >= amount) {
				endCell.armySize -= amount;
			} else {
				endCell.armySize = amount - endCell.armySize;
				endCell.ownerId = startCell.ownerId;
			}
		}

		startCell.armySize -= amount;
		if (startCell.armySize == 0) {
			startCell.ownerId = 0;
		}


	//	this.NextPlayerMove ();
	}

	public int GetCellArmySize(idx pos) {
		return cells [pos.x, pos.y].armySize;
	}
}
