using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.ComponentModel;
using System.Net.Sockets;
using System.IO;
using UnityEngine.UI;

public class TwitchChat : MonoBehaviour
{

    private TcpClient twitchClient;
    private StreamReader reader;
    private StreamWriter writer;

    public string username, password, channelName; //Get the password from https://twitchapps.com/tmi

    // public Text chatBox;

    void Start()
    {
        Connect();
    }

    void Update()
    {
        if (!twitchClient.Connected)
        {
            Connect();
        }

        ReadChat();
    }

    private void Connect()
    {
        twitchClient = new TcpClient("irc.chat.twitch.tv", 6667);
        reader = new StreamReader(twitchClient.GetStream());
        writer = new StreamWriter(twitchClient.GetStream());

        writer.WriteLine("PASS " + password);
        writer.WriteLine("NICK " + username);
        writer.WriteLine("USER " + username + " 8 * :" + username);
        writer.WriteLine("JOIN #" + channelName);
        writer.Flush();
    }

    private void ReadChat()
    {
        if (twitchClient.Available > 0)
        {
            var message = reader.ReadLine(); //Read in the current message
            if (message.Contains("food"))
            {
                ResourceArea[] resourceAreas = GameObject.FindObjectsOfType<ResourceArea>();

                foreach (var area in resourceAreas)
                {
                    area.feed();
                }
            }

        }
    }

    private void GameInputs(string ChatInputs)
    {
        //     if (ChatInputs.ToLower() == "left")
        //     {
        //         player.AddForce(Vector3.left * (speed * 1000));
        //     }

        //     if (ChatInputs.ToLower() == "right")
        //     {
        //         player.AddForce(Vector3.right * (speed * 1000));
        //     }

        //     if (ChatInputs.ToLower() == "forward")
        //     {
        //         player.AddForce(Vector3.forward * (speed * 1000));
        //     }
        // }
    }
}