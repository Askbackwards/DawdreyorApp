using UnityEngine;
using System.Collections;

public class SaveEstimatePP : MonoBehaviour {

	//TODO: make sure to add error so they can't enter nothing

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void savePP(string num) {
		PlayerPrefs.SetString ("EstimateNumber", num);
		PlayerPrefs.SetInt (("picAmount2" + num), 0);
		PlayerPrefs.SetInt ("lighting" + num, 0);
		PlayerPrefs.SetInt ("entryDoor" + num, 0);
		PlayerPrefs.SetInt ("rollUp" + num, 0);
		PlayerPrefs.SetInt ("window" + num, 0);
		PlayerPrefs.SetInt ("sheet" + num, 0);
		PlayerPrefs.SetInt ("misc" + num, 0);
	}
}
