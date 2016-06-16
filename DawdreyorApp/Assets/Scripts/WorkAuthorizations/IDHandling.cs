using UnityEngine;
using System.Collections;

public class IDHandling : MonoBehaviour {

	private string ID;

	void Start () {
	
	}

	void Update () {
	
	}

	public void SetID(string WONum) {
		ID = WONum;
	}

	public void SetPPWithID() {
		PlayerPrefs.SetString ("WOID", ID);
	}
}
