using System.Collections.Generic;

namespace _Scripts.Abstractions.Interfaces
{
    public interface ILogger
    {
        public void Log(string log);

        public void LoadLocalization(Dictionary<string, string> localization);
    }
}