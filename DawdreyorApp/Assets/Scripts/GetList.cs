using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GetList : MonoBehaviour {

	public Toggle headTog;
	public GameObject contentObj;

	private Toggle tempTog;
	private TextAsset txt;
	private string content;
	private int[] headerPos;
	private char[] hashtag;
	private int onNum, increase;

	// Use this for initialization
	void Start () {
		//load shit
		hashtag = new char[1];
		headerPos = new int[100];
		headerPos [0] = -1;
		txt = Resources.Load("Objects") as TextAsset;
		content = txt.text;
		hashtag[0] = '#';

		//Get all # positions
		do {
			onNum += 1;
			headerPos [onNum] = content.IndexOfAny (hashtag, headerPos [onNum - 1] + 1);
		} while (headerPos [onNum] != -1);

		for (int i=1; i <= onNum-2; i+=2) {
			increase += 1;
			tempTog = (Toggle)Instantiate(headTog, new Vector3(headTog.transform.position.x, headTog.transform.position.y - 20*increase, headTog.transform.position.z), Quaternion.identity);
			tempTog.transform.SetParent (contentObj.transform);
			tempTog.GetComponentInChildren<Text>().text = content.Substring (headerPos [i]+1, ((headerPos [i + 1]-1) - (headerPos[i])));
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
