using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UserSettings : MonoBehaviour {
	
    [SerializeField] GameObject[] settingFields;
    Client player;

    void Update () {
        //force loop until client found
        if (player == null) {
			GameObject go = GameObject.FindGameObjectWithTag("LocalPlayer");
            if (go != null) {
                player = go.GetComponent<Client>();
            }
			return;
		}
    }

    public void ResetAccount () {
        if (player == null) {
            return;
        }

        PlayerPrefs.DeleteAll();
        player.OnStartLocalPlayer();
    }

    public void SaveChanges () {
        if (player == null) {
            return;
        }

        if (string.IsNullOrEmpty(settingFields[0].GetComponent<InputField>().text)) {
            //Generate Random Username
			System.Guid guid = System.Guid.NewGuid();
			string name = System.Convert.ToBase64String(guid.ToByteArray());
			name = name.Replace("=", "");
			name = name.Replace("+", "");

            player.Username = name;
        } else {
            player.Username = settingFields[0].GetComponent<InputField>().text;
        }
    }

}