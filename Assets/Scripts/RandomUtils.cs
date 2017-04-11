using UnityEngine;

namespace com.epicface.vodkabets {

	public static class RNGUtil {
		public static float GenerateWeightedCrashPoint (Condition[] conditions, string seed) {
			//loop through all conditions and add them to an array according to likelyhood of being chosen
			//if you got a index out of range error, the %s didnt like up to 100
			int[] weightedIndexes = new int[100];

			int originOffset = 0; //offset so weightedIndexes array values don't get overridden
			for (int ci = 0; ci < conditions.Length; ci++) {
				Condition c = conditions[ci];
				//add amount of %s to weightedArray
				for (int i = 0; i < c.precent; i++) {
					weightedIndexes[originOffset+i] = ci; 
				}
			}

			//set up rnd
			Random.InitState(seed.GetHashCode());

			//pick random condition from weighted array
			Condition chosenOne = conditions[weightedIndexes[Random.Range(0,99)]];

			//generate crash value from condition
			//multiply each # by 100 and then divide for crash float
			float a = chosenOne.min;
			float b = chosenOne.max;
			return Random.Range(a, b);
		}
	}

	public static class RandomHashUtil {
		public static string GenerateHash () {
			System.Guid guid = System.Guid.NewGuid();
			string hash = System.Convert.ToBase64String(guid.ToByteArray());
			hash = hash.Replace("=", "");
			hash = hash.Replace("+", "");
			return hash;
		}
	}

	[System.Serializable]
	public struct Condition {
		public float min;
		public float max;
		public int precent; //actual % not 0-1
	}

}
