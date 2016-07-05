using UnityEngine;
using System.Collections;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class SaveWONumInPP : MonoBehaviour {

	public GameObject loadingScreen;

	private string txtPath;
	private bool doneSending;

	// Use this for initialization
	void Start () {
		doneSending = false;
		PlayerPrefs.SetInt ("In", 0);
		PlayerPrefs.SetInt ("Out", 0);
		PlayerPrefs.SetInt ("picAmount", 0);
		txtPath = Application.persistentDataPath + "/WOList.txt";
		//txtPath = "C:/Users/nomore/Desktop/WOList.txt";
	}
	
	// Update is called once per frame
	void Update () {
	
		if (doneSending) {
			SceneManager.LoadScene ("NewWorkAuthorizationMenu");
		}

	}

	public void SavePP(string WONum) {
		PlayerPrefs.SetString ("WOID", WONum);
	}

	public void Finish() {
		//This saves the WOID to the WOList File for later reloading

		loadingScreen.SetActive (true);

		//Create File
		if(!File.Exists(txtPath))
			File.Create (txtPath).Dispose();
		
		string[] WOList = File.ReadAllLines (txtPath);

		//OpenFile
		StreamWriter streamW = new StreamWriter(txtPath);

		//Write Old Stuff
		if (WOList.Length > 0)
			streamW.Write (WOList[0]);

		//Write new stuff
		streamW.Write (PlayerPrefs.GetString("WOID") + ",");
		streamW.Flush ();
		streamW.Close ();

		doneSending = true;

	}

	//---------------------------------------------------------------------------------------

}
