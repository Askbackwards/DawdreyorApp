using UnityEngine;
using System.Collections;

public class SaveEstimatePP : MonoBehaviour {

	//TODO: make sure to add error so they can't enter nothing

	// Use this for initialization
	void Start () {
		PlayerPrefs.SetInt ("picAmount2", 0);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void savePP(string num) {
		PlayerPrefs.SetString ("EstimateNumber", num);
	}
}
