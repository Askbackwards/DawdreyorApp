using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.IO;

public class LoadEstimateList : MonoBehaviour {

	public GameObject estimateNum, contentPane;

	private string txtPath;
	private string[] estimates,temp2;
	private int count;

	// Use this for initialization
	void Start () {
		txtPath = Application.persistentDataPath + "/";
		estimates = new string[100];

		//Find Estimate txts
		foreach (string file in Directory.GetFiles(txtPath)) {
			if (file.Contains ("Estimate")) {
				string[] temp = file.Split ("/".ToCharArray ());
				for (int i = 0; i < temp.Length; i++) {
					if (temp [i].Contains ("Estimate")) {
						temp2 = temp [i].Split ("_".ToCharArray ());
					}
				}
				estimates [count] = temp2 [0];
				count += 1;
			}
		}

		//Create the new object
		for (int i = 0; i < count; i++) {
			GameObject tempObj = (GameObject)Instantiate (estimateNum);
			tempObj.GetComponentInChildren<Text> ().text = estimates[i];
			tempObj.transform.SetParent (contentPane.transform);
			tempObj.transform.localScale = new Vector3 (1, 1, 1);
			tempObj.GetComponent<RectTransform> ().position = estimateNum.GetComponent<RectTransform> ().position - new Vector3(0,1 * i,0);
			tempObj.GetComponent<IDHandling> ().SetID (estimates[i]);
			tempObj.SetActive (true);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
