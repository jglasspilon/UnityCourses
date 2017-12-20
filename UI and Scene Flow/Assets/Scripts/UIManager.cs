using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//Simple UI Manager script. Will likely be different depending on the needs of the game
//Currently fades the scene in using the fader upon a scene being loaded 
public class UIManager : MonoBehaviour {

    //reference to the fader UI element
    private UIFader fader;

	//Use this for initialization
	private void Awake () {
        fader = GetComponentInChildren<UIFader>();
        DontDestroyOnLoad(gameObject);
	}

    //Called once the script has been enabled (after awake)
    private void OnEnable()
    {
        //Sets the OnSceneLoaded function to listen to SceneManager for a scene change
        //Delegate subscription, must be unsubscribed on disable
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    //Delegate function called once a scene has been loaded. Must be subscribed to SceneManager.sceneloaded
    //Since the since the subscribed action has 2 parameters <Scene, LoadSceneMode> this function needs those 2 parameters as well even if they aren't used in the logic
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //fade the scene in (fader goes to full transparency)
        fader.FadeSceneIn();
    }

    //called once the script has been disabled (after being set to inactive or destroyed)
    private void OnDisable()
    {
        //Delegate unsubscription, must be subscribed on enable
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
