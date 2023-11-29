using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    internal class DialWithPython
    {
        #region Properties
        /// <summary>
        /// Path degli script
        /// </summary>
        private static string? scriptsPath;
        /// <summary>
        /// Directory di lavoro
        /// </summary>
        private static string? workingDir;
        #endregion
        #region Methods
        /// <summary>
        /// Costruttore statico
        /// </summary>
        static DialWithPython()
        {
            workingDir = Path.GetDirectoryName(Environment.ProcessPath);
            while (workingDir != null && !workingDir.ToLowerInvariant().EndsWith("pythonapplication1"))
                workingDir = Path.GetDirectoryName(workingDir);
            if (workingDir != null) {
                workingDir = Path.Combine(workingDir, "PythonApplication1");
                scriptsPath = Path.Combine(workingDir, "env", "Scripts");
            }
        }
        /// <summary>
        /// Avvia il processo python
        /// </summary>
        /// <param name="args">Argomenti dell'interprete</param>
        public static async Task Start(string args = "PythonApplication1.py")
        {
            if (scriptsPath == null)
                throw new DirectoryNotFoundException("Python sources directory not found");
            var startInfo = new ProcessStartInfo(Path.Combine(scriptsPath, "python.exe"));
            startInfo.Arguments = args;
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardInput = true;
            startInfo.RedirectStandardError = true;
            startInfo.WorkingDirectory = workingDir;
            startInfo.EnvironmentVariables["PATH"] = scriptsPath + Path.PathSeparator + startInfo.EnvironmentVariables["PATH"];
            if (Process.Start(startInfo) is var process && process != null) {
                await Task.Run(() =>
                {
                    process.StandardInput.Write("prova\n");
                    var response = process.StandardOutput.ReadLine();
                    if (response == "")
                        response = process.StandardOutput.ReadLine();
                    Console.WriteLine(response);
                    process.StandardInput.Write("prova 2\n");
                    response = process.StandardOutput.ReadLine();
                    if (response == "")
                        response = process.StandardOutput.ReadLine();
                    Console.WriteLine(response);
                    process.StandardInput.WriteLine("quit");
                    process.WaitForExit();
                });
            }
        }
        #endregion
    }
}
