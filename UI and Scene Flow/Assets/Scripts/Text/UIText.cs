using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//base class for all text UI elements
public abstract class UIText : MonoBehaviour {

    //text sheet that holds the text data for the current text box
    protected TextSheet textSheet;

    /// <summary>
    /// Updates the text with a new sheet
    /// </summary>
    /// <param name="text">new text sheet to set</param>
    public virtual void UpdateTextSheet(TextSheet newText)
    {
        textSheet = newText;
    }

    /// <summary>
    /// Cycles to the next text in the text collection
    /// </summary>
    public abstract void CycleText();
}
