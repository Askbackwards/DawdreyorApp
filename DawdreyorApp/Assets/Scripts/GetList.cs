using UnityEngine;
using System.Collections;

public class GetList : MonoBehaviour {

	private TextAsset txt;
	private string content;

	// Use this for initialization
	void Start () {
		//Read Text File and get Objects
		txt = (TextAsset)Resources.Load("Objects.txt", typeof(TextAsset));
		content = txt.text;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
