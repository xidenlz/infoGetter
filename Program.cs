using System;
using System.Diagnostics;
using System.Drawing;
using System.Net.NetworkInformation;


/*Created By Musaed bzteam softwares dev.*/
namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {

            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("    _       ____         ______     __  __           ");
            Console.WriteLine("   (_)___  / __/___     / ____/__  / /_/ /____  _____");
            Console.WriteLine("  / / __ \\/ /_/ __ \\   / / __/ _ \\/ __/ __/ _ \\/ ___/");
            Console.WriteLine(" / / / / / __/ /_/ /  / /_/ /  __/ /_/ /_/  __/ /    ");
            Console.WriteLine("/_/_/ /_/_/  \\____/   \\____/\\___/\\__/\\__/\\___/_/     ");
            Console.WriteLine("\n62 7A 74 65 61 6D 20 73 6F 66 74 77 61 72 65 73");
            Console.WriteLine("version 0.22");
            Console.WriteLine(" - - - - - - - - - - - - - -");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Press left click to pause/unpuase");
            Console.ResetColor();
            Console.WriteLine("Do you want to load all informitons? (y/n)");
            string answer = Console.ReadLine();
            if (answer.ToLower() == "y" || answer.ToLower() == "yes")
            {
                int numOpenPorts = 0;
                IPGlobalProperties ipProperties = IPGlobalProperties.GetIPGlobalProperties();
                TcpConnectionInformation[] tcpConnections = ipProperties.GetActiveTcpConnections();

                foreach (TcpConnectionInformation tcpConnection in tcpConnections)
                {
                    if (tcpConnection.State == TcpState.Established)
                    {
                        numOpenPorts++;
                    }
                }
                int[] suspiciousPorts = new int[] { 22, 23, 80, 443, 4444 };

                foreach (TcpConnectionInformation tcpConnection in tcpConnections)
                {
                    if (tcpConnection.State == TcpState.Established)
                    {
                        int localPort = tcpConnection.LocalEndPoint.Port;
                        int remotePort = tcpConnection.RemoteEndPoint.Port;
                        bool isSuspicious = suspiciousPorts.Contains(localPort) || suspiciousPorts.Contains(remotePort);

                        Console.Write($"{tcpConnection.LocalEndPoint} -> {tcpConnection.RemoteEndPoint}: ");
                        Console.ForegroundColor = isSuspicious ? ConsoleColor.Red : ConsoleColor.Green;
                        Console.WriteLine(tcpConnection.State);
                        Console.ResetColor();
                        Thread.Sleep(100);
                    }
                }
                Console.WriteLine($"Number of open ports: {numOpenPorts}");
                Console.ForegroundColor= ConsoleColor.Green;
                string systemInfo = RunCommand("\nsysteminfo");
                Console.ResetColor();
                WriteAnimated("System Information:\n", systemInfo);
                bool isDefenderEnabled = IsServiceRunning("WinDefend");
                string defenderStatus = isDefenderEnabled ? "Enabled" : "Disabled";
                string defenderInfo = $"Windows Defender: {defenderStatus}\n";
                WriteAnimated("", defenderInfo);
                bool isFirewallEnabled = IsServiceRunning("MpsSvc");
                string firewallStatus = isFirewallEnabled ? "Enabled" : "Disabled";
                string firewallInfo = $"Windows Firewall: {firewallStatus}\n";
                WriteAnimated("", firewallInfo);
                Console.ResetColor();
                Console.ReadLine();

            }
            else
            {
                Console.WriteLine("Aborting process after 3 sec");
                Thread.Sleep(3000);
                Environment.Exit(0);
            }

        }

        static string RunCommand(string command, string arguments = "")
        {
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = command;
            startInfo.Arguments = arguments;
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardOutput = true;

            Process process = Process.Start(startInfo);
            string output = process.StandardOutput.ReadToEnd();
            return output;
        }

        static void WriteAnimated(string title, string text)
        {
            Console.Write(title);
            foreach (char c in text)
            {
                Console.Write(c);
                System.Threading.Thread.Sleep(10);
            }
            Console.WriteLine();
        }
        static bool IsServiceRunning(string serviceName)
        {
            string query = $"Get-Service -Name {serviceName} | Select-Object -ExpandProperty Status";
            string status = RunPowerShellCommand(query);
            return status.Trim().Equals("Running", StringComparison.InvariantCultureIgnoreCase);
        }

        static string RunPowerShellCommand(string command)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = "powershell.exe";
            startInfo.Arguments = $"-NoProfile -ExecutionPolicy unrestricted -Command \"{command}\"";
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardOutput = true;
            Process process = Process.Start(startInfo);
            string output = process.StandardOutput.ReadToEnd();
            process.WaitForExit(); 
            return output;
        }
        

    }
}
