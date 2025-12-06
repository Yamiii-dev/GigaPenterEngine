using GigaPenterEngine.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GigaPenterEngine.Networking.Server
{
    public struct ClientConnectArgs
    {
        public int id;
        public string name;
    }

    public class ServerManager : GameSystem
    {
        // Used by Client object to run packet handlers on the main thread
        private static readonly List<Action> executeOnMainThread = new List<Action>();
        private static readonly List<Action> executeCopiedOnMainThread = new List<Action>();
        private static bool actionToExecuteOnMainThread = false;

        // Initialize server when the server manager gets created
        public ServerManager(int maxPlayers, int port)
        {
            Server.Init(maxPlayers, port);
            Server.ClientConnected += OnClientConnect;
        }

        private void OnClientConnect(ClientConnectArgs args)
        {
            ClientConnected?.Invoke(args);
        }

        public override void Start()
        {
            Server.Start();
        }

        public Action<ClientConnectArgs>? ClientConnected { get; set; }

        /// <summary>Sets an action to be executed on the main thread.</summary>
        /// <param name="_action">The action to be executed on the main thread.</param>
        public static void ExecuteOnMainThread(Action _action)
        {
            if (_action == null)
            {
                Console.WriteLine("No action to execute on main thread!");
                return;
            }

            lock (executeOnMainThread)
            {
                executeOnMainThread.Add(_action);
                actionToExecuteOnMainThread = true;
            }
        }

        /// <summary>Executes all code meant to run on the main thread. NOTE: Call this ONLY from the main thread.</summary>
        public override void Update()
        {
            if (actionToExecuteOnMainThread)
            {
                executeCopiedOnMainThread.Clear();
                lock (executeOnMainThread)
                {
                    executeCopiedOnMainThread.AddRange(executeOnMainThread);
                    executeOnMainThread.Clear();
                    actionToExecuteOnMainThread = false;
                }

                for (int i = 0; i < executeCopiedOnMainThread.Count; i++)
                {
                    executeCopiedOnMainThread[i]();
                }
            }
        }
    }
}
