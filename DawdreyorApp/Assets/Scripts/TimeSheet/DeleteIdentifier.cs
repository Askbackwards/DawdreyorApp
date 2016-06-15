using UnityEngine;
using System.Collections;

public class DeleteIdentifier : MonoBehaviour {

	private int identity;

	// Use this for initialization
	void Start () {
	
	}

	// Update is called once per frame
	void Update () {
	
	}

	public void Name(int name) {
		identity = name;
	}

	public int ReturnName() {
		return identity;
	}
}
