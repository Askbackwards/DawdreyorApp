using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LoadList : MonoBehaviour {

	public GameObject WONum, contentPane, loadingScreen;

	private string listURL, theList;
	private string[] listArray;
	private bool canFinish;

	void Start () {
		//Initialize
		listURL = "https://dl.dropboxusercontent.com/s/9lnd15w7mccer0v/WOList.txt";

		//Get list
		StartCoroutine(GetTheList());

	}

	void Update () {
		if (canFinish)
			Finish ();
	}

	//Coroutine to retrieve text files
	private IEnumerator GetTheList() {
		WWW listWWW = new WWW (listURL);
		yield return listWWW;
		theList = listWWW.text;
		canFinish = true;
	}

	private void Finish() {
		//Split dat text
		listArray = theList.Split(",".ToCharArray());

		//Create the new object
		for (int i = 0; i < listArray.Length - 1; i++) {
			GameObject tempObj = (GameObject)Instantiate (WONum);
			tempObj.GetComponentInChildren<Text> ().text = listArray[i];
			tempObj.transform.SetParent (contentPane.transform);
			tempObj.transform.localScale = new Vector3 (1, 1, 1);
			tempObj.GetComponent<RectTransform> ().position = WONum.GetComponent<RectTransform> ().position - new Vector3(0,1 * i,0);
			tempObj.GetComponent<IDHandling> ().SetID (listArray [i]);
			tempObj.SetActive (true);
		}

		loadingScreen.SetActive (false);
		canFinish = false;
	}
}
