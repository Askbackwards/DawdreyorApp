﻿using UnityEngine;
using System.Collections;
using System.IO;

public class WriteToFile : MonoBehaviour {

	private int position;
	private string name, txtPath;
	private bool addIt;
	private GameObject theObject;

	// Use this for initialization
	void Start () {
		addIt = false;
		txtPath = Application.persistentDataPath + "/" + PlayerPrefs.GetString("EstimateNumber") + "_Estimate.txt";
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	//store where this thing should be written
	public void storeNumber(int place) {
		position = place;
	}

	//store the name of the object
	public void setName(string theName) {
		name = theName;
	}

	public int getNumber() {
		return position;
	}

	public string getName() {
		return name;
	}

	//Write it
	public void writeIt(string input) {
		//Make file if it doesn't exsist
		if (!File.Exists (txtPath))
			File.Create (txtPath).Dispose ();

		string[] oldText = File.ReadAllLines(txtPath);

		//OpenFile
		StreamWriter streamW = new StreamWriter(txtPath);

		//Write stuff
		if (oldText.Length > position) {
			for (int i = 0; i < oldText.Length - 1; i++) {
				if (i != position) {
					if (addIt) {
						streamW.WriteLine (oldText [i-1]);
					} else {
						streamW.WriteLine (oldText [i]);
					}
				} else {
					streamW.WriteLine (name + "," + input);
					addIt = true;
					if (oldText [i].Contains(name))
						addIt = false;
				}
			}
		} else {
			for (int i = 0; i < oldText.Length; i++) {
				if(!oldText[i].Contains(name))
					streamW.WriteLine (oldText [i]);
			}
			streamW.WriteLine (name + "," + input);
		}
		addIt = false;
		streamW.Flush ();
		streamW.Close ();
	}

	//----------------------------------------------------------------------------------

	public void WriteDropdown(int picked) {
		string[] options = theObject.gameObject.GetComponent<DefineList> ().GetOptions ();
		writeIt(options[picked]);
	}

	public void SetObject(GameObject setObject) {
		theObject = setObject;
	}
}