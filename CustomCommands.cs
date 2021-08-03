using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomCommands : MonoBehaviour
{

	static List<int> playersLoggedIn = new List<int>();

	public string[] commands = new string[]
	{
		"delay",
		"ac"
	};

	public static void OnRCCommand(int playerId, string input, string output, bool success)
	{
		if (success) { playersLoggedIn.Add(playerId); }

		if (!playersLoggedIn.Contains(playerId)) { return; }

		//remember this isnt an rc command. This is an internal command by the bot for when a delay is used.
		if (input == "All charge")
		{
			AutoAdmin.isAllCharge = true;
		}


		string[] inputArray = input.Split(' ');
		if (inputArray.Length < 2) { return; }
		if (inputArray[0] != "rc") { return; }
		switch(inputArray[1])
		{
			case "delay":
				command_delay(inputArray);
				break;
			case "ac":
				command_ac(inputArray);
				break;
			default:
				break;
		}
	}

	private static void command_ac(string[] inputArray)
	{
		if (inputArray.Length == 2)
		{
			//just call ac now.
			AutoAdmin.broadcast_prefix(AutoAdmin.allChargeMessage + ";set allowFiring false");
		}
		if (inputArray.Length == 3)
		{
			float time;
			if (float.TryParse(inputArray[2],out time) && time > 0)
			{
				float allChargeMaxTime = AutoAdmin.currentTime - time;

				float minutes = Mathf.Floor(allChargeMaxTime / 60); //could also use int division 
				float seconds = allChargeMaxTime - 60 * minutes; //could also use mod.

				if (AutoAdmin.f1MenuInputField == null) { return; }

				AutoAdmin.broadcast(string.Format("{2} All charge at {0}:{1}", minutes, seconds, AutoAdmin.MESSAGE_PREFIX));
				string command = string.Format("delayed {0} broadcast {1} {2};set allowFiring false", allChargeMaxTime, AutoAdmin.MESSAGE_PREFIX, AutoAdmin.allChargeMessage);
				AutoAdmin.f1MenuInputField.onEndEdit.Invoke(command);
			}
		}
	}

	private static void command_delay(string[] inputArray)
	{
		throw new NotImplementedException();
	}

	internal static void OnRCLogin(int playerId, string inputPassword, bool isLoggedIn)
	{
		playersLoggedIn.Add(playerId);
		throw new NotImplementedException();
	}
}
