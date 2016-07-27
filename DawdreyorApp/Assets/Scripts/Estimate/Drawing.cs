using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.IO;

public class Drawing : MonoBehaviour {

	public GameObject LRobj;
	public Material blackMaterial;

	private LineRenderer LR;
	private int curVtex;
	private bool ready, ready2;
	private GameObject UsingObj;
	private string txtPath;

	// Use this for initialization
	void Start () {
		txtPath = Application.persistentDataPath + "/";
		LR = LRobj.GetComponent<LineRenderer> ();
		curVtex = 0;
		ready = true;
		ready2 = false;
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

		if(ready2)
			SceneManager.LoadScene ("PictureSelection");
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

	public void SaveDrawing() {

		//Scan and save PNG of signature
		ready = false;
		StartCoroutine(SaveSig());
	}

	private IEnumerator SaveSig() {

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
		PlayerPrefs.SetInt(("picAmount2" + PlayerPrefs.GetString("EstimateNumber")), PlayerPrefs.GetInt(("picAmount2" + PlayerPrefs.GetString("EstimateNumber"))) + 1);
		File.WriteAllBytes(txtPath + "Estimate_" + PlayerPrefs.GetString("EstimateNumber") + "_Picture" + PlayerPrefs.GetInt(("picAmount2" + PlayerPrefs.GetString("EstimateNumber"))) + ".png", bytes);

		ready2 = true;
	}
}
