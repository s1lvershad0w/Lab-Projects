using System;
using System.Reflection;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

public class Program
{
	[DllImport("kernel32.dll")]
	public static extern IntPtr OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);
	
	[DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
	public static extern IntPtr VirtualAllocEx(IntPtr hProcess, IntPtr lpAddress, uint dwSize, uint flAllocationType, uint flProtect);
	
	[DllImport("kernel32.dll", SetLastError = true)]
	public static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, uint nSize, out UIntPtr lpNumber);
	
	const int PROCESS_CREATE_THREAD = 0X0002;
	const int PROCESS_QUERY_INFORMATION = 0X0400;
	const int PROCESS_VM_OPERATION = 0x008;
	const int PROCESS_VM_WRITE = 0x0020;
	const int PROCESS_VM_READ = 0x0010;
	
	const uint MEM_COMMIT = 0x00001000;
	const uint MEM_RESERVE = 0x00002000;
	const uint PAGE_READWRITE = 0x40;
	
	public static void Main()
	{
		Console.WriteLine("[+] Process List\n");
		
		
		Process[] procs = Process.GetProcesses();
		foreach (Process proc in procs)
		{
			try 
			{
				Console.WriteLine("Name: {0}\t\t PID: {1}", proc.ProcessName, proc.Id);
			}
			
			catch
			{
				Console.WriteLine("[X] Process listing error!");
				continue;
				
			}
		}
		
		Console.WriteLine("\n\n Enter PID: ");
		int val;
		val = Convert.ToInt32(Console.ReadLine());
		
		Process proc1 = Process.GetProcessById(val);
		
		
		Console.WriteLine("Obtaining a HANDLE to {0}", proc1.ProcessName);
		IntPtr hProcess = OpenProcess(PROCESS_CREATE_THREAD | PROCESS_QUERY_INFORMATION | PROCESS_VM_OPERATION | PROCESS_VM_READ | PROCESS_VM_WRITE, false, proc1.Id);
		Console.WriteLine("\n[+] OpenProcess : {0}", hProcess);
		
		string blob = "PENTESTER_ACADEMY";
		
		IntPtr memaddr = VirtualAllocEx(hProcess, IntPtr.Zero, (uint)((blob.Length + 1) * Marshal.SizeOf(typeof(char))), MEM_COMMIT | MEM_RESERVE, PAGE_READWRITE);
		Console.WriteLine("\n[+] VirtualAllocEx : {0}", memaddr);
		
		UIntPtr bytesWritten;
		bool res = WriteProcessMemory(hProcess, memaddr, Encoding.Default.GetBytes(blob), (uint)((blob.Length + 1) * Marshal.SizeOf(typeof(char))), out bytesWritten);
		Console.WriteLine("\n[+] WriteProcessMemory : {0}", res);		
		
	}
}