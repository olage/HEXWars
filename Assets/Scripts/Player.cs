using UnityEngine;
using System.Collections;


public abstract class Player {
	int id;
	protected bool endTurn;

	public Player(int id) {
		this.id = id;
	}

	public abstract MoveInfo GetNextMove ();	
	public abstract void MakeNextMove ();
	public abstract void StartTurn();
	public abstract void EndTurn ();

	public bool IsEndTurn() {
		return endTurn;
	}
}
