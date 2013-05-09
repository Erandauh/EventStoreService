﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;

namespace EventStoreService
{
    public class EventStoreProcessWrapper
    {
        private readonly List<Process> _processes;
        private readonly string _filePath;
        private readonly IPAddress _address;
        private readonly ServiceInstance _instance;

        public EventStoreProcessWrapper(string filePath, IPAddress address, ServiceInstance instance)
        {
            _filePath = filePath;
            _address = address;
            _instance = instance;
            _processes = new List<Process>();
        }

        public void Start()
        {
            var info = _instance.GetProcessStartInfo(_filePath, _address);

            var process = new Process { StartInfo = info, EnableRaisingEvents = true };

            //                string name = instance.Name.ToLowerInvariant();
            //                DataReceivedEventHandler outputHandler = (s, e) => File.AppendAllLines(string.Format("{0}-output.log", name), e.Data.Split(Environment.NewLine.ToCharArray()));
            //                DataReceivedEventHandler errorHandler = (s, e) => File.AppendAllLines(string.Format("{0}-error.log", name), e.Data.Split(Environment.NewLine.ToCharArray()));
            //                process.ErrorDataReceived += errorHandler;
            //                process.OutputDataReceived += outputHandler;

            process.Start();
            process.BeginErrorReadLine();
            process.BeginOutputReadLine();

            process.Exited += (sender, args) => Stop();
            _processes.Add(process);
        }

        public void Stop()
        {
            _processes.ForEach(p =>
            {
                p.Refresh();

                if (p.HasExited) return;

                p.Kill();
                p.WaitForExit(TimeSpan.FromSeconds(10).Milliseconds);
                p.Dispose();
            });
        }
    }
}