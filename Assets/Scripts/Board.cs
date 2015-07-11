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

	public enum TurnPhase {
		GrowPhase,
		MovePhase
	}

	public TurnPhase turnPhase;

	public CellInfo[, ] cells;

	private int edgeSize;
	private int arraySize;

	public int ArraySize {
		get { return arraySize; }
	}

	public int[] playersGrowAmount;

	public int currentPlayerId { get; private set; }
	public int numberOfPlayers { get; private set; }

	int[, ] directions = {{1, 0}, {-1, 0}, {0, 1}, {0, -1}, {1, 1}, {-1, -1}};

	public Board(int edgeSize, int numberOfPlayers) {
		this.edgeSize = edgeSize;
		this.arraySize = 2 * edgeSize - 1;
		this.cells = new CellInfo[this.arraySize, this.arraySize];
		this.playersGrowAmount = new int[numberOfPlayers + 1];
		for (int i = 1; i <= numberOfPlayers; ++i) {
			this.playersGrowAmount [i] = -1;
		}

		foreach( idx i in GetCellsIdx()) {
			this.cells[i.x, i.y] = new CellInfo(0, 0);
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

		this.turnPhase = TurnPhase.MovePhase;
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

	private void ComputeGrowRate() {
		foreach( idx i in GetCellsIdx() ) {
			if(this.cells[i.x, i.y].ownerId == currentPlayerId) {
				this.playersGrowAmount[currentPlayerId] += 1;
			}
		}

		if (this.playersGrowAmount [currentPlayerId] == 0) {
			this.turnPhase = Board.TurnPhase.MovePhase;
		}
	}

	public void NextPlayerMove() {
		this.currentPlayerId = (this.currentPlayerId % this.numberOfPlayers) + 1;
		this.turnPhase = TurnPhase.GrowPhase;
		ComputeGrowRate ();
	}
	
	public IEnumerable<idx> GetCellNeighbours(idx start) {
		for(int i = 0; i < directions.GetLength(0); ++i) {

			idx end = new idx(start.x + directions[i, 0], start.y + directions[i, 1]);
			if (IsOnBoard(end)) {
				yield return end;
			}
		}
	}

	public IEnumerable<idx> GetCellsIdx() {
		for(int i = 0; i < this.arraySize; ++i) {
			for(int j = 0; j < this.arraySize; ++j) {
				if(IsOnBoard(i, j)) {
					yield return new idx(i, j);
				}
			}
		}
	}

	public bool IsGrowPossible(idx cell) {
		if (!IsOnBoard (cell)) {
			return false;
		}

		if (this.cells [cell.x, cell.y].ownerId == currentPlayerId && this.playersGrowAmount[currentPlayerId] > 0) {
			return true;
		}
		return false;
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

	public void MakeGrow(idx cell) {
		this.cells [cell.x, cell.y].armySize += 1;
		this.playersGrowAmount [currentPlayerId] -= 1;

		if (this.playersGrowAmount [currentPlayerId] == 0) {
			this.turnPhase = TurnPhase.MovePhase;
		}
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
