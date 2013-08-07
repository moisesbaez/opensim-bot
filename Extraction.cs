using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using OpenMetaverse;
using OpenMetaverse.Assets;

namespace FirstBot
{
    static class Extraction
    {
        private static List<string> foundData = new List<string>();

        public static void DownloadSteg(byte[] fileData, UUID fileID, GridClient Client)
        {
            Console.WriteLine("Found Something!");
            
            string fileName = fileID.ToString() + ".jp2";
            foundData.Add(fileName);

            try
            {
                File.WriteAllBytes(fileName, fileData);
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message, Helpers.LogLevel.Error, Client, ex);
            }
        }

        public static void ExtractSteg()
        {
            Process commandLine = new Process();
            commandLine.StartInfo.FileName = "7za.exe";
            commandLine.StartInfo.UseShellExecute = false;
            commandLine.StartInfo.RedirectStandardOutput = true;

            foreach (string file in foundData)
            {
                commandLine.StartInfo.Arguments = "x " + file;
                commandLine.Start();
                string stringOutput = commandLine.StandardOutput.ReadToEnd();
                commandLine.WaitForExit();
            }
        }
    }
}
