using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Collections;

public class PickPictureList : MonoBehaviour {

	public GameObject loadingScreen, pictureSample, pictureButton, contentPane, view;

	private string txtPath;

	// Use this for initialization
	void Start () {
		txtPath = Application.persistentDataPath + "/";
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void BringItUp() {
		
		view.SetActive (true);

		for (int i = 1; i <= PlayerPrefs.GetInt("picAmount2" + PlayerPrefs.GetString("EstimateNumber")); i++) {
			
			GameObject tempObj = (GameObject)Instantiate (pictureSample);
			byte[] data = File.ReadAllBytes(txtPath + "Estimate_" + PlayerPrefs.GetString("EstimateNumber") + "_Picture" + i + ".png");
			Texture2D texture = new Texture2D (64, 64, TextureFormat.ARGB32, false);
			texture.LoadImage (data);
			tempObj.GetComponent<RawImage> ().texture = texture;
			tempObj.transform.SetParent (contentPane.transform);
			tempObj.transform.localScale = new Vector3 (1, 1, 1);
			tempObj.SetActive (true);

			tempObj = (GameObject)Instantiate (pictureButton);
			tempObj.GetComponent<IDStuff> ().SetID (i.ToString());
			tempObj.transform.SetParent (contentPane.transform);
			tempObj.transform.localScale = new Vector3 (1, 1, 1);
			tempObj.SetActive (true);

		}
	}
}
