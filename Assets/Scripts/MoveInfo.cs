using UnityEngine;
using System.Collections;

public class MoveInfo {
	public enum MoveType {
		Grow,
		Move
	}

	public MoveType moveType;
	public idx start;
	public idx end;
	public int amount;

	public MoveInfo(idx start, idx end, int amount) {
		this.start = start;
		this.end = end;
		this.amount = amount;
		this.moveType = MoveType.Move;
	}

	public MoveInfo(idx start) {
		this.start = start;
		this.moveType = MoveType.Grow;
	}
}
