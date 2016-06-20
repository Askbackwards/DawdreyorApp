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

	private string WOListURL, WOList, txtPath;
	private bool doneSending;

	// Use this for initialization
	void Start () {
		//txtPath = Application.persistentDataPath + "WOList.txt";
		txtPath = "C:/Users/nomore/Desktop/WOList.txt";
		WOListURL = "https://dl.dropboxusercontent.com/s/9lnd15w7mccer0v/WOList.txt";
		StartCoroutine (GetWOList ());
	}
	
	// Update is called once per frame
	void Update () {
	
		if (doneSending)
			SceneManager.LoadScene ("NewWorkAuthorizationMenu");

	}

	public void SavePP(string WONum) {
		PlayerPrefs.SetString ("WOID", WONum);
	}

	public void Finish() {
		//This saves the WOID to the WOList File for later reloading

		loadingScreen.SetActive (true);

		//Create File
		File.Create (txtPath).Dispose();

		//OpenFile
		StreamWriter streamW = new StreamWriter(txtPath);

		//Write Old Stuff
		streamW.WriteLine (WOList);

		//Write new stuff
		streamW.WriteLine (PlayerPrefs.GetString("WOID") + ",");
		streamW.Flush ();
		streamW.Close ();

		//Send the list to dropbox
		StartCoroutine (SendList ());
	}

	//---------------------------------------------------------------------------------------

	//Pull list from dropbox
	private IEnumerator GetWOList() {
		WWW WOWWW = new WWW (WOListURL);
		yield return WOWWW;
		WOList = WOWWW.text;
	}

	//---------------------------------------------------------------------------------------

	private IEnumerator SendList() {
		//Email that file
		MailMessage mail = new MailMessage();
		mail.From = new MailAddress ("crews4hiresender@gmail.com");
		mail.To.Add ("jerod_2de0@sendtodropbox.com");
		mail.Subject = "WorkAuthorizations";
		mail.Body = "WorkAuthorizations";
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

		yield return null;
	}

	//---------------------------------------------------------------------------------------

	//This is called when the message has been sent
	private void FinishedSending(object sender, System.ComponentModel.AsyncCompletedEventArgs e) {
		doneSending = true;
		File.Delete (txtPath + "WOList.txt");
	}
}
