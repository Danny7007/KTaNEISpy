using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestModScript : MonoBehaviour {
	public KMSelectable btn;
	// Use this for initialization
	void Start () {
		btn.OnInteract += delegate () { GetComponent<KMBombModule>().HandlePass(); return false; };
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
