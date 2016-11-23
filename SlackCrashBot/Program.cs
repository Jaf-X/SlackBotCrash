using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Threading;
using SlackAPI;


namespace SlackCrashBot
{
    class Program
    {
        public string AUTHTOKEN = "xoxb-106585938448-78NFQiiNKP7IN23s1McXlc8V";
        public bool Status;
        public string IP = "localhost";
        Action<LoginResponse> connected;


        public SlackSocketClient client;


        static void Main(string[] args) => new Program().Start(args);





        void Start(string[] args)
        {
            client = new SlackSocketClient(AUTHTOKEN);
            // Create seperate thread for the head loop to prevent of Thread.Sleep
            Thread headloop = new Thread(HeadLoop);
            headloop.Start();
            //Thread CheckStat = new Thread(() => CheckStatus(client));
            //CheckStat.Start();

            //client.OnMessageReceived += (message) =>
            //{
            //    // Handle each message as you receive them
            //    switch (message.text.ToLower())
            //    {
            //        case "":

            //            break;
            //    }
            //};

            // If in case something needs to be handled via Direct Message commands.

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
                        //case "":
                        //    break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
        }
        


        void SendToLogChat(string Message,SlackSocketClient client)
        {
            connected = null;
            client.Connect(connected =>
            {
                var c = client.Channels.Find(x => x.name.Equals("servercrashbot"));
            client.SendMessage(null , c.id, Message);

            });
        }

        void MessageTo(string Message, SlackSocketClient client, DirectMessageConversation userchannelid)
        {
            connected = null;
            client.Connect(connected =>
            {
                client.SendMessage(null, userchannelid.id, Message);

            });
        }

    }

 }
