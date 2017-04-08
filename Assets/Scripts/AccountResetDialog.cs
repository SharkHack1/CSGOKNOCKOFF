using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AccountResetDialog : MonoBehaviour {

	int dialogStage = 0;

	Text dialogText;
	Transform confirmButton;
	Vector3 ConfirmOriginalPos;
	Transform cancelButton;
	Vector3 CancelOriginalPos;

	void Start () {
		dialogText = transform.Find("Text").GetComponent<Text>();
		confirmButton = transform.Find("Confirm");
		ConfirmOriginalPos = confirmButton.position;
		cancelButton = transform.Find("Cancel");
		CancelOriginalPos = cancelButton.position;
	}

	public void ConfirmDelete () {
		switch (dialogStage) {
				
			case 1:
				dialogText.text = "Are you REALLY sure?";
				dialogText.fontSize += 2;
				break;

			case 2:
				dialogText.text = "Are you REALLY REALLY sure?";
				dialogText.fontSize += 2;
				//swap buttton position
				Vector3 tempPos = confirmButton.position;
				confirmButton.position = cancelButton.position;
				cancelButton.position = tempPos;
				break;
			case 3:
				//reset account
				FindObjectOfType<UserSettings>().ResetAccount();

				dialogText.text = "Ok, account has been reset.";
				dialogText.fontSize = 14;
				//hide confirm/cancel buttons
				confirmButton.gameObject.SetActive(false);
				cancelButton.gameObject.SetActive(false);
				break;
		}
		dialogStage++; //increment dialog stage
	}

	public void ResetDialog () {
		if (dialogStage == 0) {
			return;
		}

		dialogStage = 0;

		dialogText.text = "Are you sure? \n This will delete all progress (rank, items, vlads, etc.)";
		dialogText.fontSize = 14;
		//reset buttons
		confirmButton.gameObject.SetActive(true);
		cancelButton.gameObject.SetActive(true);
		confirmButton.position = ConfirmOriginalPos;
		cancelButton.position = CancelOriginalPos;
	}
}
