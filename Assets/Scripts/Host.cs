using com.epicface.vodkabets.crash;
using com.epicface.vodkabets.crash.markers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace com.epicface.vodkabets.networking {

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
				CrashBetHandler.DisableBetting();
				CrashBetHandler.EnableWithdraw();
				
				//start the round
				string gameSeed = RandomHashUtil.GenerateHash();

				//reset marker values
				SecondsMarker.ResetValues();
				MultiplierMarker.ResetValues();

				//instantiate marker spawners
				RectTransform sms = Instantiate(SecondsMakerSpawner);
				sms.SetParent(GameObject.Find("Canvas/Graph").transform);
				sms.anchoredPosition3D = Vector3.zero;
				sms.localScale = Vector3.one;
				RectTransform mms = Instantiate(MultiplierMarkerSpawner);
				mms.SetParent(GameObject.Find("Canvas/Graph").transform);
				mms.anchoredPosition3D = Vector3.zero;
				mms.localScale = Vector3.one;

				//instantiate crash arrow
				Transform cr = Instantiate(CrashArrow);
				cr.GetComponent<CrashGrapher>().seed = gameSeed;

				//wait until the grapher is killed
				yield return new WaitUntil(() => cr == null);
			}

		}

	}

}
