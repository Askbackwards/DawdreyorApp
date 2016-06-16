using UnityEngine;
using System.Collections;

public class DeActivate : MonoBehaviour {

	public GameObject objToTurnOff;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void TurnOff() {
		objToTurnOff.SetActive (false);
	}
}
