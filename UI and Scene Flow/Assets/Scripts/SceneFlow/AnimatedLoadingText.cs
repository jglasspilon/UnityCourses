using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Animates the loading text by adding a period until it reaches three then cycles back at zero
//This is just one possible loading feedback option. It can be anything from a loading bar, to an animated image
public class AnimatedLoadingText : MonoBehaviour {

    //related to the animated text
    public float animationSpeed = 0.5f;                 //seconds between each period is added
    private string loadingText = "LOADING";             //text to display
    private Coroutine animationRoutine;                 //keeps track of the coroutine that animates the text
    private Text text;

    private void Awake()
    {
        text = GetComponent<Text>();
        text.text = loadingText;
    }

    //Called when the script is enabled
    private void OnEnable()
    {
        animationRoutine = StartCoroutine(AnimateLoadingText());
    }

    //Called when the script is disabled (when set to inactive or destroyed)
    private void OnDisable()
    {
        StopCoroutine(animationRoutine);
        loadingText = "LOADING";
    }

    //Coroutine that animates the text
    private IEnumerator AnimateLoadingText()
    {
        //keeps track of how many periods there are
        int count = 0;

        while (true)
        {
            //if there are less than 3 periods then add one and increase the count
            if(count < 3)
            {
                loadingText += ".";
                count++;
            }

            //otherwise reset the text and counter
            else
            {
                loadingText = "LOADING";
                count = 0;
            }

            //update the displayed text
            text.text = loadingText;

            //wait [animationSpeed] second before reiterating in the loop
            yield return new WaitForSeconds(animationSpeed);
        }
    }
}
