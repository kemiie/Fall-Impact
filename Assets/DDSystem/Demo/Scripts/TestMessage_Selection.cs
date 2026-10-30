using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Doublsb.Dialog;

public class TestMessage_Selection : MonoBehaviour
{
    public DialogManager DialogManager;

    private void Awake()
    {
        var dialogTexts = new List<DialogData>();

        var Text1 = new DialogData("Do you know where you are?");
        Text1.SelectList.Add("Correct", "no");
        Text1.SelectList.Add("Wrong", "yes");
        Text1.SelectList.Add("Whatever", "Why should I care?");

        Text1.Callback = () => Check_Correct();

        dialogTexts.Add(Text1);

        DialogManager.Show(dialogTexts);
    }

    private void Check_Correct()
    {
        if(DialogManager.Result == "Correct")
        {
            var dialogTexts = new List<DialogData>();

            dialogTexts.Add(new DialogData("You are right. I am EESUIA... Here to guide you..."));

            DialogManager.Show(dialogTexts);
        }
        else if (DialogManager.Result == "Wrong")
        {
            var dialogTexts = new List<DialogData>();

            dialogTexts.Add(new DialogData("You are wrong. Find where you are."));

            DialogManager.Show(dialogTexts);
        }
        else
        {
            var dialogTexts = new List<DialogData>();

            dialogTexts.Add(new DialogData("Right. You don't have to get the answer. Be that way, you'll need my help."));

            DialogManager.Show(dialogTexts);
        }
    }
}
