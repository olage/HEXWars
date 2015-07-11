using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIManager : MonoBehaviour {
	public Text currentPlayerText;
	public Text growRateText;

	public void UpdateUI(Board board) {
		currentPlayerText.text = "Current player: " + board.currentPlayerId.ToString();

		string s = "";
		for(int i = 1; i <= board.numberOfPlayers; ++i) {
			s += "Player " + i.ToString() + ": " + board.playersGrowAmount[i] + "\n";
		}
		growRateText.text = s;
	}
}
