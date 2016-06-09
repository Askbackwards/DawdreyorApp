using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;

public class ClockIt : MonoBehaviour {

	public GameObject actionText, timeText, jobInput, submitJob, submitError, contentPane;

	private GameObject tempObj;
	private int valueChosen;
	private string txtPath, job;
	private float stamps;

	//-----------------------------------------------------------------------------------

	void Start () {
		txtPath = Application.persistentDataPath + "/" + System.DateTime.Now.Date + "::" + PlayerPrefs.GetString("Username") +".txt";
		// TODO: Replace with Application.persistentDataPath + "/" + System.DateTime.Now.Date + "::" + PlayerPrefs.GetString("Username") +".txt"
		valueChosen = 4;
		job = " ";
		stamps = 1;

		if(File.Exists(txtPath) == true)
			LoadCurrentText ();
	}

	//-----------------------------------------------------------------------------------

	void Update () {
	
	}

	//-----------------------------------------------------------------------------------

	private void LoadCurrentText() {
		char[] spaceArray;
		spaceArray = "\t".ToCharArray();

		//make it check the txt file on load and show current hours and shit
		string[] oldText = File.ReadAllLines(txtPath);
		for (int i = 0; i < oldText.Length; i++) {
			string[] tempLine = oldText [i].Split (spaceArray, 2);
			PunchIt (tempLine [0], true, tempLine [1]);
		}
	}

	//-----------------------------------------------------------------------------------

	//Save Job input
	public void SaveJob(string input) {
		job = input;
	}

	//-----------------------------------------------------------------------------------

	public void SubmitJob() {
		//Make sure they entered text
		string trimmedJob = job.Replace (" ", "");
		if (trimmedJob != "") {
			//Clean up the UI and send it to processing and handling
			submitError.SetActive (false);
			jobInput.SetActive (false);
			submitJob.SetActive (false);
			PunchIt (job, false, "");
		} else {
			//Pop Error if they didn't put in anything
			submitError.SetActive (true);
		}
	}

	//-----------------------------------------------------------------------------------

	//Saves what option was chosen
	public void SaveValue(int val) {
		valueChosen = val;
	}

	//-----------------------------------------------------------------------------------

	//Puts a time stamp on the action
	public void Clock() {
		//Make sure a value is chosen
		if (valueChosen < 5) {
			//Check to see if input is needed for the job option
			if (valueChosen != 4) {
				//Send it through the action method
				switch (valueChosen) {
				case(1):
					PunchIt ("Shop", false, "");
					break;
				case(2):
					PunchIt ("Travel", false, "");
					break;
				case(3):
					PunchIt ("Out", false, "");
					break;
				}
			} else {
				//Have them fill out the job
				jobInput.SetActive (true);
				submitJob.SetActive (true);
			}
		}
	}

	//-----------------------------------------------------------------------------------

	//Save the time stamp and add it to the text file that will be sent
	private void PunchIt(string action, bool oldTime, string timeToUse) {

		//Create the stamp
		tempObj = (GameObject)Instantiate (actionText);
		tempObj.GetComponent<Text> ().text = action;
		tempObj.transform.SetParent (contentPane.transform);
		tempObj.transform.localScale = new Vector3 (1, 1, 1);
		tempObj.GetComponent<RectTransform> ().position = actionText.GetComponent<RectTransform> ().position - new Vector3(0,1 * stamps,0);
		tempObj.SetActive (true);

		tempObj = (GameObject)Instantiate (timeText);
		if (oldTime == false) {
			tempObj.GetComponent<Text> ().text = System.DateTime.Now.Hour.ToString () + ":" + System.DateTime.Now.Minute.ToString ();
		} else {
			tempObj.GetComponent<Text> ().text = timeToUse;
		}
		tempObj.transform.SetParent (contentPane.transform);
		tempObj.transform.localScale = new Vector3 (1, 1, 1);
		tempObj.GetComponent<RectTransform> ().position = timeText.GetComponent<RectTransform> ().position - new Vector3 (0, 1 * stamps, 0);
		tempObj.SetActive (true);

		//Make sure next stamp is under this one
		stamps += 1;
		if (oldTime == false)
			Logit (action);
	}

	//-----------------------------------------------------------------------------------

	//Log the time and job in a text file
	private void Logit(string action) { 
		//Create File

		if (File.Exists (txtPath) == false) {
			File.Create (txtPath); 
		}
		//Read File
		string[] oldFile = File.ReadAllLines (txtPath);

		//OpenFile
		StreamWriter streamW = new StreamWriter(txtPath);
		//Rewrite what was there
		for (int i = 0; i < oldFile.Length; i++) {
			streamW.WriteLine(oldFile[i]);
		}
		//Write new stuff
		streamW.WriteLine (action + "\t" + System.DateTime.Now.Hour + ":" + System.DateTime.Now.Minute);
		streamW.Flush ();
		streamW.Close ();
	}

	//-----------------------------------------------------------------------------------

	public void Submit() {
		File.Delete (txtPath);
		SceneManager.LoadScene ("TimeSheet");
	}
}
