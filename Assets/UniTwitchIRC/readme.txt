UniTwitch IRC Version 1.0.0 11/29/2015

GENERAL USAGE NOTES
==============================================
Allows for any Internet Relay Chat client to facilitate communication.  The API and accompanying components are tuned to connect to the Twitch.tv chat room.  Add a general usage prefab, enter in the required credentials and press play.  Yes is is that easy because all the complication and sophistication is abstracted away and the high level components are exposed and awaiting configuration.

INSTALLATION
==============================================
-Down the package from Unity Asset Store
-Import all the assets
-Put the UniTwitch IRC Basic Example prefab into the scene
-Enter in the required credentials and press play

EXAMPLE
==============================================
The example below show how to create a custom player spawner from the provided API.
----------------------------------------------
public sealed class BallSpawner : TwitchSpawner
{
    protected override void PlayerPrefabInstantiated(TwitchPlayerController pc, GameObject pGO)
    {
        base.PlayerPrefabInstantiated(pc, pGO);

        IPlayerUI playerUI = pGO.GetComponent<IPlayerUI>();
        if(playerUI != null)
        {
            pGO.GetComponent<IPlayerUI>().pc = pc;
            pGO.name = string.Concat(pGO.name, ".", pc.nick);
        }
    }
}
----------------------------------------------

CONTACT
==============================================
Developer: caLLowCreation
Author: Jones S. Cropper
E-mail: callowcreation@gmail.com 
Subject: UniTwitch IRC
Website: www.caLLowCreation.com
