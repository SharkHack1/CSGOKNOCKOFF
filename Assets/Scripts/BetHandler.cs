using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BetHandler : MonoBehaviour {

	[SerializeField] int minBet;
	int bet;

	GameObject player;
	InputField betField;
	GameObject disabledPanel;

	// Use this for initialization
	void Start () {
		betField = transform.FindChild("BetField").GetComponent<InputField>();
		disabledPanel = transform.FindChild("DisabledOverlay").gameObject;

		betField.text = "0";
	}

	// Update is called every frame
	void Update () {
		if (player == null) {
			player = GameObject.FindGameObjectWithTag("LocalPlayer");
			return;
		}

		int.TryParse(betField.text, out bet);
	}
	
	public void DoubleBet () {
		if (player == null) {
			return;
		}

		betField.text = (2*bet).ToString();
	}

	public void HalfBet () {
		if (player == null) {
			return;
		}

		betField.text = Mathf.RoundToInt(bet/2).ToString();
	}

	public void MaxBet () {
		if (player == null) {
			return;
		}

		betField.text = player.GetComponent<Client>().Vlads.ToString();
	}

	public void MinBet () {
		if (player == null) {
			return;
		}
		
		betField.text = minBet.ToString();
	}

	public void PlaceBet () {
		if (player == null) {
			return;
		}

		//make sure bet is valid

		Client playerInfo = player.GetComponent<Client>();
		if (bet <= playerInfo.Vlads && bet >= minBet) {
			//disabledPanel.SetActive(true);

			//handle bet
			BetEntryUtility.CreateEntry(playerInfo.username, bet);

			playerInfo.Vlads -= bet;
		}
	}
}
