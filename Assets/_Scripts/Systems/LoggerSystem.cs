using System.Collections.Generic;
using _Scripts.Ui;
using DI;
using UnityEngine;
using ILogger = _Scripts.Abstractions.Interfaces.ILogger;

namespace _Scripts.Systems
{
    public class LoggerSystem : ILogger
    {
        [Inject] private Notification _notification;
        private Dictionary<string, string> _localizationDictionary = new();

        public void LoadLocalization(Dictionary<string, string> localization)
        {
            _localizationDictionary = localization;
        }
        
        public void Log(string key)
        {
            if (!_localizationDictionary.ContainsKey(key))
            {
                Debug.LogError($"Localization key not found: {key}");
                return;
            }

            string localizedMessage = _localizationDictionary[key];

            if (_notification != null)
            {
                _notification.Show(localizedMessage);
            }
        }
    }
}
