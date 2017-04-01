using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Client : NetworkBehaviour {

	public string username;
	int vlads;
	int totalVladsEarned;

	private const int startingVlads = 100;

	[SerializeField] Transform hostObj;

	Dictionary<string,RectTransform> profileUI = new Dictionary<string, RectTransform>();


	void Start () {
		if (true) {
			Instantiate(hostObj);
		}
	}	

	public override void OnStartLocalPlayer () {
		transform.tag = "LocalPlayer";

		//retrive player data, and if there is no account, create one
		if (PlayerPrefs.HasKey("username")) {
			username = PlayerPrefs.GetString("username");
			vlads = PlayerPrefs.GetInt("vlads");
			totalVladsEarned = PlayerPrefs.GetInt("totalvladsearned");
		} else {
			//Generate Random Username
			System.Guid guid = System.Guid.NewGuid();
			string name = System.Convert.ToBase64String(guid.ToByteArray());
			name = name.Replace("=", "");
			name = name.Replace("+", "");

			username = name;
			PlayerPrefs.SetString("username", name);

			//vlads auto save, so no need to call PlayerPrefs
			vlads = startingVlads;

			//total vlads earned
			PlayerPrefs.SetInt("totalvladsearned", 0);
		}

		//map all the profile ui objects
		profileUI.Add("Profile Bar", GameObject.Find("Canvas/Profile/Profile Bar").GetComponent<RectTransform>());
		profileUI.Add("Rank Emblem", GameObject.Find("Canvas/Profile/Profile Bar/Rank Emblem").GetComponent<RectTransform>());
		profileUI.Add("Username", GameObject.Find("Canvas/Profile/Profile Bar/Username").GetComponent<RectTransform>());
		profileUI.Add("Level", GameObject.Find("Canvas/Profile/Profile Bar/Level").GetComponent<RectTransform>());
		

	}

	void OnVladsChanged (int value) {
		totalVladsEarned += value; //add to the total amount gained from betting

		PlayerPrefs.SetInt("vlads", value); //save vlads value in PlayerPrefs
	}

	public int Vlads {
		get {return vlads;}
		set {
			OnVladsChanged(value);
			vlads = value;
		}
	}

}
