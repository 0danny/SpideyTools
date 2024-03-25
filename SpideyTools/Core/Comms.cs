using SpideyTools.Core.Helper;
using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpideyTools.Core
{
    public class Comms
    {
        private static string pipeName = "SpideyPipe";
        private static string serverName = ".";

        public static void send(string message)
        {
            try
            {
                using (NamedPipeClientStream pipeClient = new NamedPipeClientStream(serverName, pipeName, PipeDirection.Out))
                {
                    pipeClient.Connect();
                    Logger.Log("Connected. Sending a message...");

                    message += '\0';
                    byte[] bytesToSend = Encoding.UTF8.GetBytes(message);

                    pipeClient.Write(bytesToSend, 0, bytesToSend.Length);

                    Logger.Log("Message sent.");
                }
            }
            catch (Exception ex)
            {
                Logger.Log($"Error: {ex.Message}");
            }
        }
    }
}
