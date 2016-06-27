using UnityEngine;
using System.Collections;

public class WorkIDHandler : MonoBehaviour {

	private string ID;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void setID(string toBe) {
		ID = toBe;
	}

	public string getID() {
		return ID;
	}
}
