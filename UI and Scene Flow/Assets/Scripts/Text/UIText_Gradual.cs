using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//extends the UIText_Simple class and displays each string one at a time gradually (letters appear as if being typed)
public class UIText_Gradual : UIText_Simple {

    //related to text rolling (gradual appearance of string)
    public float textRollSpeed = 0.1f;                          //the speed at which the text will appear (time between each letter appearing)
    private Coroutine rollingTextRoutine;                       //keeps track of the active coroutine controlling the text roll
    private string currentTextString = "";                      //current string that should be displayed

    public override void UpdateTextSheet(TextSheet newText)
    {
        base.UpdateTextSheet(newText);

        //resets the current string
        currentTextString = "";
        
        //if there is already a text roll coroutine running then stop it
        if(rollingTextRoutine != null)
        {
            StopCoroutine(rollingTextRoutine);
        }

        //start the new text roll coroutine and store it
        rollingTextRoutine = StartCoroutine(RollText());
    }

    public override void CycleText()
    {
        //if there is no current text sheet then we can't cycle
        if(textSheet == null)
        {
            return;
        }

        //if there is a currently active text roll coroutine then stop it, remove it from memory and set the full string from the text sheet to be displayed
        //this acts as a skip feature for the text roll
        if(rollingTextRoutine != null)
        {
            StopCoroutine(rollingTextRoutine);
            rollingTextRoutine = null;
            text.text = textSheet.textCollection[textIndex];
        }

        else
        {
            //if we have more texts to go through than go to the next text, reset the current text string and start a new text roll coroutine
            if (textIndex < textSheet.textCollection.Length - 1)
            {
                textIndex++;
                currentTextString = "";
                rollingTextRoutine = StartCoroutine(RollText());
            }

            //otherwise we are at the end of the text sheet so close the text box
            else
            {
                textGroup.alpha = 0;
                textGroup.blocksRaycasts = false;
            }
        }
    }

    //Text roll coroutine that displays the text gradually one letter at a time
    private IEnumerator RollText()
    {
        for(int i = 0; i < textSheet.textCollection[textIndex].Length; i++)
        {
            currentTextString += textSheet.textCollection[textIndex][i];
            text.text = currentTextString;
            yield return new WaitForSeconds(textRollSpeed);
        }

        rollingTextRoutine = null;
    }
}
