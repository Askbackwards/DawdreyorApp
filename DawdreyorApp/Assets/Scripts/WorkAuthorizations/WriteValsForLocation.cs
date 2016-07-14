using UnityEngine;
using System.Collections;
using System.IO;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WriteValsForLocation : MonoBehaviour {

	public GameObject[] inputs; 

	private string txtPath, customerName, propertyAddress, menInCrew, city, state, zip;
	private int whichVal;

	// Use this for initialization
	void Start () {
		whichVal = 0;
		customerName = "";
		propertyAddress = "";
		menInCrew = "";
		city = "";
		state = "";
		zip = "";

		txtPath = Application.persistentDataPath + "/" + PlayerPrefs.GetString ("WOID") + "_Info.txt";
		//txtPath = "C:/Users/nomore/Desktop/" + PlayerPrefs.GetString ("WOID") + "_Info.txt";

		if (File.Exists (txtPath)) {
			string[] oldText = File.ReadAllLines (txtPath);
			if (oldText [0].Contains ("Customer")) {
				for (int i = 0; i < 6; i++) {
					string[] splitText = oldText [i].Split (",".ToCharArray());
					inputs [i].GetComponentInChildren<Text> ().text = splitText [1];
					switch (i) {
					case(0):
						customerName = splitText[1];
						break;
					case(1):
						propertyAddress = splitText[1];
						break;
					case(2):
						menInCrew = splitText[1];
						break;
					case(3):
						city = splitText[1];
						break;
					case(4):
						state = splitText[1];
						break;
					case(5):
						zip = splitText[1];
						break;
					}
				}
			}
		}

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	//Determine what value the input will be saved as
	public void whatValue(string valueName) {
		if (valueName == "CustomerName")
			whichVal = 1;
		else if (valueName == "PropertyAddress")
			whichVal = 2;
		else if (valueName == "MenInCrew")
			whichVal = 3;
		else if (valueName == "City")
			whichVal = 4;
		else if (valueName == "State")
			whichVal = 5;
		else if (valueName == "Zip")
			whichVal = 6;
	}

	//---------------------------------------------------------------------------------------------------------------------

	//temp store that value for later writing
	public void SaveValue(string value) {
		switch (whichVal) {
		case(1):
			customerName = value;
			break;
		case(2):
			propertyAddress = value;
			break;
		case(3):
			menInCrew = value;
			break;
		case(4):
			city = value;
			break;
		case(5):
			state = value;
			break;
		case(6):
			zip = value;
			break;
		}
	}

	//---------------------------------------------------------------------------------------------------------------------

	//Write to a File
	public void Submit() {

		//Make file if it doesn't exsist
		if (!File.Exists (txtPath))
			File.Create (txtPath ).Dispose ();

		string[] oldText = File.ReadAllLines(txtPath);

		//Create the values to help format the file
		string[] format = { "CustomerName", "PropertyAdress", "MenInCrew", "City", "State", "Zip", "Date", "Employee" }; 

		string date = System.DateTime.Now.Date.ToString ();
		string[] newDate = date.Split (" ".ToCharArray ());

		string employee = PlayerPrefs.GetString ("Username");

		string[] localVals = { customerName, propertyAddress, menInCrew, city, state, zip, newDate[0], employee };

		//OpenFile
		StreamWriter streamW = new StreamWriter(txtPath);

		//Write new stuff
		for (int i = 0; i < 8; i++) {

			streamW.WriteLine(format[i] + "," + localVals[i]);

		}

		//write old stuff
		if (oldText.Length > 0) {
			for (int i = 0; i < oldText.Length; i++) {
				streamW.WriteLine (oldText [i]);
			}
		}

		streamW.Flush ();
		streamW.Close ();

		SceneManager.LoadScene ("NewWorkAuthorizationMenu");

	}
}
