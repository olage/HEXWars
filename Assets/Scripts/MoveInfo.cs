using UnityEngine;
using System.Collections;

public class MoveInfo {
	public idx start;
	public idx end;
	public int amount;

	public MoveInfo(idx start, idx end, int amount) {
		this.start = start;
		this.end = end;
		this.amount = amount;
	}
}
