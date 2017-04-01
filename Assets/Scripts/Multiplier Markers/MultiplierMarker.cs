using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class MultiplierMarker : NetworkBehaviour {

	const float graphHeight = 450;
	static int numMarkers;
	static int numActiveMarkers;
	static float distance;
	RectTransform rectTransform;
	int multiplier;
	static int multiplierBase = 1;

	void Start () {
		numMarkers++;
		numActiveMarkers++;
		multiplier = numMarkers;

		transform.localPosition = Vector3.zero;

		//Up the base by 10 when there are too many markers
		if (numActiveMarkers >= 4) {
			multiplierBase *= 5;
		}
		GetComponentInChildren<Text>().text = (multiplier+1) + "x";
		rectTransform = GetComponent<RectTransform>();
	}

	void Update () {
		if (multiplier % multiplierBase != 0) {
			Destroy(this.gameObject);
			numActiveMarkers--;
		} else {
			GetComponent<Image>().enabled = true;
			GetComponentInChildren<Text>().enabled = true;
		}

		Vector3 pos = rectTransform.anchoredPosition3D;
		pos.x = 0;
		pos.y = Mathf.Lerp(pos.y, -getUnlerpedDistance() * (numMarkers-multiplier), Time.deltaTime);
		rectTransform.anchoredPosition3D = pos;
	}

	public static float getUnlerpedDistance () {
		return graphHeight/numMarkers;
	}

	public static float getLerpedDistance () {
		distance = Mathf.Lerp(distance, getUnlerpedDistance(), Time.deltaTime);
		return distance;
	}

}
