using UnityEngine;
using System.Collections;

public class AssertionFailureException : UnityException {
	public AssertionFailureException() : base("Assertion failure!") { }
	public AssertionFailureException(string why) : base(why) { }
}

public class Assert {
	public static void assert(bool condition) {
		if (condition == false) throw new AssertionFailureException();
	}
	
	public static void assert(bool condition, string message) {
		if (condition == false) throw new AssertionFailureException(message);
	}
	
	public static void debug_assert(bool condition, string message) {
		if (Debug.isDebugBuild && condition == false) throw new AssertionFailureException(message);
	}
}