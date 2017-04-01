using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Host : NetworkBehaviour {

	[SerializeField] Transform CrashArrow;

	void Start () {
		StartRound();
	}

	public void StartRound () {
		string gameSeed = GenerateSeed();

		Transform cr = Instantiate(CrashArrow);
		cr.GetComponent<CrashGrapher>().seed = gameSeed;
	}

	string GenerateSeed () {
		System.Guid guid = System.Guid.NewGuid();
		string hash = System.Convert.ToBase64String(guid.ToByteArray());
		hash = hash.Replace("=", "");
		hash = hash.Replace("+", "");
		return hash;
	}

}
