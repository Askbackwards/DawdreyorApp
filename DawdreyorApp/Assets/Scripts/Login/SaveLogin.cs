using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SaveLogin : MonoBehaviour {

	public GameObject usernameError, passwordError, LoadingScreen;
	public string mainMenuScene;

	private string[] userArray, passArray;
	private TextAsset usernameFile, passwordFile;
	private string usernameList, passwordList, userURL, passURL;
	private int i, continueCheck, count, userIndex, passIndex;
	private bool inArray, firstRun;

	// Use this for initialization
	void Start () {
		//Get URL to download files
		userURL = "https://dl.dropboxusercontent.com/s/apejh96v4aol566/Usernames.txt";
		passURL = "https://dl.dropboxusercontent.com/s/1naeh8jbgfdmo4j/Passwords.txt";
		StartCoroutine(GetUsernames());
		StartCoroutine (GetPasswords());
		StartCoroutine (HoldIt ());

		//Set Playerprefs
		PlayerPrefs.SetString("Username", "nothing");
		PlayerPrefs.SetString ("Password", "nothing");
		PlayerPrefs.SetInt ("Index", 0);
	
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	//Coroutine to retrieve text files
	private IEnumerator GetUsernames() {
		WWW userWWW = new WWW (userURL);
		yield return userWWW;
		usernameList = userWWW.text;
		continueCheck += 1;
	}

	private IEnumerator GetPasswords() {
		WWW passWWW = new WWW (passURL);
		yield return passWWW;
		passwordList = passWWW.text;
		continueCheck += 1;
	}

	//Keep up loading screen until urls are found
	private IEnumerator HoldIt() {
		while (continueCheck != 2) {
			yield return new WaitForSeconds(1);
			LoadingScreen.GetComponentInChildren<Text>().text = "Loading..." + i;
			i++;
		}
		//set string to string array in order to find position
		userArray = usernameList.Split('=');
		passArray = passwordList.Split('=');

		LoadingScreen.SetActive (false);
	}

	//Tell the scripts not to stackoverflow
	public void SetFirstRun (bool isFirst) {
		firstRun = isFirst;
	}

	//This will make sure the Username Exsist when they finish typing it
	public void CheckUsername (string username) {
		PlayerPrefs.SetString ("Username", username);

		//Check to see if entered user is a username
		inArray = false;
		foreach (string str in userArray) {
			if (str == username) {
				inArray = true;
				break;
			}
		}

		if (inArray == false) {
			usernameError.SetActive (true);
			userIndex = 0;
		} else {
			usernameError.SetActive (false);

			//Check for the index number of username to compare it to the password's
			count = 0;
			foreach (string str in userArray) {
				count += 1;
				if (str == username) {
					userIndex = count;
					break;
				}
			}
		}
		if (PlayerPrefs.GetString ("Password") != "nothing" & firstRun == true) {
			firstRun = false;
			CheckPassword (PlayerPrefs.GetString ("Password"));
		}
	}



	//This will make sure the Password Exsist when they finish typing it and it correlates with the username
	public void CheckPassword (string password) {
		PlayerPrefs.SetString ("Password", password);
		
		//Check to see if entered pass is a password
		inArray = false;
		foreach (string str in passArray) {
			if (str == password) {
				inArray = true;
				break;
			}
		}

		// Check to make sure it's the right password
		if (inArray == false) {
			passIndex = 0;
			passwordError.SetActive (true);
		} else {
			// Find Pass index
			count = 0;
			foreach (string str in passArray) {
				count += 1;
				if (str == password) {
					passIndex = count;
					break;
				}
			}
			//Compare pass and user index
			if (passIndex != userIndex) {
				passwordError.SetActive (true);
			} else {
				passwordError.SetActive (false);
			}
		}
		if (PlayerPrefs.GetString ("Username") != "nothing" & firstRun == true) {
			firstRun = false;
			CheckUsername (PlayerPrefs.GetString ("Username"));
		}
	}

	//Check to make sure the creds are right to login and if so, login
	public void LoginCheck() {
		if (usernameError.activeSelf == false & passwordError.activeSelf == false & PlayerPrefs.GetString ("Username") != "nothing" & PlayerPrefs.GetString ("Password") != "nothing") {
			PlayerPrefs.SetInt ("Index", userIndex);
			SceneManager.LoadScene (mainMenuScene);
		}
	}
}
