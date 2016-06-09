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

public class ClockIt : MonoBehaviour {

	public GameObject actionText, timeText, jobInput, submitJob, submitError, contentPane, popUpBackground, loadingScreen;

	private GameObject tempObj;
	private int valueChosen;
	private string emailList, emailURL, txtPath, job, date;
	private float stamps;
	private string[] emailArray;

	//-----------------------------------------------------------------------------------

	void Start () {
		//Assign the file path
		date = (System.DateTime.Now.Month.ToString()) + "-" + (System.DateTime.Now.Day.ToString()) + "-" + (System.DateTime.Now.Year.ToString());
		//txtPath = Application.persistentDataPath + "/" + date + "~" + PlayerPrefs.GetString("Username") +".txt";
		txtPath = "C:/Users/nomore/Desktop/Test.txt";

		//Set some values
		valueChosen = 4;
		job = " ";
		stamps = 1;
		emailURL = "https://dl.dropboxusercontent.com/s/u1vz76hek3u4hep/Emails.txt";
		StartCoroutine (GetEmails());


		//Load the current logs if there are any
		if(File.Exists(txtPath) == true)
			LoadCurrentText ();
	}

	//-----------------------------------------------------------------------------------

	void Update () {
	
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
			string[] tempLine = oldText [i].Split (spaceArray, 3);
			streamW.WriteLine (tempLine[0] + "," + tempLine[1] + "," + tempLine[2] + "\n");
		}
		streamW.Flush ();
		streamW.Close ();

		//Email that file
		MailMessage mail = new MailMessage();
		mail.From = new MailAddress ("jerod.dep@gmail.com");
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
		smtp.Credentials = new System.Net.NetworkCredential ("jerod.dep@gmail.com", "JDio683$") as ICredentialsByHost;
		smtp.EnableSsl = true;

		ServicePointManager.ServerCertificateValidationCallback =
			delegate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) {
			return true;
		};
		smtp.Send(mail);

		//GetRid of loading screen
		loadingScreen.SetActive (false);

		//SceneManager.LoadScene ("TimeSheet");

		yield return null;



	}
}
