using Microsoft.Win32;
using System;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;


namespace WpfHelloWorld.WinUtils
{
    /// <summary>
    /// HEY!  You should do this kind of thing via an installer or something, you lazy hacker!
    /// </summary>
    public class JkhFileAssociation
    {
        //https://stackoverflow.com/a/60322339
        public static void RegisterFileExtensionForCurrentUser(string extension, string applicationPath)
        {
            if (string.IsNullOrEmpty(extension))
                return;   //also, what did you THINK would happen?

            if (!extension.StartsWith("."))
                extension = $".{extension}";

            RegistryKey? FileReg = Registry.CurrentUser.CreateSubKey("Software\\Classes\\" + extension);
            FileReg.CreateSubKey("shell\\open\\command").SetValue("", $"\"{applicationPath}\" \"%1\"");
            FileReg.Close();

            SHChangeNotify(0x08000000, 0x0000, IntPtr.Zero, IntPtr.Zero);
        }

        //CREDITS: http://mel-green.com/2009/04/c-set-file-type-association/
        // AdminAssociateAllUsers file extension with progID, description, icon and application
        public static bool AdminAssociateAllUsers(string extension, string progID, string applicationPath, string description, string iconPath, int iconIndex)
        {
            if (string.IsNullOrEmpty(extension))
                return false;   //duh

            if(!extension.StartsWith("."))
                extension = $".{extension}";

            bool retval = false;

            RegistryKey? userClassesRoot = Registry.ClassesRoot;

            if (IsUserAdministrator())
            {
                userClassesRoot.CreateSubKey(extension).SetValue("", progID);
                if (progID != null && progID.Length > 0)
                {
                    using (RegistryKey? key = userClassesRoot.CreateSubKey(progID))
                    {
                        if (!string.IsNullOrEmpty(description))
                            key.SetValue("", description);

                        if (!string.IsNullOrEmpty(applicationPath))
                            key.CreateSubKey(@"Shell\Open\Command").SetValue("", applicationPath + " \"%1\"");
                        //							key.CreateSubKey(@"Shell\Open\Command").SetValue("", ToShortPathName(applicationPath) + " \"%1\"");

                        if (!string.IsNullOrEmpty(iconPath))
                        {
                            string shortIconPath = ToShortPathName(iconPath);
                            if (iconIndex > 0)
                                shortIconPath += "," + iconIndex.ToString();
                            key.CreateSubKey("defaultIcon").SetValue("", shortIconPath);
                        }

                        //tell Windows Explorer about the new association so it will change the icons for your files without rebooting
                        SHChangeNotify(0x08000000, 0x0000, IntPtr.Zero, IntPtr.Zero);

                        retval = true;
                    }
                }
            }
            //else you aren't an admin, but maybe it is already registered?
            return retval;
        }

        // Return true if extension already associated in registry
        public static bool IsAssociated(string extension)
        {
            RegistryKey? userClassesRoot = Registry.CurrentUser.OpenSubKey(@"Software\Classes");
            if (userClassesRoot == null)
                return false;
            else
                return (userClassesRoot.OpenSubKey(extension, false) != null);
        }

        private static string ToShortPathName(string longName)
        {
            StringBuilder s = new StringBuilder(1000);
            uint iSize = (uint)s.Capacity;
            _ = GetShortPathName(longName, s, iSize);
            return s.ToString();
        }

        static public bool IsUserAdministrator()
        {
            bool retval;
            try
            {   //get the currently logged in user
                WindowsIdentity user = WindowsIdentity.GetCurrent();
                WindowsPrincipal principal = new WindowsPrincipal(user);
                retval = principal.IsInRole(WindowsBuiltInRole.Administrator);
            }
            catch (UnauthorizedAccessException)
            {
                retval = false;
            }
            catch (Exception)
            {
                retval = false;
            }
            return retval;
        }

        [DllImport("Kernel32.dll")]
        private static extern uint GetShortPathName(string lpszLongPath, [Out] StringBuilder lpszShortPath, uint cchBuffer);

        [DllImport("shell32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern void SHChangeNotify(uint wEventId, uint uFlags, IntPtr dwItem1, IntPtr dwItem2);

    }
}
