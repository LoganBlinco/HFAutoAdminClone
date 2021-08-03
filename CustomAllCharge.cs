using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomAllCharge : MonoBehaviour
{

    private static float startVal = AutoAdmin.liveTimer - AutoAdmin.allChargeTimeTrigger;


    private static float defaultPercentage = 0.25f;

    private static float c = -1;
    private static float b = (-32 * defaultPercentage + 8 * defaultPercentage * Mathf.Sqrt(17)) / (defaultPercentage * defaultPercentage);
    private static float a = -b * b / 8;


    private static float k_temp = -(a / 3 * (defaultPercentage * defaultPercentage * defaultPercentage - 1) + b / 2 * (defaultPercentage * defaultPercentage - 1) - defaultPercentage - c);
    private static float k_mod = 12;
    private static float k_beta = startVal * k_temp / k_mod;

    private static float t_lam = -135;
    //value comes from sum of time change from timeMax to timeMin [value obtained via intergral from tMax to tMin]
    //makes it so without any kills, all charge will get ran at timeMin all kills will then push it forward/back.
    private static float t_alpha = startVal / (t_lam * (Mathf.Exp(startVal / t_lam) - 1));

    public static void AllCharge_CustomSystemInitiate()
    {
        defaultPercentage = AutoAdmin.allChargeMinPercentageAlive;

        startVal = AutoAdmin.liveTimer - AutoAdmin.allChargeTimeTrigger;
        AutoAdmin.allChargeActivityVal = startVal;

        b = (-32 * defaultPercentage + 8 * defaultPercentage * Mathf.Sqrt(17)) / (defaultPercentage * defaultPercentage);
        a = -b * b / 8;

        k_temp = -(a / 3 * (defaultPercentage * defaultPercentage * defaultPercentage - 1) + b / 2 * (defaultPercentage * defaultPercentage - 1) - defaultPercentage - c);
        k_beta = startVal * k_temp / k_mod;
    }



    public static float getKillChange(float x)
    {
        return k_beta * (a * x * x + b * x + c);
    }
    public static float getTimeChange(int cTime)
    {
        return t_alpha * (Mathf.Exp((cTime - AutoAdmin.allChargeTimeTrigger) / t_lam));
    }

    //Check occurs when a kill happens or a new second occurs
    public static void AllCharge_CustomSystemCheck()
    {
        if (AutoAdmin.allChargeState > 0) { return; }

        if (AutoAdmin.allChargeActivityVal == -1) { Debug.Log("never initialized. Error"); return; }

        //ac should be called.

        //wait delay seconds then, then warning then all charge
        var timeForWarning = AutoAdmin.currentTime - AutoAdmin.allChargeTriggerDelay;
        var acTime = timeForWarning - AutoAdmin.allChargeVisableWarning;

        float minutes = Mathf.Floor(acTime / 60); //could also use int division 
        float seconds = acTime - 60 * minutes; //could also use mod.

        string command = string.Format("delayed {0} broadcast {3} All charge at {1}:{2}", timeForWarning, minutes, seconds, AutoAdmin.MESSAGE_PREFIX);
        AutoAdmin.f1MenuInputField.onEndEdit.Invoke(command);
        command = string.Format("delayed {0} broadcast {1} {2};set allowFiring false", AutoAdmin.allChargeTimeTrigger, AutoAdmin.MESSAGE_PREFIX, AutoAdmin.allChargeMessage);
        AutoAdmin.f1MenuInputField.onEndEdit.Invoke(command);
    }

}
