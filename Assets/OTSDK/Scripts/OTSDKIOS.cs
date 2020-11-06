using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

public class OTSDKIOS : MonoBehaviour {

	// [SerializeField] private bool running = false;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	[DllImport("__Internal")]
	extern static public void StartOTSession(string key, string session, string token,string type, string role);
	[DllImport("__Internal")]
	extern static public void CloseOTSession();
	[DllImport("__Internal")]
	extern static public void ShowSessionView();
	[DllImport("__Internal")]
	extern static public void HideSessionView();
	[DllImport("__Internal")]
	extern static public bool IsCallback1();
	[DllImport("__Internal")]
	extern static public bool IsCallback2();
	[DllImport("__Internal")]
	extern static public bool IsViewHidden();
	[DllImport("__Internal")]
	extern static public bool IsViewShowing();
}
