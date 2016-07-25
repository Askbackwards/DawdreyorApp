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

	private string txtPath2;
	private int curInputs, emptyCount, skipCount;
	private string[] inputs, empties;
	private bool[] inputsDone;
	private bool doneSending;

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
		if (doneSending) {
			SceneManager.LoadScene ("MainMenu");
		}
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
		for (int i = 0; i < (curInputs - skipCount); i++) {
			if (oldText.Length <= i) {
				empties [emptyCount] = inputs [i];
				emptyCount += 1;
				Debug.Log (skipCount);
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
		
}
