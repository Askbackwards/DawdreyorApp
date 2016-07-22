using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.IO;
	
public class SubmitEstimate : MonoBehaviour {

	public GameObject[] parents;
	public GameObject confirmText, popUp;

	private string txtPath2;
	private int curInputs, emptyCount, skipCount;
	private string[] inputs, empties;
	private bool[] inputsDone;

	// Use this for initialization
	void Start () {
		//Assign Values!
		txtPath2 = Application.persistentDataPath + "/" + PlayerPrefs.GetString("EstimateNumber") + "_Estimate.txt";
		curInputs = 16;
		emptyCount = 0;
		int tempNum = 0;
		inputs = new string[100];
		empties = new string[100];

		for (int i = 0; i < parents.Length; i++) {
			foreach (Transform child in parents[i].transform) {
				if (child.name.Contains("Input") || child.name.Contains("Drop")) {
					inputs [tempNum] = child.name;
					tempNum += 1;
				}
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	//Add number of inputs if an object is added
	public void AddInputs(int amountAdded) {
		curInputs += amountAdded;
	}

	//Send the estimate txt
	public void SendIt() {
		//Get Old text
		string[] oldText = File.ReadAllLines(txtPath2);

		//read through it
		for (int i = 0; i <= (curInputs - skipCount); i++) {
			if (oldText.Length <= i) {
				empties [emptyCount] = inputs [i];
				emptyCount += 1;
			} else if(oldText[i + skipCount].CompareTo(inputs[i]) != -1) {
				empties[emptyCount] = inputs[i];
				emptyCount += 1;
				skipCount += 1;
			} 
		}

		popUp.SetActive (true);

		//Tell what they missed
		if (emptyCount > 0) {
			string textToEnter = "You have not filled out these items:\n\n";
			for (int i = 0; i <= emptyCount; i++) {
				textToEnter += empties [i] + "\n";
			}
			textToEnter += "\nWould you like to continue?";
			confirmText.GetComponent<Text> ().text = textToEnter;
		}
	}
		
}
