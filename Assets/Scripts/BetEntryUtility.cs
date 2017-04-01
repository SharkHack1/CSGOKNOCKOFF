using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class BetEntryUtility : NetworkBehaviour {

	[SerializeField] RectTransform entryTemplate;
	[SerializeField] RectTransform container;

	static RectTransform et;
	static RectTransform ct;

	static int numEntries;

	//static variable initializer
	void Awake () {
		et = entryTemplate;
		ct = container;
	}

	public static void CreateEntry (string Username, int bet) {
		//spawn entry and set the parent to be the scroll container
		RectTransform entry = Instantiate(et);
		entry.SetParent(ct);
		entry.localPosition = new Vector3 (0, -27.8f*numEntries-13.9f, 0); //the y offset is so the entry is not off screen & is in the list
		numEntries++;														 //the x offset is so the entry is not off screen

		//change the size of the container to match the amount of entries

		//340 is the width of the container (static)
		//y is the height of the entryTemplate (27.8f) multiplied by the amount of entries
		ct.sizeDelta = new Vector2 (340, 27.8f * numEntries);

	}

}
