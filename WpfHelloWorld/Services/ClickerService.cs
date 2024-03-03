using System.Threading.Tasks;

namespace WpfHelloWorld.Services
{
    public interface IClickerService
    {
        public int ClickCounter { get; }

        void SetClicks(int clickCount);
        public Task ClickAsync();
    }

    public class ClickerService : IClickerService
    {
        public int ClickCounter { get; private set; }

        public void SetClicks(int clickCount)
        {
            ClickCounter = clickCount;
        }

        public async Task ClickAsync()
        {
            ClickCounter++;
            await Task.Delay(1000);
        }
    }
}
