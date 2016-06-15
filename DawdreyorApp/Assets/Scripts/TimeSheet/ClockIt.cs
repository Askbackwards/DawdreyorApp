using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;
using System;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Collections.Generic;

public class ClockIt : MonoBehaviour {

	public GameObject actionText, timeText, jobInput, submitJob, submitError, contentPane, popUpBackground, loadingScreen, deleteButton, clockIt, actionChoose, customActionChoose, customHourInput, customMinuteInput, submitCustom, customSubmitError, customJobInput;

	private GameObject tempObj;
	private int hour, minute, valueChosen, deleteAmount;
	private string hourS, minuteS, customAction, emailList, emailURL, txtPath, job, date;
	private float stamps;
	private string[] emailArray;
	private bool ready, canDelete, doneSending;

	//-----------------------------------------------------------------------------------

	void Start () {
		//Assign the file path
		date = (System.DateTime.Now.Month.ToString ()) + "-" + (System.DateTime.Now.Day.ToString ()) + "-" + (System.DateTime.Now.Year.ToString ());
		txtPath = Application.persistentDataPath + "/" + date + "~" + PlayerPrefs.GetString ("Username") + ".txt";
		//txtPath = "C:/Users/nomore/Desktop/Test.txt";

		//Set some values
		valueChosen = 4;
		job = " ";
		stamps = 1;
		doneSending = false;
		ready = true;
		deleteAmount = 0;
		customAction = "none";
		emailURL = "https://dl.dropboxusercontent.com/s/u1vz76hek3u4hep/Emails.txt";
		StartCoroutine (GetEmails ());

		//Load the current logs if there are any
		if (File.Exists (txtPath) == true) {
			LoadCurrentText ();
			canDelete = true;
		}

		//Create File if it's not there
		if (File.Exists (txtPath) == false) {
			File.Create (txtPath).Dispose(); 
			canDelete = false;
		}
	}

	//-----------------------------------------------------------------------------------

	void Update () {
		if (doneSending == true) {
			//Delete File
			File.Delete(txtPath);
			//GetRid of loading screen
			loadingScreen.SetActive (false);
			//Restart App
			SceneManager.LoadScene("TimeSheet");
		}
	}

	//-----------------------------------------------------------------------------------

	//Coroutine to retrieve text files
	private IEnumerator GetEmails() {
		WWW emailWWW = new WWW (emailURL);
		yield return emailWWW;
		emailList = emailWWW.text;
		emailArray = emailList.Split ('=');
	}

	//-----------------------------------------------------------------------------------

