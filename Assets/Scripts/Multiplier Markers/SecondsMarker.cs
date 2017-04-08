using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class SecondsMarker : NetworkBehaviour {

	const float graphLegnth = 900;
	static int numMarkers;
	static int numActiveMarkers;
	static float distance;
	RectTransform rectTransform;
	int seconds;
	static int secondsBase = 1;

	void Start () {
		numMarkers++;
		numActiveMarkers++;
		seconds = numMarkers;

		//Up the base by 10
		if (numActiveMarkers >= 20) {
			secondsBase *= 10;
		}
		GetComponent<Text>().text = seconds + "s";
		rectTransform = GetComponent<RectTransform>();
	}

	void Update () {
		if (seconds % secondsBase != 0) {
			Destroy(this.gameObject);
			numActiveMarkers--;
		} else {
			GetComponent<Text>().enabled = true;
		}

		Vector3 pos = rectTransform.anchoredPosition3D;
		pos.x = getDistance() * seconds;
		pos.y = -475;
		rectTransform.anchoredPosition3D = pos;
	}

	public static float getDistance () {
		 distance = Mathf.Lerp(distance, getUnLerpedDistance(), Time.deltaTime);
		 return distance;
	}

	public static float getUnLerpedDistance () {
		return graphLegnth/numMarkers;
	}

	public static void ResetValues () {
		numMarkers = new int();
		numActiveMarkers = new int();
		distance = new int();
		secondsBase = 1;
	}

}
