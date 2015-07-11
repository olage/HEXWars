using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
	Player[] players;
	Board board;
	Player currentPlayer;

	public UIManager uiManager;

	void Start () {
		int numberOfPlayers = 2;

		board = new Board (3, numberOfPlayers);



		players = new Player[numberOfPlayers];
		for(int i = 0; i < numberOfPlayers; ++i) {
			players[i] = new DevicePlayer (i + 1);
		}

		currentPlayer = this.players [board.currentPlayerId - 1];
		currentPlayer.StartTurn ();

		GridController.instance.board = board;
		GridController.instance.CreateGrid ();
	}

	void Update () {
		MoveInfo move = currentPlayer.GetNextMove ();
		if (move != null) {
			if(move.moveType == MoveInfo.MoveType.Grow) {
				this.board.MakeGrow (move.start);
			} else if(move.moveType == MoveInfo.MoveType.Move) {
				this.board.MakeMove (move.start, move.end, move.amount);
			}
			currentPlayer.MakeNextMove ();
		}

		if(currentPlayer.IsEndTurn()) {
			this.NextPlayer();
		}

		uiManager.UpdateUI (board);
	}

	void NextPlayer() {
		this.currentPlayer.EndTurn ();
		board.NextPlayerMove ();
		currentPlayer = this.players [board.currentPlayerId - 1];
		currentPlayer.StartTurn ();
	}
}
