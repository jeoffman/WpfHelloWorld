using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace WpfHelloWorld.Models
{
    public class HelloDocument
    {
        public int MainWindowTop { get; set; }
        public int MainWindowLeft { get; set; }
        public int MainWindowWidth { get; set; } = 800;
        public int MainWindowHeight { get; set; } = 450;

        public string SomeHello { get; set; } = "Hello";
        public int ClickCounts { get; set; } = 0;

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
