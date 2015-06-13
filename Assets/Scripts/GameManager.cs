using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
	Player[] players;
	Board board;

	// Use this for initialization
	void Start () {
		int numberOfPlayers = 3;


		board = new Board (numberOfPlayers);

		players = new Player[numberOfPlayers];
		for(int i = 0; i < numberOfPlayers; ++i) {
			players[i] = new DevicePlayer (i + 1);
		}

		this.players [board.currentPlayerId - 1].StartTurn ();

		GridController.instance.board = board;
		GridController.instance.CreateGrid ();
	}
	
	// Update is called once per frame
	void Update () {
		MoveInfo move = players [board.currentPlayerId - 1].GetNextMove ();
		if (move != null) {
			this.board.MakeMove (move.start, move.end, move.amount);
			NextPlayer();
		}
	}

	void NextPlayer() {
		this.players [board.currentPlayerId - 1].EndTurn ();
		board.NextPlayerMove ();
		this.players [board.currentPlayerId - 1].StartTurn ();
	}
}
