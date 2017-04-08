using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class CrashGrapher : NetworkBehaviour {

	LineRenderer lr;
	Transform arrow;
	Text graphDisplay;
	[SerializeField] Vector3 OriginOffset;
	[SerializeField] float graphStartY;
	[SyncVar] public string seed = "Ruski Spy";
	public static float multiplier;
	public float timeElapsed;
	int[] crashFraction;
	System.Random rnd;
	[SerializeField] bool willCrash = false;
	[SerializeField] bool crashed = false;
	[SerializeField] float crashLimit;

	//Graph Formula y = (.1x)^2+1
	//y = multiplier
	//x = seconds elapsed

	// Use this for initialization
	void Start () {
		//get components
		lr = GetComponent<LineRenderer>();
		graphDisplay = GameObject.Find("Canvas/Graph/Multiplier").GetComponent<Text>();
		arrow = transform.FindChild("arrow");

		//set up crash vars
		crashFraction = new int[2];
		crashFraction[0] = 5;
		crashFraction[1] = 100;

		rnd = new System.Random(seed.GetHashCode());

		//Set-up chance to crash on startup
		if (crashFraction[0] >= rnd.Next(1, crashFraction[1])) {
			crashed = true;
			StartCoroutine(Crash());
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (crashed) {
			return;
		}
		
		if (willCrash) {
			if (crashFraction[0] >= rnd.Next(1, crashFraction[1])) {
				//graph crashed
				crashed = true;
				StartCoroutine(Crash());
			}
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

					//handle crash stuff

					//the numerator increases 1 and denominator increases 2
					crashFraction[0]++;
					crashFraction[1] += 2;

					if (crashFraction[0] <= rnd.Next(1, crashFraction[1])) {
						willCrash = true;
					}

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
		//destroy multiplier spawner and seconds spawner
		foreach (Spawner s in GameObject.Find("Canvas/Graph").GetComponentsInChildren<Spawner>()) {
			Destroy(s);
		}

		//handle rest of crash stuff
		lr.gameObject.SetActive(false);

		//count down until 0
		float timeTillRoundStart = 5f;
		
		while (timeTillRoundStart > 0) {
			timeTillRoundStart -= Time.deltaTime;
			graphDisplay.text = "Crashed @ " + string.Format("{0:0.00}", multiplier) + "x\n" +
			 string.Format("{0:0.00}", timeTillRoundStart) + "s until next round...";
			yield return null;
		}

		BetHandler.ResetBetPanel();

		if (isServer) {
			FindObjectOfType<Host>().InstantiateRoundCoroutine();
		}
	}

}
