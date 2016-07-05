using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;

public class TakePicture : MonoBehaviour {

	public GameObject button1, button2;

	private WebCamTexture webCamTex;
	private bool first, done;
	private string txtPath;

	// Use this for initialization
	void Start () {
		first = true;
		done = false;
			
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
		if (done == true) {
			button1.SetActive (true);
			button2.SetActive (true);
			done = false;
		}
	}

	public void SnapPicture() {
		//Turn off stuff to take picture
		button1.SetActive (false);
		button2.SetActive (false);

		StartCoroutine (SnapIt ());
	}

	private IEnumerator SnapIt() {
		//Wait till end of frame
		yield return new WaitForEndOfFrame();

		//Create a texture the size of the area
		Texture2D tex = new Texture2D(Camera.main.pixelWidth, Camera.main.pixelHeight, TextureFormat.RGB24, false);

		//Read screen contents into the texture
		tex.ReadPixels(new Rect(0,0, Camera.main.pixelWidth, Camera.main.pixelHeight), 0, 0);
		tex.Apply ();

		//Encode texture into PNG
		byte[] bytes = tex.EncodeToPNG();
		Destroy (tex);

		//Save PNG
		PlayerPrefs.SetInt("picAmount", PlayerPrefs.GetInt("picAmount") + 1);
		File.WriteAllBytes(txtPath + PlayerPrefs.GetString("WOID") + "_Picture" + PlayerPrefs.GetInt("picAmount") + ".png", bytes);

		done = true;
	}

	public void Endit() {
		webCamTex.Stop ();
		SceneManager.LoadScene ("NewWorkAuthorizationMenu");
	}
}
