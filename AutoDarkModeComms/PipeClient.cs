﻿using System;
using System.IO;
using System.IO.Pipes;
using System.Threading.Tasks;
using AutoDarkModeSvc.Communication;

namespace AutoDarkModeComms
{
    public class PipeClient : ICommandClient
    {
        private string PipeName { get; set; }
        public PipeClient(string pipename)
        {
            PipeName = pipename;
        }

        private void PipeMessenger(string message)
        {
            using NamedPipeClientStream pipeClient = new NamedPipeClientStream(".", PipeName + Address.DefaultPipeCommand, PipeDirection.Out);
            pipeClient.Connect(5000);
            using StreamWriter sw = new StreamWriter(pipeClient)
            {
                AutoFlush = true
            };
            sw.WriteLine(message);
        }

        private bool ReceiveReponse()
        {
            bool ok = true;
            using NamedPipeClientStream pipeClient = new NamedPipeClientStream(".", PipeName + Address.DefaultPipeResponse, PipeDirection.In);
            try
            {
                pipeClient.Connect(1000);
                using StreamReader sr = new StreamReader(pipeClient);
                string temp;
                while ((temp = sr.ReadLine()) != null)
                {
                    if (temp.Contains(StatusCode.Err))
                    {
                        ok = false;
                    }
                }
            }
            catch (TimeoutException)
            {
                return false;
            }
            return ok;
        }

        public string SendMessageAndGetReply(string message, int timeoutSeconds)
        {
            throw new NotImplementedException();
        }

        public Task<bool> SendMessageAsync(string message, int timeoutSeconds)
        {
            throw new NotImplementedException();
        }

        public Task<string> SendMessageAndGetReplyAsync(string message, int timeoutSeconds)
        {
            throw new NotImplementedException();
        }

        public string SendMessageWithRetries(string message, int timeoutSeconds = 3, int retries = 3)
        {
            throw new NotImplementedException();
        }
    }
}
