using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Client : NetworkBehaviour {

	[SerializeField] string username;
	[SerializeField] int vlads;
	[SerializeField] int totalVladsEarned;

	private const int startingVlads = 100;

	[SerializeField] Transform hostObj;

	Dictionary<string, GameObject> profileUI = new Dictionary<string, GameObject>();


	void Start () {
		//change to true for debugging
		if (isServer) {
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

			//Username also auto save, so no need to call PlayerPrefs
			Username = name;

			//vlads auto save, so no need to call PlayerPrefs
			Vlads = startingVlads;

			//total vlads earned
			PlayerPrefs.SetInt("totalvladsearned", 0);
		}

		//clear all ui mappings
		profileUI = new Dictionary<string, GameObject>();

		//map all the profile bar objects
		profileUI.Add("Rank Emblem", GameObject.Find("Canvas/Profile/Profile Bar/Rank Emblem"));
		profileUI.Add("Username", GameObject.Find("Canvas/Profile/Profile Bar/Username"));
		profileUI.Add("Level", GameObject.Find("Canvas/Profile/Profile Bar/Level"));
		
		//general profile objects
		profileUI.Add("CurrentExp", GameObject.Find("Canvas/Profile/CurrentExp"));
		profileUI.Add("ProgressBar", GameObject.Find("Canvas/Profile/ProgressBarBackground/ProgressBar"));
		profileUI.Add("RequiredExp", GameObject.Find("Canvas/Profile/RequiredExp"));
		profileUI.Add("Vlads", GameObject.Find("Canvas/Profile/Vlads Panel/Vlads"));

		//Change the text to be correct
		profileUI["Username"].GetComponent<Text>().text = Username;
		//TODO: Work on later
		profileUI["Level"].GetComponent<Text>().text = "Lvl: ";

		profileUI["Vlads"].GetComponent<Text>().text = Vlads + " vlads";

	}

	void OnVladsChanged (int value) {
		totalVladsEarned += value; //add to the total amount gained from betting

		PlayerPrefs.SetInt("vlads", value); //save vlads value in PlayerPrefs
		profileUI["Vlads"].GetComponent<Text>().text = vlads + " vlads";
	}

	public string Username {
		get {return username;}
		set {
			PlayerPrefs.SetString("username", value);
			profileUI["Username"].GetComponent<Text>().text = value;
			username = value;
		}
	}

	public int Vlads {
		get {return vlads;}
		set {
			vlads = value;
			OnVladsChanged(value);
		}
	}

}
