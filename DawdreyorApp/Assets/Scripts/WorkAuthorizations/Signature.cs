using UnityEngine;
using System.Collections;
using System.IO;
using System;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class Signature : MonoBehaviour {

	public GameObject LRobj, loadingScreen;
	public Material blackMaterial;

	private LineRenderer LR;
	private int curVtex;
	private GameObject UsingObj;
	private bool doneSending, ready, ready2;
	private string txtPath, newArray;

	// Use this for initialization
	void Start () {
		LR = LRobj.GetComponent<LineRenderer> ();
		curVtex = 0;
		UsingObj = LRobj;
		ready = false;
		ready2 = false;
		txtPath = Application.persistentDataPath + "/";
		//txtPath = "C:/Users/nomore/Desktop/";
	}
	
	// Update is called once per frame
	void Update () {
		if ((Input.touchCount > 0 && Input.GetTouch (0).phase == TouchPhase.Moved && ready) || (Input.touchCount > 0 && Input.GetTouch (0).phase == TouchPhase.Stationary && ready)) {
			// Get movement of the finger since last frame
			Vector3 touchDeltaPosition = Camera.main.ScreenToWorldPoint(new Vector3 (Input.GetTouch (0).position.x, Input.GetTouch(0).position.y, 0f));

			touchDeltaPosition = new Vector3 (touchDeltaPosition.x, touchDeltaPosition.y, 0f);

			//Create new vector to connect to after each movement of the finger
			if (Input.GetTouch (0).deltaPosition.x > .5f || Input.GetTouch(0).deltaPosition.y > .5f) {
				//Move the line
				curVtex += 1;
				LR.SetVertexCount (curVtex);

				LR.SetPosition (curVtex-1, touchDeltaPosition);
			}
		//Start a new Line Renderer when the finger begins
		} else if (Input.touchCount > 0 && Input.GetTouch (0).phase == TouchPhase.Began && ready) {
			ready = false;
			StartCoroutine (StartNewLine ());
		}

		if (ready2) {
			//Send that shit
			ready2 = false;
			StartCoroutine (SendWorkAuth ());
		}

		if (doneSending) {
			//Delete old files
			File.Delete (txtPath + PlayerPrefs.GetString ("WOID") + "_CustomerSignature.png");
			File.Delete (txtPath + PlayerPrefs.GetString ("WOID") + "_Crews4HireRepSignature.png");
			File.Delete (txtPath + PlayerPrefs.GetString ("WOID") + "_Info.txt");
			if (PlayerPrefs.GetInt ("picAmount") != 0) {
				for (int i = 1; i <= PlayerPrefs.GetInt ("picAmount"); i++) {
					File.Delete (txtPath + PlayerPrefs.GetString ("WOID") + "_Picture" + i + ".png");
				}
			}

			//Get rid of WOID in list
			string[] oldText = File.ReadAllLines (txtPath + "WOList.txt");
			string [] listArray = oldText[0].Split(",".ToCharArray());

			StreamWriter streamW = new StreamWriter(txtPath + "WOList.txt");
			for (int i = 0; i < listArray.Length; i++) {
				if (listArray [i] != PlayerPrefs.GetString ("WOID"))
					newArray += listArray [i] + ",";
			}
			streamW.WriteLine (newArray);
			streamW.Flush ();
			streamW.Close ();

			SceneManager.LoadScene ("MainMenu");
		}
	}

	private IEnumerator StartNewLine() {
		UsingObj = (GameObject)Instantiate (LRobj);
		UsingObj.transform.position = Camera.main.ScreenToWorldPoint (Input.GetTouch (0).position);
		UsingObj.AddComponent<LineRenderer> ();
		LR = UsingObj.GetComponent<LineRenderer> ();
		LR.useWorldSpace = true;
		LR.material = blackMaterial;
		LR.SetWidth (.1f, .1f);
		LR.SetColors (new Color (255, 255, 255), new Color (255, 255, 255));
		ready = true;
		curVtex = 0;
		yield return null;
	}

	//-------------------------------------------------------------------------------------------------------

	public void SaveAndSubmitSignatures() {

		//Scan and save PNG of signature
		ready2 = false;
		ready = false;
		StartCoroutine(SaveSig(false));
	}

	public void SaveCustomerSignature() {

		//No more writing
		ready2 = false;
		ready = false;
		//Scan and save PNG of signature
		StartCoroutine(SaveSig(true));
	}

	private IEnumerator SaveSig(bool isCustomer) {

		//Wait till end of frame
		yield return new WaitForEndOfFrame();

		//Create a texture the size of the sig area
		Texture2D tex = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);

		//Read screen contents into the texture
		tex.ReadPixels(new Rect(0,0, Screen.width, Screen.height), 0, 0);
		tex.Apply ();

		//Encode texture into PNG
		byte[] bytes = tex.EncodeToPNG();
		Destroy (tex);

		//Save PNG
		if (isCustomer)
			File.WriteAllBytes(txtPath + PlayerPrefs.GetString("WOID") + "_CustomerSignature.png", bytes);
		else if (!isCustomer)
			File.WriteAllBytes(txtPath + PlayerPrefs.GetString("WOID") + "_Crews4HireRepSignature.png", bytes);

		//Show loading screen
		loadingScreen.SetActive(true);
		ready2 = true;
	}

	//------------------------------------------------------------------------------------------------------------------

	private IEnumerator SendWorkAuth() {

		//Email that file
		MailMessage mail = new MailMessage();
		mail.From = new MailAddress ("crews4hiresender@gmail.com");
		mail.To.Add ("david_6e25@sendtodropbox.com");
		mail.Subject = "WorkAuthorization_" + PlayerPrefs.GetString("WOID");
		mail.Body = "WorkAuthorizations";

		if (PlayerPrefs.GetInt ("picAmount") != 0) {
			for (int i = 1; i <= PlayerPrefs.GetInt ("picAmount"); i++) {
				System.Net.Mail.Attachment at = new System.Net.Mail.Attachment (txtPath + PlayerPrefs.GetString ("WOID") + "_Picture" + i + ".png");
				mail.Attachments.Add (at);
			}
		}

		System.Net.Mail.Attachment attachment = new System.Net.Mail.Attachment (txtPath + PlayerPrefs.GetString("WOID") + "_CustomerSignature.png" );
		System.Net.Mail.Attachment attachment2 = new System.Net.Mail.Attachment (txtPath + PlayerPrefs.GetString("WOID") + "_Crews4HireRepSignature.png" );
		System.Net.Mail.Attachment attachment3 = new System.Net.Mail.Attachment (txtPath + PlayerPrefs.GetString ("WOID") + "_Info.txt");
		mail.Attachments.Add (attachment);
		mail.Attachments.Add (attachment2);
		mail.Attachments.Add (attachment3);

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
	private void FinishedSending(object sended, System.ComponentModel.AsyncCompletedEventArgs e) {
		doneSending = true;
	}

	//------------------------------------------------------------------------------------------------------------------
		
	public void AgreementFinished() {
		ready = true;
	}
		
}
