using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO;

public class LoadList : MonoBehaviour {

	public GameObject WONum, contentPane, loadingScreen;

	private string listURL, theList, txtPath;
	private string[] listArray, oldText;

	void Start () {
		//Initialize
		txtPath = Application.persistentDataPath + "/WOList.txt";
		if (File.Exists (txtPath)) {
			oldText = File.ReadAllLines (txtPath);

			//Split dat text
			listArray = oldText[0].Split(",".ToCharArray());

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
		}

	}

	void Update () {
		
	}
		
}
