using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO;

public class TakePicture : MonoBehaviour {

	private WebCamTexture webCamTex;
	private bool first;
	private string txtPath;

	// Use this for initialization
	void Start () {
		first = true;
		Application.RequestUserAuthorization (UserAuthorization.WebCam);

		txtPath = Application.persistentDataPath + "/";
	}

	// Update is called once per frame
	void Update () {
		if (Application.HasUserAuthorization (UserAuthorization.WebCam) && first == true) {
			webCamTex = new WebCamTexture ();
			webCamTex.Play ();
			first = false;
		}
		if (first == false) {
			GetComponent<RawImage> ().texture = webCamTex;
		}
	}

	public void SnapPicture() {
		StartCoroutine (SnapIt ());
	}

	private IEnumerator SnapIt() {
		//Wait till end of frame
		yield return new WaitForEndOfFrame();

		//Create a texture the size of the area
		Texture2D tex = new Texture2D(webCamTex.width, webCamTex.height, TextureFormat.RGB24, false);

		//Read screen contents into the texture
		tex.ReadPixels(new Rect(0,0, webCamTex.width, webCamTex.height), 0, 0);
		tex.Apply ();

		//Encode texture into PNG
		byte[] bytes = tex.EncodeToPNG();
		Destroy (tex);

		//Save PNG
		PlayerPrefs.SetInt("picAmount", PlayerPrefs.GetInt("picAmount") + 1);
		File.WriteAllBytes(txtPath + PlayerPrefs.GetString("WOID") + "_Picture" + PlayerPrefs.GetInt("picAmount") + ".png", bytes);
	}

	public void Endit() {
		webCamTex.Stop ();
	}
}
