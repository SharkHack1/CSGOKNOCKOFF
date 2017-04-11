using com.epicface.vodkabets.crash.markers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace com.epicface.vodkabets.crash {

	public class CrashGrapher : NetworkBehaviour {

		LineRenderer lr;
		Transform arrow;
		Text graphDisplay;
		[SerializeField] Vector3 OriginOffset;
		[SerializeField] float graphStartY;
		[SyncVar] public string seed = "Ruski Spy";
		public static float multiplier;
		public float timeElapsed;
		public bool crashed;
		public float crashMultiplier;
		[SerializeField] TextAsset crashValues;
		[SerializeField] int omittedLines;
		static Condition[] crashValueChooserConditions;

		//Graph Formula y = (.1x)^2+1
		//y = multiplier
		//x = seconds elapsed

		// Use this for initialization
		void Start () {
			//get components
			lr = GetComponent<LineRenderer>();
			graphDisplay = GameObject.Find("Canvas/Graph/Multiplier").GetComponent<Text>();
			arrow = transform.FindChild("arrow");

			//generate random crash float

			//get date from file if needed
			if (crashValueChooserConditions == null) {
				string[] lines = crashValues.text.Split(new char[] {'\n'}, System.StringSplitOptions.RemoveEmptyEntries);

				crashValueChooserConditions = new Condition[lines.Length-omittedLines];
				for (int i = 0; i < lines.Length-omittedLines; i++) {
					string[] condition = lines[i+omittedLines].Split(',');
					//add values to condition array
					crashValueChooserConditions[i].min = float.Parse(condition[0]);
					crashValueChooserConditions[i].max = float.Parse(condition[1]);
					crashValueChooserConditions[i].precent = int.Parse(condition[2]);
				}
				
			}

			crashMultiplier = RNGUtil.GenerateWeightedCrashPoint(crashValueChooserConditions, seed);

			if (crashMultiplier == 0f) {
				crashed = true;
				StartCoroutine(Crash());
			}
		}
		
		// Update is called once per frame
		void Update () {
			//check if crashed
			if (crashed) {
				return;
			}

			if (multiplier >= crashMultiplier) {
				crashed = true;
				StartCoroutine(Crash());
			}

			//update time
			timeElapsed += Time.deltaTime;

			//TODO: OPTIMIZE
			//update each graph point for scale and adding new ones
			for (int i = 1; i < lr.numPositions; i++) {
				if (i == lr.numPositions-1) {
					if (timeElapsed >= lr.numPositions) {
						//new point
						lr.numPositions++;
						lr.SetPosition(i, GetPointPosition(i));
						lr.SetPosition(i+1, GetPointPosition(timeElapsed));
					} else {
						lr.SetPosition(i, GetPointPosition(timeElapsed));
					}

					//set the arrow position and rotation
					Vector3 pointA = lr.GetPosition(i);
					Vector3 pointB = lr.GetPosition(i-1);

					float angle = Mathf.Atan2(pointB.y - pointA.y, pointB.x - pointA.x) * Mathf.Rad2Deg + 90f; //90f is the offset in degrees

					arrow.position = pointA;
					arrow.rotation = Quaternion.Euler(0, 0, angle);

				} else {
					lr.SetPosition(i, GetPointPosition(i));
				}
			}

			//display multiplier
			multiplier = Mathf.Pow(.1f*timeElapsed, 2)+1;
			graphDisplay.text = string.Format("{0:0.00}", multiplier) + "x";
		}

		Vector3 GetPointPosition (float i) {
			//100 pixels is one unit in world space
			Vector3 point = Vector3.zero;

			//set coords
			point.x = i * SecondsMarker.getDistance();
			point.y = Mathf.Pow(.1f*i, 2) * MultiplierMarker.getLerpedDistance();
			point.z = 0;
			point += OriginOffset*100;

			//force y to be non-negative
			point.y = Mathf.Max(point.y, graphStartY * 100);

			//convert to world space
			point /= 100;

			return point;
		}

		IEnumerator Crash () {

			//crash entries
			CrashBetEntryUtility.OnCrash();

			//destroy multiplier spawner and seconds spawner
			foreach (Spawner s in GameObject.Find("Canvas/Graph").GetComponentsInChildren<Spawner>()) {
				Destroy(s.gameObject);
			}

			//hide arrow
			arrow.gameObject.SetActive(false);

			//handle rest of crash stuff
			lr.enabled = false;
			CrashBetHandler.DisableBetting();

			//count down until 0
			float timeTillRoundStart = 5f;
			
			while (timeTillRoundStart > 0) {
				timeTillRoundStart -= Time.deltaTime;
				graphDisplay.text = "Crashed @ " + string.Format("{0:0.00}", multiplier) + "x\n" +
				string.Format("{0:0.00}", timeTillRoundStart) + "s until next round...";
				yield return null;
			}

			//prepare for next round
			CrashBetHandler.ResetBetPanel();
			Destroy(this.gameObject);
		}
	}

}
