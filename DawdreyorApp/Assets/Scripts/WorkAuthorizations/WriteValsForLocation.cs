using UnityEngine;
using System.Collections;
using System.IO;
using UnityEngine.SceneManagement;

public class WriteValsForLocation : MonoBehaviour {

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

		//txtPath = Application.persistentDataPath + PlayerPrefs.GetString ("WOID") + "_Info.txt";
		txtPath = "C:/Users/nomore/Desktop/" + PlayerPrefs.GetString ("WOID") + "_Info.txt";

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

		//Create the values to help format the file
		string[] format = { "CustomerName", "PropertyAdress", "MenInCrew", "City", "State", "Zip", "Date" }; 

		string date = System.DateTime.Now.Date.ToString ();

		string[] localVals = { customerName, propertyAddress, menInCrew, city, state, zip, date };

		//OpenFile
		StreamWriter streamW = new StreamWriter(txtPath);

		//Write new stuff
		for (int i = 0; i < 7; i++) {

			streamW.WriteLine(format[i] + "," + localVals[i] + "\n");

		}

		streamW.Flush ();
		streamW.Close ();

		SceneManager.LoadScene ("NewWorkAuthorizationMenu");

	}
}
