using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace com.epicface.vodkabets.crash {

	public class CrashBetEntryUtility : NetworkBehaviour {

		[SerializeField] RectTransform entryTemplate;
		[SerializeField] RectTransform container;

		static RectTransform et;
		static RectTransform ct;

		static CrashBetEntryUtility instance;

		static Dictionary<int, RectTransform> entries = new Dictionary<int, RectTransform>();
		static int numEntries;

		//static variable initializer
		void Awake () {
			et = entryTemplate;
			ct = container;

			instance = this;
		}

		public static int CreateEntry (string Username, int bet) {
			//spawn entry and set the parent to be the scroll container
			RectTransform entry = Instantiate(et);
			entry.SetParent(ct);
			entry.localPosition = new Vector3 (0, -27.8f*numEntries-13.9f, 0); //the y offset is so the entry is not off screen & is in the list
			numEntries++;														 //the x offset is so the entry is not off screen

			//change the size of the container to match the amount of entries

			//340 is the width of the container (static)
			//y is the height of the entryTemplate (27.8f) multiplied by the amount of entries
			ct.sizeDelta = new Vector2 (340, 27.8f * numEntries);

			//get text array
			Text[] entryText = getTextArrayFromEntry(entry);

			//set text to proper stuff
			entryText[0].text = Username;
			entryText[1].text = "-";
			entryText[2].text = bet.ToString();
			entryText[3].text = "--";

			//add entry to list and return index
			entries.Add(entries.Count, entry);
			return entries.Count-1;
		}

		public static void WithdrawEntry (int index, int bet, float multiplier) {
			RectTransform entry = entries[index];

			//get text array
			Text[] entryText = getTextArrayFromEntry(entry);

			//set text to bet exit multiplier and calculate profit
			entryText[1].text = string.Format("{0:0.00}", multiplier) + "x";
			entryText[3].text = Mathf.Round(bet * multiplier).ToString();

			//change color of panel
			ChangePanelColor(entry, new Color32(0, 250, 0, 150));
		}

		public static void OnCrash () {
			//loop through each entry to deal with it
			for (int i = 0; i < numEntries; i++) {
				//get values from dictionary
				RectTransform entry = entries[i];
				Text[] entryText = getTextArrayFromEntry(entry);

				//check if entry has been withdrawn, if so change the color of it
				if (entryText[1].text == "-" || entryText[3].text == "--") {
					//entry has not been withdrawn
					ChangePanelColor(entry, new Color32 (250, 0, 0, 150));
				}
			}

			//reset entries after delay (5f)
			instance.Invoke("ResetEntries", 5f);
		}

		void ResetEntries () {
			//destroy all instances of entryBet
			for (int i = 0; i < numEntries; i++) {
				Destroy (entries[i].gameObject);
			}

			//clear dictionary
			entries = new Dictionary<int, RectTransform>();
			numEntries = 0;

			// resize container
			ct.sizeDelta = new Vector2 (340, 0);
		}

		public static void ChangePanelColor (RectTransform entry, Color32 c) {
			entry.GetComponentInChildren<Image>().color = c;
		}

		static Text[] getTextArrayFromEntry (RectTransform entry) {
			Text[] entryText = new Text[entry.childCount-1];
			for (int i = 1; i < entry.childCount; i++) {
				entryText[i-1] = entry.GetChild(i).GetComponent<Text>();
			}
			return entryText;
		}
	}

}
