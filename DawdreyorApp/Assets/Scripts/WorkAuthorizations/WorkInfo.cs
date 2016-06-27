using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;

public class WorkInfo : MonoBehaviour {

	public GameObject[] duplicateList;
	public GameObject holder, contentPane;

	private List<GameObject> doneDuped;
	private string[] unitNum, measurement, workPerformed;
	private string[,] supplies, quantity;
	private int place, dupeMarker, whichVal, singleID, supplyID, quantityID;
	private bool noGo;
	private string txtPath;

	// Use this for initialization
	void Start () {
		singleID = 0;
		supplyID = 1;
		quantityID = 1;
		dupeMarker = 0;
		noGo = false;
		doneDuped = new List<GameObject> ();
		unitNum = new string[50];
		measurement = new string[50];
		workPerformed = new string[50];
		supplies = new string[50,50];
		quantity = new string[50,50];

		//txtPath = Application.persistentDataPath + PlayerPrefs.GetString ("WOID") + "_Info.txt";
		txtPath = "C:/Users/nomore/Desktop/" + PlayerPrefs.GetString ("WOID") + "_Info.txt";

		//Set ID
		duplicateList[1].GetComponent<WorkIDHandler>().setID("0");
		duplicateList[3].GetComponent<WorkIDHandler>().setID("0");
		duplicateList[5].GetComponent<WorkIDHandler>().setID("0");
		duplicateList[7].GetComponent<WorkIDHandler>().setID("0,0");
		duplicateList[9].GetComponent<WorkIDHandler>().setID("0,0");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	//---------------------------------------------------------------------------------------------------------------------

	//Add another unit for work
	public void AddUnit() {
		singleID += 1;
		for (int i = 0; i < duplicateList.Length; i++) {
			GameObject tempObj = Instantiate (duplicateList [i]);
			tempObj.transform.SetParent (contentPane.transform);
			tempObj.transform.localScale = new Vector3 (1, 1, 1);
			tempObj.GetComponent<RectTransform> ().position = new Vector3 (tempObj.GetComponent<RectTransform> ().position.x, tempObj.GetComponent<RectTransform> ().position.y, 0);
			if (i == 1 || i == 3 || i == 5) {
				tempObj.GetComponent<WorkIDHandler> ().setID (singleID + "");
			} else if (i == 5 || i == 7 || i == 9) {
				tempObj.GetComponent<WorkIDHandler> ().setID (singleID + ",0");
			}
		}
		quantityID = 0;
		supplyID = 0;
	}

	//---------------------------------------------------------------------------------------------------------------------

	public void NewPart(GameObject thisObj) {
		holder = thisObj;
		noGo = false;

		//Test to see if this object has already been duped
		if (dupeMarker > 0) {
			for (int i = 0; i < dupeMarker; i++) {
				if (thisObj == doneDuped [i])
					noGo = true;
			}
		}

		//Dupe it
		if (noGo == false) {
			GameObject tempObj = Instantiate (thisObj);
			tempObj.transform.SetParent (contentPane.transform);
			tempObj.transform.localScale = new Vector3 (1, 1, 1);
			tempObj.GetComponent<RectTransform> ().position = new Vector3 (tempObj.GetComponent<RectTransform> ().position.x, tempObj.GetComponent<RectTransform> ().position.y, 0);
			tempObj.transform.SetSiblingIndex (thisObj.transform.GetSiblingIndex () + 1);
			if (tempObj.name.Contains ("Quantity")) {
				tempObj.GetComponent<WorkIDHandler> ().setID (singleID + "," + quantityID);
				quantityID += 1;
			} else {
				tempObj.GetComponent<WorkIDHandler> ().setID (singleID + "," + supplyID);
				supplyID += 1;
			}
			doneDuped.Add (holder);
			dupeMarker += 1;
		}
	}

	//---------------------------------------------------------------------------------------------------------------------

	//Determine what value the input will be saved as
	public void whatValue(string valueName) {
		if (valueName == "UnitNum")
			whichVal = 1;
		else if (valueName == "Measurement")
			whichVal = 2;
		else if (valueName == "WorkPerformed")
			whichVal = 3;
		else if (valueName == "Supplies")
			whichVal = 4;
		else if (valueName == "Quantity")
			whichVal = 5;
	}

	//---------------------------------------------------------------------------------------------------------------------

	//temp store that value for later writing
	public void SaveValue(string value) {
		string ID;
		string[] splitID= {"n","n"};
		switch (whichVal) {
		case(1):
			ID = holder.GetComponent<WorkIDHandler> ().getID ();
			unitNum [int.Parse(ID)] = value;
			break;
		case(2):
			ID = holder.GetComponent<WorkIDHandler> ().getID ();
			measurement [int.Parse(ID)] = value;
			break;
		case(3):
			ID = holder.GetComponent<WorkIDHandler> ().getID ();
			workPerformed [int.Parse(ID)] = value;
			break;
		case(4):
			ID = holder.GetComponent<WorkIDHandler> ().getID ();
			splitID = ID.Split (",".ToCharArray (), 2);
			supplies [int.Parse(splitID[0]), int.Parse(splitID[1])] = value;
			break;
		case(5):
			ID = holder.GetComponent<WorkIDHandler> ().getID ();
			splitID = ID.Split (",".ToCharArray (), 2);
			quantity [int.Parse(splitID[0]), int.Parse(splitID[1])] = value;
			break;
		}
	}

	//---------------------------------------------------------------------------------------------------------------------

	public void setHolder(GameObject objToHold) {
		holder = objToHold;
	}

	//---------------------------------------------------------------------------------------------------------------------

	//Write to a File
	public void Submit() {

		//Make file if it doesn't exsist
		if (!File.Exists (txtPath))
			File.Create (txtPath ).Dispose ();

		string[] oldText = File.ReadAllLines(txtPath);

		//OpenFile
		StreamWriter streamW = new StreamWriter(txtPath);

		//Write old stuff
		if (oldText.Length > 0) {
			if (oldText [0].Contains ("CustomerName")) {
				for (int i = 0; i < 7; i++) {

					streamW.WriteLine (oldText [i]);

				}
			}
		}

		//Write new stuff
		for (int i = 0; i <= singleID; i++) {
			streamW.WriteLine (unitNum [i] + "," + measurement[i] + "," + workPerformed[i]  + "," + supplies[i,0] + "," + quantity[i,0]);
			for (int l = 1; l < supplies.GetLength(i); l++) {
				if (supplies [i, l] == null) { 
					break;
				}
				streamW.WriteLine (",,," + supplies [i, l] + "," + quantity [i, l]);
			}
		}

		streamW.Flush ();
		streamW.Close ();

		SceneManager.LoadScene ("NewWorkAuthorizationMenu");

	}
}
	
