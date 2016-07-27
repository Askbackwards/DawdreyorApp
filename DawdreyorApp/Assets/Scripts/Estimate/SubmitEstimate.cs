using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.IO;
using System;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Collections.Generic;
	
public class SubmitEstimate : MonoBehaviour {

	public GameObject[] parents;
	public GameObject confirmText, popUp, loading;

	private GameObject[] inputObjs;
	private string txtPath2, txtPath;
	private int curInputs, emptyCount, skipCount;
	private string[] inputs, empties;
	private bool[] inputsDone;
	private bool doneSending;

	// Use this for initialization
	void Start () {
		//Assign Values!
		txtPath = Application.persistentDataPath + "/";
		txtPath2 = Application.persistentDataPath + "/" + PlayerPrefs.GetString("EstimateNumber") + "_Estimate.txt";
		curInputs = 17;
		emptyCount = 0;
		skipCount = 0;
		int tempNum = 0;
		inputs = new string[100];
		empties = new string[100];
		inputObjs = new GameObject[100];

		for (int i = 0; i < parents.Length; i++) {
			foreach (Transform child in parents[i].transform) {
				if (child.name.Contains("Input") || child.name.Contains("Drop")) {
					inputs [tempNum] = child.name;
					inputObjs [tempNum] = child.gameObject;
					tempNum += 1;
				}
			}
		}

		LoadCurrentInputed ();
	}
	
	// Update is called once per frame
	void Update () {
		if (doneSending) {
			PlayerPrefs.SetInt ("picAmount2" + PlayerPrefs.GetString ("EstimateNumber"), 0);
			File.Delete (txtPath2);
			for (int i = 0; i <= PlayerPrefs.GetInt ("picAmount2" + PlayerPrefs.GetString ("EstimateNumber")); i++) {
				File.Delete (txtPath + "Estimate_" + PlayerPrefs.GetString ("EstimateNumber") + "_Picture" + i + ".png");
			}
			SceneManager.LoadScene ("MainMenu");
		}
	}

	//Add number of inputs if an object is added
	public void AddInputs(int amountAdded) {
		curInputs += amountAdded;
	}

	//Send the estimate txt
	public void SendIt() {

		skipCount = 0;

		//Get Old text
		string[] oldText = File.ReadAllLines(txtPath2);

		//read through it
		for (int i = 0; i < (curInputs - skipCount); i++) {
			if (oldText.Length <= i) {
				empties [emptyCount] = inputs [i];
				emptyCount += 1;
			} else if(oldText[i + skipCount].CompareTo(inputs[i]) != -1 && oldText[i + skipCount].CompareTo(inputs[i]) != 1) {
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

	//-----------------------------------------------------------------------------------

	public void UseMailIt() {
		loading.SetActive (true);
		StartCoroutine (MailIt ());
	}

	private IEnumerator MailIt() {

		//Email that file
		MailMessage mail = new MailMessage();
		mail.From = new MailAddress ("crews4hiresender@gmail.com");
		mail.Bcc.Add ("jerod_2de0@sendtodropbox.com");
		mail.Subject = "Estimate_" + PlayerPrefs.GetString ("EstimateNumber");
		mail.Body = "Estimate";
		System.Net.Mail.Attachment attachment = new System.Net.Mail.Attachment (txtPath2);
		mail.Attachments.Add (attachment);
		for (int i = 1; i < PlayerPrefs.GetInt ("picAmount2" + PlayerPrefs.GetString ("EstimateNumber")); i++) {
			System.Net.Mail.Attachment a2 = new System.Net.Mail.Attachment (txtPath + "Estimate_" + PlayerPrefs.GetString("EstimateNumber") + "_Picture" + i + ".png");
			mail.Attachments.Add (a2);
		}

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

		yield return null;
	}

	//This is called when the message has been sent
	private void FinishedSending(object sender, System.ComponentModel.AsyncCompletedEventArgs e) {
		doneSending = true;
	}

	//-----------------------------------------------------------------------------------

	public void Back() {
		popUp.SetActive (false);
	}

	//-----------------------------------------------------------------------------------

	//See what has already but put in and put it back in
	public void LoadCurrentInputed() {

		if (File.Exists (txtPath2)) {
			//Get Old text
			string[] oldText = File.ReadAllLines (txtPath2);

			//read through it
			for (int i = 0; i < (oldText.Length - 1 + skipCount); i++) {
				if (oldText[i -skipCount].CompareTo(inputs[i]) == -1) {
					string[] holder = oldText [i - skipCount].Split (",".ToCharArray ());
					inputObjs [i].GetComponentInChildren<Text> ().text = holder[1];
				} else {
					skipCount += 1;
				}

			}
		}
	}
		
}
