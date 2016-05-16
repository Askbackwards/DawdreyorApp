using UnityEngine;
using System.Collections;

public class GetList : MonoBehaviour {

	private TextAsset txt;
	private string content;
	private int[] headerPos;

	// Use this for initialization
	void Start () {
		//load Text File
		txt = (TextAsset)Resources.Load("Objects.txt", typeof(TextAsset));
		content = txt.text;

		//Search it for headers
		//Use IndexOf(char, Int32) int 32 being starting pos
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
