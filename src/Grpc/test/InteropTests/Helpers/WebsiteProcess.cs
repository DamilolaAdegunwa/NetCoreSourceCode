// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Internal;
using Xunit.Abstractions;

namespace InteropTests.Helpers
{
    public class WebsiteProcess : IDisposable
    {
        private readonly Process _process;
        private readonly ProcessEx _processEx;
        private readonly TaskCompletionSource<object> _startTcs;
        private readonly StringBuilder _consoleOut = new StringBuilder();
        private static readonly Regex NowListeningRegex = new Regex(@"^\s*Now listening on: .*:(?<port>\d*)$");

        public string ServerPort { get; private set; }

        public WebsiteProcess(string path, ITestOutputHelper output)
        {
            _process = new Process();
            _process.StartInfo = new ProcessStartInfo
            {
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                FileName = "dotnet",
                Arguments = path
            };
            _process.EnableRaisingEvents = true;
            _process.OutputDataReceived += Process_OutputDataReceived;
            _process.Start();

            _processEx = new ProcessEx(output, _process);

            _startTcs = new TaskCompletionSource<object>(TaskCreationOptions.RunContinuationsAsynchronously);
        }

        public Task WaitForReady()
        {
            if (_processEx.HasExited)
            {
                return Task.FromException(new InvalidOperationException("Server is not running."));
            }

            return _startTcs.Task;
        }

        private void Process_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            var data = e.Data;
            if (data != null)
            {
                _consoleOut.AppendLine(data);
                var m = NowListeningRegex.Match(data);
                if (m.Success)
                {
                    ServerPort = m.Groups["port"].Value;
                }

                if (data.Contains("Application started. Press Ctrl+C to shut down."))
                {
                    _startTcs.TrySetResult(null);
                }
            }
        }

        public void Dispose()
        {
            var attributes = Assembly.GetExecutingAssembly()
                .GetCustomAttributes<AssemblyMetadataAttribute>();
            var serverLogPath = attributes.SingleOrDefault(a => a.Key == "ServerLogPath")?.Value;
            if (!string.IsNullOrEmpty(serverLogPath))
            {
                File.WriteAllText(serverLogPath, _consoleOut.ToString());
            }
            else
            {
                var logDir = Path.Combine(Directory.GetCurrentDirectory(), "artifacts", "logs");
                Directory.CreateDirectory(logDir);
                File.WriteAllText(Path.Combine(logDir, "InteropServer.log"), _consoleOut.ToString());
            }
            _processEx.Dispose();
        }
    }
}