	private void LoadCurrentText() {
		//Sets up char array that will split job and time
		char[] spaceArray;
		spaceArray = "\t".ToCharArray();

		//make it check the txt file on load and show current hours and shit
		string[] oldText = File.ReadAllLines(txtPath);
		for (int i = 0; i < oldText.Length; i++) {
			string[] tempLine = oldText [i].Split (spaceArray, 2);
			if (tempLine [0] == "Out") {
				actionChoose.SetActive (false);
				clockIt.SetActive (false);
			}
			if (tempLine [0] == "Out" || i == 0) {
				canDelete = false;
			} else {
				canDelete = true;
			}
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

			//Don't let  them spam it
			ready = false;

			PunchIt (job, false, "");
			ready = true;
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
		if (valueChosen < 5 && ready == true) {
			//Check to see if input is needed for the job option
			if (valueChosen != 4) {
				//Send it through the action method
				switch (valueChosen) {
				case(1):
					//Disable the job input
					jobInput.SetActive (false);
					submitJob.SetActive (false);

					//Don't let  them spam it
					ready = false;

					PunchIt ("Shop", false, "");
					canDelete = true;
					ready = true;
					break;
				case(2):
					//Disable the job input
					jobInput.SetActive (false);
					submitJob.SetActive (false);

					//Don't let  them spam it
					ready = false;

					PunchIt ("Travel", false, "");
					canDelete = true;
					ready = true;
					break;
				case(3):
					//Disable the job input
					jobInput.SetActive (false);
					submitJob.SetActive (false);

					//Don't let  them spam it
					ready = false;

					//Disable any more clocking
					actionChoose.SetActive (false);
					clockIt.SetActive (false);

					canDelete = false;
					PunchIt ("Out", false, "");
					canDelete = true;
					ready = true;
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
			//Get Minute with zeroes
			string minuteNow = System.DateTime.Now.TimeOfDay.ToString();
			string[] minuteArray = minuteNow.Split (":".ToCharArray (), 3);

			tempObj.GetComponent<Text> ().text = System.DateTime.Now.Hour.ToString () + ":" + minuteArray[1];
		} else {
			tempObj.GetComponent<Text> ().text = timeToUse;
		}
		tempObj.transform.SetParent (contentPane.transform);
		tempObj.transform.localScale = new Vector3 (1, 1, 1);
		tempObj.GetComponent<RectTransform> ().position = timeText.GetComponent<RectTransform> ().position - new Vector3 (0, 1 * stamps, 0);
		tempObj.SetActive (true);

		if (canDelete == true) {
			tempObj = (GameObject)Instantiate (deleteButton);
			tempObj.transform.SetParent (contentPane.transform);
			tempObj.transform.localScale = new Vector3 (1, 1, 1);
			tempObj.GetComponent<RectTransform> ().position = deleteButton.GetComponent<RectTransform> ().position - new Vector3 (0, 1 * stamps, 0);
			tempObj.name = "deleteButton" + deleteAmount;
			tempObj.GetComponent<DeleteIdentifier> ().Name (deleteAmount);
			tempObj.SetActive (true);
		}

		deleteAmount += 1;

		//Make sure next stamp is under this one
		stamps += 1;
		if (oldTime == false)
			Logit (action);
		else
			ready = true;
	}

	//-----------------------------------------------------------------------------------

	//Log the time and job in a text file
	private void Logit(string action) { 
		
		//Read File
		string[] oldFile = File.ReadAllLines (txtPath);
		
		//OpenFile
		StreamWriter streamW = new StreamWriter(txtPath);
		
		//Rewrite what was there
		for (int i = 0; i < oldFile.Length; i++) {
			streamW.WriteLine (oldFile [i]);
		}

		//Get minutes with zeroes
		string minuteNow = System.DateTime.Now.TimeOfDay.ToString();
		string[] minuteArray = minuteNow.Split (":".ToCharArray (), 3);

		//Write new stuff
		streamW.WriteLine (action + "\t" + System.DateTime.Now.Hour + ":" + minuteArray[1]);
		streamW.Flush ();
		streamW.Close ();
		
		ready = true;
		
	}

	//-----------------------------------------------------------------------------------

	//Sends the file to dropbox after reformating it for excel (csv) and deletes it
	public void Submit() {
		//Confirm their action
		if (File.Exists(txtPath))
			popUpBackground.SetActive (true);
	}

	public void No() {
		popUpBackground.SetActive (false);
	}

	public void Yes() {
		popUpBackground.SetActive (false);
		loadingScreen.SetActive (true);
		StartCoroutine(SendIt ());
	}

	//Where the action happens
	private IEnumerator SendIt() {
		//Sets up char array that will split job and time
		char[] spaceArray;
		spaceArray = "\t:".ToCharArray();

		//Rewrite the file with excel friendly info (csv)
		string[] oldText = File.ReadAllLines(txtPath);
		StreamWriter streamW = new StreamWriter(txtPath);
		for (int i = 0; i < oldText.Length; i++) {
			string[] tempLine = oldText [i].Split (spaceArray, 6);
			if (tempLine.Length < 4) {
				streamW.WriteLine (tempLine [0] + "," + tempLine [1] + "," + tempLine [2] + "\n");
			} else {
				streamW.WriteLine (tempLine [0] + "," + tempLine [1] + "," + tempLine [2] + ",," + tempLine [3] + "," + tempLine [4] + "," + tempLine[5] + "\n");
			}
		}
		streamW.Flush ();
		streamW.Close ();

		//Email that file
		MailMessage mail = new MailMessage();
		mail.From = new MailAddress ("crews4hiresender@gmail.com");
		mail.Bcc.Add ("jerod_2de0@sendtodropbox.com");
		//Make sure the send them a copy if they have an email
		if (emailArray[PlayerPrefs.GetInt("Index") - 1] != "none")
			mail.To.Add (emailArray[PlayerPrefs.GetInt("Index")-1]);
		mail.Subject = PlayerPrefs.GetString ("Username");
		mail.Body = "TimeSheet";
		System.Net.Mail.Attachment attachment = new System.Net.Mail.Attachment (txtPath);
		mail.Attachments.Add (attachment);

		SmtpClient smtp = new SmtpClient ("smtp.gmail.com");
		smtp.Port = 587;
		smtp.Credentials = new System.Net.NetworkCredential ("crews4hiresender@gmail.com", "C4H7265$%") as ICredentialsByHost;
		smtp.EnableSsl = true;

		ServicePointManager.ServerCertificateValidationCallback =
			delegate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) {
			return true;
		};
		smtp.SendAsync(mail, "");

		smtp.SendCompleted += new SendCompletedEventHandler (FinishedSending);

		//SceneManager.LoadScene ("TimeSheet");

		yield return null;
	}


	//This is called when the message has been sent
	private void FinishedSending(object sender, System.ComponentModel.AsyncCompletedEventArgs e) {
		doneSending = true;
	}

	//-----------------------------------------------------------------------------------

	public void DeleteLine(DeleteIdentifier dI) {
		//Fetch what line to delete
		int lineToDelete = dI.ReturnName();

		//Read File
		string[] oldFile = File.ReadAllLines (txtPath);

		//OpenFile
		StreamWriter streamW = new StreamWriter(txtPath);

		//Rewrite what was there and exclude the deleted line
		for (int i = 0; i < oldFile.Length; i++) {
			if (i != lineToDelete)
				streamW.WriteLine (oldFile [i]);
		}

		//Close file
		streamW.Flush ();
		streamW.Close ();

		//Reload scene
		SceneManager.LoadScene ("TimeSheet");
	}

	//-----------------------------------------------------------------------------------

	//Saves inputed hour
	public void HourValSave(string time) {
		hourS = time;
		bool parsingTry = int.TryParse (time, out hour);
		if (parsingTry == false) {
			hour = 999;
		}
	}

	//Saves inputed minute
	public void MinuteValSave(string time) {
		minuteS = time;
		bool parsingTry = int.TryParse (time, out minute);
		if (parsingTry == false) {
			minute = 999;
		}
	}

	public void SaveCustomJobInput(string job) {
		customAction = job.Trim ();
	}

	//Insert a custom time into the timesheet
	public void InsertCustomStart() {
		//Set up UI for custom input
		clockIt.SetActive (false);
		actionChoose.SetActive (false);
		customActionChoose.SetActive (true);
		customHourInput.SetActive (true);
		customMinuteInput.SetActive (true);
		submitCustom.SetActive (true);

		valueChosen = 5;
		minute = 999;
		hour = 999;
	}

	//Return what action is chosen in string format
	public void SortValue(int val) {
		switch (val) {
		case(1):
			customJobInput.SetActive (false);
			customAction = "Shop";
			break;
		case(2):
			customJobInput.SetActive (false);
			customAction = "Travel";
			break;
		case(3):
			customJobInput.SetActive (false);
			customAction = "Out";
			break;
		case(4):
			customJobInput.SetActive (true);
			break;
		}
	}

	public void InsertCustom() {

		if (minute == 999 || hour == 999 || customAction == "none" || minute > 59 || minute < 0 || hour > 23 || hour < 0) {
			customSubmitError.SetActive (true);
			return;
		}

		//Read Written Inputs
		string[] oldFile = File.ReadAllLines (txtPath);

		//Declare vars
		int upClose, downClose, equal, insertLine, downNum, upNum;
		bool hasWritten, hasDown, hasUp;

		upClose = 0;
		downClose = 0;
		insertLine = 0;
		downNum = 0;
		upNum = 0;
		hasWritten = false;
		hasUp = false;
		hasDown = false;
		equal = 999;

		//Sets up char array that will split job and time
		char[] spaceArray;
		spaceArray = "\t:".ToCharArray ();

		if (oldFile != null) {

			//Find out what time it belongs in
			for (int i = 0; i < oldFile.Length; i++) {
				string[] tempLine = oldFile [i].Split (spaceArray, 3);

				int tempInt = int.Parse (tempLine [1].Trim());

				if (tempInt < hour && tempInt >= downNum) {
					downNum = tempInt;
					downClose = i;
					hasDown = true;
				} else if (tempInt > hour && tempInt < upNum && tempInt != upNum) {
					upNum = tempInt;
					upClose = i;
					hasUp = true;
				} else if (tempInt == hour) {
					equal = tempInt;
				}
			}


			//Set its line
			if (equal == 999) {
				if (hasUp == true) {
					insertLine = upClose - 1;
				} else if (hasDown == true) {
					insertLine = downClose + 1;
				}
			} else {
				//Reset Values
				upClose = 0;
				downClose = 0;
				upNum = 0;
				downNum = 0;
				equal = 999;
				hasUp = false;
				hasDown = false;

				//Check Minutes
				for (int i = 0; i < oldFile.Length; i++) {
					string[] tempLine = oldFile [i].Split (spaceArray, 6);

					int tempInt = int.Parse (tempLine [2].Trim ());
					int tempInt2 = int.Parse (tempLine [1].Trim ());



					if (tempInt2 == hour) {
						if (tempInt < minute && tempInt >= downNum) {
							downNum = tempInt;
							downClose = i;
							hasDown = true;
						} else if ((tempInt > minute && tempInt < upNum && tempInt != upNum) || (tempInt > minute && upNum == 0) ) {
							upNum = tempInt;
							upClose = i;
							hasUp = true;
						} else if (tempInt == minute) {
							equal = i + 1;
						}
					}
				}

				//Find line based on minute
				if (equal == 999) {
					if (hasUp == true) {
						insertLine = upClose;
					} else if (hasDown == true) {
						insertLine = downClose + 1; 
					}
				} else {
					insertLine = equal;
				}
			}

		} else {
			insertLine = 0;
		}

		//Make sure insertLine isn't neg
		if (insertLine == -1) {
			insertLine = 0;
		}

		//OpenFile
		StreamWriter streamW = new StreamWriter(txtPath);

		//Get minute with zeroes
		string minuteNow = System.DateTime.Now.TimeOfDay.ToString();
		string[] minuteArray = minuteNow.Split (":".ToCharArray (), 3);

		//Write stuff
		for (int i = 0; i < oldFile.Length + 1; i++) {
			if (i == insertLine) {
				streamW.WriteLine (customAction + "\t" + hourS + ":" + minuteS + "\t" + "CustomCreationTime:" + System.DateTime.Now.Hour + ":" + minuteArray[1]);
				hasWritten = true;
			} else if (i != insertLine && hasWritten == false) {
				streamW.WriteLine (oldFile [i]);
			} else {
				streamW.WriteLine (oldFile [i - 1]);
			}
		}

		//Close it
		streamW.Flush ();
		streamW.Close ();

		SceneManager.LoadScene ("TimeSheet");
	}
}
