using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace TCPClient
{
    public class Shell
    {
        public Process CMD { get; private set; }
        public bool Alive = false;

        public Queue StdOutput = new Queue();
        public Queue StdInput = new Queue();

        private const char _TERMINATOR = '\uffff';
        private Thread[] StdioThreads = new Thread[3];

        public Shell()
        {
            #region process
            CMD = new Process()
            {
                StartInfo =
                {
                    FileName = @"cmd.exe",
                    UseShellExecute = false,
                    CreateNoWindow = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    RedirectStandardInput = true,
                },
            };
            #endregion

            StdioThreads[0] = new Thread(() => ReadStdOutput(CMD));
            StdioThreads[1] = new Thread(() => WriteStdInput(CMD));
            StdioThreads[2] = new Thread(() => ReadStdError(CMD));
        }

        public void Start()
        {
            CMD.Start();
            Alive = true;

            foreach (Thread thread in StdioThreads)
                thread.Start();
        }

        public void Stop()
        {
            CMD.Kill();
            Alive = false;

            foreach (Thread thread in StdioThreads)
                thread.Abort();
        }

        private void WriteStdInput(Process procHandle)
        {
            using (StreamWriter sw = procHandle.StandardInput)
            {
                while (Alive)
                {
                    while (StdInput.Count > 0)
                    {
                        sw.Write(StdInput.Peek());
                        StdInput.Dequeue();
                    }
                }
            }
        }

        private void ReadStdError(Process procHandle)
        {
            using (StreamReader streamSTDE = procHandle.StandardError)
            {
                while (Alive)
                {
                    char charBuffer;
                    string line = string.Empty;

                    while ((charBuffer = (char)streamSTDE.Read()) != _TERMINATOR)
                    {
                        line = string.Format("{0}{1}", line, charBuffer);

                        if (line.Contains("\r\n"))
                        {
                            StdOutput.Enqueue(line);
                            line = string.Empty;
                        }

                        Thread.Sleep(1);
                    }

                    Thread.Sleep(1);
                }
            }

        }

        private void ReadStdOutput(Process procHandle)
        {
            Alive = true;

            using (StreamReader streamSTDO = procHandle.StandardOutput)
            {
                while (Alive)
                {
                    char charBuffer;
                    string line = string.Empty;

                    while ((charBuffer = (char)streamSTDO.Read()) != _TERMINATOR)
                    {
                        line = string.Format("{0}{1}", line, charBuffer);

                        if (line.Contains("\r\n") || line.EndsWith(">") || line.EndsWith("\n"))
                        {
                            StdOutput.Enqueue(line);

                            line = string.Empty;
                        }

                        Thread.Sleep(1);
                    }

                    Thread.Sleep(1);
                }
            }

        }
    }
}
