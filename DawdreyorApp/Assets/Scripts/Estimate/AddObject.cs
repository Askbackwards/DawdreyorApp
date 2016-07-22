using UnityEngine;
using System.Collections;

public class AddObject : MonoBehaviour {

	public GameObject codeHolder, contentPane, lightElec, entry, rollUp, window, sheet, misc;

	private int choice;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	//store what choice they picked
	public void setChoice(int pick) {
		choice = pick;
	}

	//Start the duplication method
	public void DupEm() {
		switch (choice) {
		case(0):
			duPros (lightElec);
			break;
		case(1):
			duPros (entry);
			break;
		case(2):
			duPros (rollUp);
			break;
		case(3):
			duPros (window);
			break;
		case(4):
			duPros (sheet);
			break;
		case(5):
			duPros (misc);
			break;
		}
	}

	//Duplicate objects
	public void duPros(GameObject copyParent) {
		//Create the new object
		foreach (Transform child in copyParent.transform) {
			GameObject tempObj = (GameObject)Instantiate (child.gameObject);
			tempObj.transform.SetParent (contentPane.transform);
			tempObj.transform.localScale = new Vector3 (1, 1, 1);
			tempObj.SetActive (true);
			//Add new inputs to count
			if (tempObj.name.Contains("Input") || tempObj.name.Contains("Drop"))
				codeHolder.GetComponent<SubmitEstimate> ().AddInputs (1);
		}
	}
}
