using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.epicface.vodkabets {

	public static class RNGUtil {

	}

	public enum ConditionType {
		Above,Equals,Below,Within
	}

	[System.Serializable]
	public class Condition {
		public ConditionType ct;
		public float[] conditions;
		public float precent;
	}

}
