using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Host : NetworkBehaviour {
    [SerializeField] Transform CrashArrow;
	[SerializeField] RectTransform SecondsMakerSpawner;
	[SerializeField] RectTransform MultiplierMarkerSpawner;
	[SerializeField] bool enableGameLoop = true;
	Text MultiplierText;

	void Start () {
		MultiplierText = GameObject.Find("Canvas/Graph/Multiplier").GetComponent<Text>();
		StartCoroutine(GameLoop());
	}

	string GenerateSeed () {
		System.Guid guid = System.Guid.NewGuid();
		string hash = System.Convert.ToBase64String(guid.ToByteArray());
		hash = hash.Replace("=", "");
		hash = hash.Replace("+", "");
		return hash;
	}

	IEnumerator GameLoop () {

		//forever loop unless game is stopped
		while (enableGameLoop) {

			//count down until 0
			float timeTillRoundStart = 7.5f;
			
			while (timeTillRoundStart > 0) {
				timeTillRoundStart -= Time.deltaTime;
				MultiplierText.text = "Start Betting!\n" + string.Format("{0:0.00}", timeTillRoundStart) + "s until next round...";
				yield return null;
			}

			//disable bettin and enable withdraw
			BetHandler.DisableBetting();
			BetHandler.EnableWithdraw();
			
			//start the round
			string gameSeed = GenerateSeed();

			//reset marker values
			SecondsMarker.ResetValues();
			MultiplierMarker.ResetValues();

			//instantiate marker spawners
			RectTransform sms = Instantiate(SecondsMakerSpawner);
			sms.SetParent(GameObject.Find("Canvas/Graph").transform);
			sms.localPosition = Vector3.zero;
			RectTransform mms = Instantiate(MultiplierMarkerSpawner);
			mms.SetParent(GameObject.Find("Canvas/Graph").transform);
			mms.localPosition = Vector3.zero;

			//instantiate crash arrow
			Transform cr = Instantiate(CrashArrow);
			cr.GetComponent<CrashGrapher>().seed = gameSeed;

			//wait until the grapher is killed
			yield return new WaitUntil(() => cr == null);
		}

	}

}
