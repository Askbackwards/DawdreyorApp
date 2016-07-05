using UnityEngine;
using System.Collections;
using System.IO;
using UnityEngine.UI;

public class AgreementText : MonoBehaviour {

	private string txtPath;
	private string newText;
	private int n;

	// Use this for initialization
	void Start () {
		txtPath = Application.persistentDataPath + "/" + PlayerPrefs.GetString ("WOID") + "_Info.txt";
		//txtPath = "C:/Users/nomore/Desktop/" + PlayerPrefs.GetString ("WOID") + "_Info.txt";

		string[] oldText = File.ReadAllLines(txtPath);


		for (int i = 0; i < oldText.Length; i++) {
			if (i == 7) {
				newText += "\nFormat = UnitNum, Measurement, Work Performed, Supplies, Quantity\n";
			} else {
				newText += oldText [i] + "\n";
			}

			n = i;
		}

		newText += "\nAgreement: Crews4HIRE, LLC supplied all material and labor, in complete accordance with all above services described. All material is guaranteed to be provided as specified above. All work is completed in a professional manner. Any alteration or deviation from 'work request' specifications involving extra material, additional labor, or any other costs will only be executed upon written orders.\n";
		newText += "\nCustomer Authorization: Signer agrees that all specifications, terms and conditions, and pricing are satisfactory and accepted.";

		GetComponent<Text> ().text = newText;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
