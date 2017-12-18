using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour {

    private UIFader fader;
    private UIText text;

	// Use this for initialization
	void Start () {
        DontDestroyOnLoad(gameObject);

        fader = GetComponentInChildren<UIFader>();

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
