using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Host : NetworkBehaviour {
    [SerializeField] Transform CrashArrow;
	[SerializeField] RectTransform SecondsMakerSpawner;
	[SerializeField] RectTransform MultiplierMarkerSpawner;
	Text MultiplierText;

	void Start () {
		MultiplierText = GameObject.Find("Canvas/Graph/Multiplier").GetComponent<Text>();
		StartCoroutine(InstantiateRound());
	}

	void StartRound () {
		string gameSeed = GenerateSeed();

		//instantiate marker spawners
		RectTransform sms = Instantiate(SecondsMakerSpawner);
		sms.SetParent(GameObject.Find("Canvas/Graph").transform);
		sms.anchoredPosition3D = new Vector3 (0, 160, 0); //offset so seconds dont look weird
		RectTransform mms = Instantiate(MultiplierMarkerSpawner);
		mms.SetParent(GameObject.Find("Canvas/Graph").transform);
		mms.anchoredPosition3D = Vector3.zero;

		//instantiate crash arrow
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

	IEnumerator InstantiateRound () {
		//count down until 0
		float timeTillRoundStart = 7.5f;
		
		while (timeTillRoundStart > 0) {
			timeTillRoundStart -= Time.deltaTime;
			MultiplierText.text = "Start Betting!\n" + string.Format("{0:0.00}", timeTillRoundStart) + "s until next round...";
			yield return null;
		}

		BetHandler.DisableBetting();
		BetHandler.EnableWithdraw();
		StartRound();
	}

	public void InstantiateRoundCoroutine() {
		StartCoroutine(InstantiateRound());
	}
}
