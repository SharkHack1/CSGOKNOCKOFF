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
	public float multiplier;
	public float timeElapsed;
	public float crashMulitplier;
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

		//TODO: Make it have a chance to crash every frame
		//Generate Crash Value (out of 1 million)
		Random.InitState(seed.GetHashCode());
		crashMulitplier = Random.Range(1f, crashLimit);
	}
	
	// Update is called once per frame
	void Update () {
		
		//update time
		timeElapsed += Time.deltaTime;

		//TODO: OPTIMIZE
		//update each graph point for scale and adding new ones
		for (int i = 1; i < lr.numPositions; i++) {
			if (i == lr.numPositions-1) {
				if (timeElapsed >= lr.numPositions) {
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
		graphDisplay.text = string.Format("{0:0.00}", System.Math.Round(Mathf.Pow(.1f*timeElapsed, 2)+1, 2)) + "x";
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

}
