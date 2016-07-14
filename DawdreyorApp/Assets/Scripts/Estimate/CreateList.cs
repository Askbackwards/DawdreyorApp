using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CreateList : MonoBehaviour {

	public GameObject loadingScreen, colorSample, colorButton, contentPane;

	private string txtPath, colorURL, colorList;
	private string[] colorArray;
	private bool ready;

	void Start () {
		ready = false;

		txtPath = Application.persistentDataPath + "/";
		colorURL = "https://dl.dropboxusercontent.com/s/567kjkawr8uodbq/Colors.txt";

		//Get the colors
		StartCoroutine (GetColors ());

	}

	void Update () {
		//Create the color list
		if (ready) {
			for (int i = 0; i < colorArray.Length - 1; i++) {
				string[] tempArray = colorArray [i].Split (",".ToCharArray ());

				GameObject tempObj = (GameObject)Instantiate (colorSample);
				Color myColor = new Color();
				ColorUtility.TryParseHtmlString(tempArray[0], out myColor);
				tempObj.GetComponent<Image>().color = myColor;
				tempObj.transform.SetParent (contentPane.transform);
				tempObj.transform.localScale = new Vector3 (1, 1, 1);
				tempObj.SetActive (true);

				tempObj = (GameObject)Instantiate (colorButton);
				tempObj.GetComponentInChildren<Text> ().text = tempArray[1];
				tempObj.transform.SetParent (contentPane.transform);
				tempObj.transform.localScale = new Vector3 (1, 1, 1);
				tempObj.SetActive (true);

			}

			ready = false;
			loadingScreen.SetActive (false);
		}
	}

	//-----------------------------------------------------------------------------------

	//Coroutine to retrieve text files
	private IEnumerator GetColors() {
		WWW colorWWW = new WWW (colorURL);
		yield return colorWWW;
		colorList = colorWWW.text;
		colorArray = colorList.Split ("\n".ToCharArray ());
		ready = true;
	}
}
