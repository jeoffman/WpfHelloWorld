using System.Diagnostics;
using System;
using System.IO;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;
using WpfHelloWorld.WinUtils;

namespace WpfHelloWorld.Models
{
    public class HelloDocument
    {
        static bool _ExtensionAlreadyRegistered = false;

        public const string FileExtension = "HelloWorld";

        public static void RegisterFileExtensionForCurrentUser()
        {
            string? exeThatLaunchedUs = Assembly.GetExecutingAssembly().Location;
            if (exeThatLaunchedUs.EndsWith(".dll", StringComparison.OrdinalIgnoreCase))
                exeThatLaunchedUs = Environment.ProcessPath;
            if (exeThatLaunchedUs == null)
            {
                Trace.TraceError("Couldn't associate FileExtension to this EXE because ProcessPath is NULL, run as administrator?");
                return; //you are a null process? huh.
            }

            JkhFileAssociation.RegisterFileExtensionForCurrentUser(FileExtension, exeThatLaunchedUs);
            if (!JkhFileAssociation.AdminAssociateAllUsers(FileExtension, FileExtension, exeThatLaunchedUs, "Hello World Document", exeThatLaunchedUs, 0))
                Trace.TraceError("Couldn't associate FileExtension to this EXE you aren't admin, run as administrator?");
        }


        public int MainWindowTop { get; set; }
        public int MainWindowLeft { get; set; }
        public int MainWindowWidth { get; set; } = 800;
        public int MainWindowHeight { get; set; } = 450;

        public string SomeHello { get; set; } = "Hello";
        public int ClickCounts { get; set; } = 0;

        public HelloDocument()
        {
            if (!_ExtensionAlreadyRegistered)
            {
                _ExtensionAlreadyRegistered = true;
                RegisterFileExtensionForCurrentUser();
            }
        }

        public async Task LoadAsync(string fileName)
        {
            HelloDocument? incoming;
            try
            {
                string jsonString = await File.ReadAllTextAsync(fileName);
                incoming = JsonSerializer.Deserialize<HelloDocument>(jsonString);

                if (incoming == null)
                    incoming = new HelloDocument();
            }
            catch (FileNotFoundException)
            {
                incoming = new HelloDocument();
            }

            MainWindowHeight = incoming.MainWindowHeight;
            MainWindowWidth = incoming.MainWindowWidth;
            MainWindowTop = incoming.MainWindowTop;
            MainWindowLeft = incoming.MainWindowLeft;

            SomeHello = incoming.SomeHello;
            ClickCounts = incoming.ClickCounts;
        }

        public async Task SaveAsync(string fileName)
        {
            string jsonString = JsonSerializer.Serialize(this);
            await File.WriteAllTextAsync(fileName, jsonString);
        }
    }
}
