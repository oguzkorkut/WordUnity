using UnityEngine;
using System.Collections;
using WordShufflerConsoleTest;

public class GenerateProgram : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Program.Instance.Generate();
	}

}
