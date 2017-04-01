using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Client : NetworkBehaviour {

	public string username;
	int vlads;
	int totalVladsEarned;

	private const int startingVlads = 100;

	[SerializeField] Transform hostObj;

	Hashtable profileUI = new Hashtable();


	void Start () {
		if (true) {
			Instantiate(hostObj);
		}
	}	

	void Update () {

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

		//map all the profile bar objects
		profileUI.Add("Rank Emblem", GameObject.Find("Canvas/Profile/Profile Bar/Rank Emblem").GetComponent<Image>());
		profileUI.Add("Username", GameObject.Find("Canvas/Profile/Profile Bar/Username").GetComponent<Text>());
		profileUI.Add("Level", GameObject.Find("Canvas/Profile/Profile Bar/Level").GetComponent<Text>());
		
		//general profile objects
		profileUI.Add("CurrentExp", GameObject.Find("Canvas/Profile/CurrentExp").GetComponent<Text>());
		profileUI.Add("ProgressBar", GameObject.Find("Canvas/Profile/ProgressBarBackground/ProgressBar").GetComponent<RectTransform>());
		profileUI.Add("RequiredExp", GameObject.Find("Canvas/Profile/RequiredExp").GetComponent<Text>());
		profileUI.Add("Vlads", GameObject.Find("Canvas/Profile/Vlads").GetComponent<Text>());

		//Change the text to be correct
		profileUI["Username"] = username;
		//TODO: Work on later
		profileUI["Level"] = "Lvl: ";

		profileUI["Vlads"] = vlads + " vlads";

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
