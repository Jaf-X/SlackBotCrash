using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Threading;
using SlackAPI;


namespace Slack_Crash_Bot
{
    class Program
    {
        
        static void Main(string[] args) => new Program().Start(args);
        public string AUTHTOKEN;
        public string[] userbase;
        Action<LoginResponse> Connected;
        public SlackSocketClient client;
        public string DefaultChannel;


        enum MessageType
        {
            Crash,
            Error,
            Warning,
            Info
        }

        void Start(string[] args)
        {
            
            var MyIni = new IniFile("Settings.ini");



            if(!MyIni.KeyExists("Token"))
            {
                MyIni.Write("Token", "EnterTokenHere");
                AUTHTOKEN = MyIni.Read("Token");
            }
            else 
            {
                AUTHTOKEN = MyIni.Read("Token");
            }

            if (!MyIni.KeyExists("Userbase"))
            {
                MyIni.Write("Userbase", "User1,User2,User3");
                userbase = MyIni.Read("Userbase").Split(',');
            }
            else
            {
                userbase = MyIni.Read("Userbase").Split(',');
            }
            if (!MyIni.KeyExists("DefaultChannel"))
            {
                MyIni.Write("DefaultChannel", "general");
                DefaultChannel = MyIni.Read("DefaultChannel");
            }
            else
            {
                DefaultChannel = MyIni.Read("DefaultChannel");
            }



            client = new SlackSocketClient(AUTHTOKEN);
            // Create seperate thread for the head loop to prevent of Thread.Sleep
            Thread headloop = new Thread(HeadLoop);
            headloop.Start();
        }
        void HeadLoop()
        {
            while (true)
            {
                try
                {
                    switch(Console.ReadLine().ToString().ToLower())
                    {
                      
                        case "stop":
                            Environment.Exit(Environment.ExitCode);
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
                Thread.Sleep(200);
            }
        }
        

        /// <summary>
        /// Sends out a message to the servercrashbot channel.
        /// </summary>
        /// <param name="Message"> The message that is going to be sent</param>
        /// <param name="client"> The SlackSocketClient that is used to connect</param>
        /// <param name="Channelname">Name of the channel you want to send it to</param>
        void SendToLogChat(string Message, SlackSocketClient client, MessageType type)
        {
            Connected = null;
            client.Connect(Connected =>
            {
                var c = client.Channels.Find(x => x.name.Equals(DefaultChannel));
                switch (type)
                {
                    case MessageType.Crash:
                        client.SendMessage(null, c.id, "Crash : " + Message);
                        for (int i = 0; i < userbase.Length; i++)
                        {
                            var user = client.Users.Find(x => x.name.Equals(userbase[i])); // everyone is the userbase

                            if (user != null)
                            { 
                                var dmchannel = client.DirectMessages.Find(x => x.user.Equals(user.id));
                                if (user != null && dmchannel != null)
                                    MessageTo(Message, client, dmchannel, type);
                            }
                           
                        }
                        break;
                    case MessageType.Error:
                        client.SendMessage(null, c.id, "Error : " + Message);
                        break;
                    case MessageType.Info:
                        client.SendMessage(null, c.id, "Info : " + Message);
                        break;
                    case MessageType.Warning:
                        client.SendMessage(null, c.id, "Warning : " + Message);
                        break;
                }
            

            });
        }
        /// <summary>
        /// Simplified message to a user
        /// </summary>
        /// <param name="Message">String :"Message"</param>
        /// <param name="client">client</param>
        /// <param name="userchannelid">userchannelid</param>
        void MessageTo(string Message, SlackSocketClient client, DirectMessageConversation userchannelid, MessageType type)
        {
            Connected = null;
            client.Connect(Connected =>
            {
                if (type != MessageType.Crash)
                    client.SendMessage(null, userchannelid.id, Message);
                else
                    client.SendMessage(null, userchannelid.id, "Crash :" + Message);

            });
        }

    }

 }
