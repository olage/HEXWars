using UnityEngine;
using System.Collections;

public class DevicePlayer : Player {
	public MoveInfo nextMove;

	public DevicePlayer(int id) :base(id) {	}

	public override MoveInfo GetNextMove() {
		return nextMove;
	}
	
	public override void StartTurn() {
		this.endTurn = false;
		GridController.instance.currentPlayer = this;
	}
	
	public override void EndTurn() {
		GridController.instance.currentPlayer = null;
		nextMove = null;
	}
}
