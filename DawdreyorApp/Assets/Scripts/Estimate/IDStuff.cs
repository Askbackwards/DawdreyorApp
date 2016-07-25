using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.IO;

public class IDStuff : MonoBehaviour {

	private string ID, txtPath;

	public GameObject drawing, scrollView;

	void Start () {
		txtPath = Application.persistentDataPath + "/";
	}

	void Update () {

	}

	public void SetID(string WONum) {
		ID = WONum;
	}

	public void SelectPic() {
		byte[] data = File.ReadAllBytes(txtPath + "Estimate_" + PlayerPrefs.GetString("EstimateNumber") + "_Picture" + ID + ".png");
		Texture2D texture = new Texture2D (64, 64, TextureFormat.ARGB32, false);
		texture.LoadImage (data);
		drawing.GetComponent<RawImage> ().texture = texture;
		scrollView.SetActive (false);
	}
}
