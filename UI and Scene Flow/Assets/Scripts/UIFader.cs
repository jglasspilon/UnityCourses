using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//UI element that fades in and out the UI
public class UIFader : MonoBehaviour {

    //Related to fading
    public float fadeTime = 2.0f;                   //determines the amount of time the fade will last
    private float fadeSpeed;                        //speed of the fade
    private CanvasGroup group;                      //canvas group that controls the fade

    private Coroutine fadingRoutine = null;         //keeps track of the active fade coroutine
    public bool isFading = false;                   //is the fader currently fading

	// Use this for initialization
	private void Awake () {
        group = GetComponent<CanvasGroup>();

        //calculate the speed of fading by dividing the magnitude of change (1) by the fade time
        fadeSpeed = 1 / fadeTime;
	}

    //Called once every frame
    private void Update()
    {
        //if the fader is transparent then set it so it doesn't block raycasts
        if(group.alpha == 0)
        {
            if(group.blocksRaycasts)
                group.blocksRaycasts = false;
        }

        //otherwise block all raycasts
        else
        {
            if(!group.blocksRaycasts)
                group.blocksRaycasts = true;
        }
    }

    /// <summary>
    /// Fades the scene in (fades the fader to transparent)
    /// </summary>
    public void FadeSceneIn()
    {
        if (fadingRoutine != null)
        {
            StopCoroutine(fadingRoutine);
            isFading = false;
        }
        fadingRoutine = StartCoroutine(SceneFade(0));
    }

    /// <summary>
    /// Fades the scene out (fades the fader to full opacity)
    /// </summary>
    public void FadeSceneOut()
    {
        if(fadingRoutine != null)
        {
            StopCoroutine(fadingRoutine);
            isFading = false;
        }
        fadingRoutine = StartCoroutine(SceneFade(1));
    }

    /// <summary>
    /// Allows the distinct setting of the fader's opacity
    /// </summary>
    /// <param name="alpha">desired opacity level</param>
    public void SetFaderOpacity(int alpha)
    {
        group.alpha = Mathf.Clamp01(alpha);
    }

    //Fades the scene either in or out depending on the target parameter (0 transparent, 1 opaque)
    private IEnumerator SceneFade(int target)
    {
        isFading = true;

        //keeps track of how far along the transition we are
        float counter = 0;

        //starting value of the alpha
        float startValue = group.alpha;

        //as long as the alpha is not approximately equal to the target, move it towards it at a speed of fadespeed scaled by the frame rate
        while(!Mathf.Approximately(group.alpha, target))
        {
            group.alpha = Mathf.Lerp(startValue, target, counter);
            counter += fadeSpeed * Time.deltaTime;

            yield return null;
        }
        group.alpha = target;
        isFading = false;
    }
}
