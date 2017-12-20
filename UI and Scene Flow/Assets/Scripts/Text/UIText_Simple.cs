using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Extends the base UIText class and displays each string from the text sheet one at a time instantly
public class UIText_Simple : UIText {

    //related to the text component
    protected Text text;                      //actual text to be printed in the text box
    protected CanvasGroup textGroup;          //canvas group that controls the text box visibility and interactability
    protected int textIndex = 0;              //keeps track of the current text within the text collection

    // Use this for initialization
    protected virtual void Awake()
    {
        text = GetComponentInChildren<Text>();
        textGroup = GetComponent<CanvasGroup>();
    }

    public override void UpdateTextSheet(TextSheet newText)
    {
        base.UpdateTextSheet(newText);

        //initialize the text component with the new text string
        textIndex = 0;
        text.text = textSheet.textCollection[textIndex];

        //open the text box
        textGroup.alpha = 1;
        textGroup.blocksRaycasts = true;
    }

    public override void CycleText()
    {
        //if there is no current text sheet then we can't cycle
        if(textSheet == null)
        {
            return;
        }

        else
        {
            //if we have more texts to go through than go to the next text
            if(textIndex < textSheet.textCollection.Length - 1)
            {
                textIndex++;
                text.text = textSheet.textCollection[textIndex];
            }

            //otherwise we are at the end of the text sheet so close the text box
            else
            {
                textGroup.alpha = 0;
                textGroup.blocksRaycasts = false;
            }
        }
    }
}
