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
        public string AUTHTOKEN = "xoxb-106585938448-78NFQiiNKP7IN23s1McXlc8V";
       
        Action<LoginResponse> Connected;
        public SlackSocketClient client;
        
        void Start(string[] args)
        {
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
                        case "Send":
                            SendToLogChat("Hello World!",client);
                            break;
                        case "stop":
                            Environment.Exit(Environment.ExitCode);
                            break;
                        //case "":
                        //    break;
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
        /// <param name="Message">Some string</param>
        /// <param name="client">SlackSocketClient IE client</param>
        void SendToLogChat(string Message, SlackSocketClient client)
        {
            Connected = null;
            client.Connect(connected =>
            {
                var c = client.Channels.Find(x => x.name.Equals("servercrashbot"));
            client.SendMessage(null , c.id, Message);

            });
        }
        /// <summary>
        /// Simplified message to a user
        /// </summary>
        /// <param name="Message">String :"Message"</param>
        /// <param name="client">client</param>
        /// <param name="userchannelid">userchannelid</param>
        void MessageTo(string Message, SlackSocketClient client, DirectMessageConversation userchannelid)
        {
            Connected = null;
            client.Connect(connected =>
            {
                client.SendMessage(null, userchannelid.id, Message);

            });
        }

    }

 }
