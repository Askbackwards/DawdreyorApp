using UnityEngine;
using System.Collections;

public class Signature : MonoBehaviour {

	public GameObject LRobj;

	private LineRenderer LR;
	private int curVtex;

	// Use this for initialization
	void Start () {
		LR = LRobj.GetComponent<LineRenderer> ();
		curVtex = 2;
		Screen.orientation = ScreenOrientation.LandscapeRight;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.touchCount > 0 && Input.GetTouch (0).phase == TouchPhase.Moved) {
			// Get movement of the finger since last frame
			Vector3 touchDeltaPosition = Camera.main.ScreenToWorldPoint(new Vector3 (Input.GetTouch (0).position.x, Input.GetTouch(0).position.y, 0f));

			Debug.Log (touchDeltaPosition);
			if (Input.GetTouch (0).deltaPosition.x > .1f) {
				//Move the line
				curVtex += 1;
				LR.SetVertexCount (curVtex);

				LR.SetPosition (curVtex, new Vector3 (1, 1, 0));
			}
		}
	}
}
