using UnityEngine;
using System.Collections;

public class ScreenOrient : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void ReturnScreenToPortrait() {
		Screen.orientation = ScreenOrientation.Portrait;
	}

	public void SendToLandscape() {
		Screen.orientation = ScreenOrientation.LandscapeRight;
	}
}
