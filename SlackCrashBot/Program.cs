﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SlackAPI;
using System.Net.NetworkInformation;
using System.Collections;
using System.Threading;

namespace SlackCrashBot
{
    class Program
    {
        public string AUTHTOKEN = "xoxb-106585938448-78NFQiiNKP7IN23s1McXlc8V";

        Action messageRecived;
        public bool Status;
        public string IP = "localhost";
        Action<LoginResponse> connected;
        

        public static bool IsPingable(string nameOrAddress)
        {
            bool pingable = false;
            Ping pinger = new Ping();

          

            try
            {
                PingReply reply = pinger.Send(nameOrAddress);
                pingable = reply.Status == IPStatus.Success;
            }
            catch (PingException)
            {
                // Discard PingExceptions and return false;
            }
            return pingable;
          
        }
        public SlackSocketClient client;


        static void Main(string[] args) => new Program().Start(args);





        void Start(string[] args)
        {


            

            if (client == null)
            {
                client = new SlackSocketClient(AUTHTOKEN);
            }

            Thread headloop = new Thread(HeadLoop);
            
            headloop.Start();
            Thread CheckStat = new Thread(() => CheckStatus(client));
            CheckStat.Start();

            // If in case something needs to be handled via Direct Message commands.
            client.OnMessageReceived += (message) =>
            {
                // Handle each message as you receive them




            };
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

        void CheckStatus(SlackSocketClient client)
        {
           while (true) { 
            if (IsPingable(IP))
            {
                Status = false;
                Send("Server Down @ " + System.DateTime.Now.ToString(),client);
                    Thread.Sleep(10000);
                }
            else
            {
                
                Status = true;
                Thread.Sleep(10000);
            }

            }
          
        }






        void Send(string Message,SlackSocketClient client)
        {
            connected = null;
            client.Connect(connected =>
            {
                var user = client.Users.Find(x => x.name.Equals("antonviklund")); // you can replace slackbot with everyone else here
                var dmchannel = client.DirectMessages.Find(x => x.user.Equals(user.id));
                client.SendMessage(null, dmchannel.id, Message);


            });
        }

    }

 }
