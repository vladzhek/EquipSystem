using System;
using Data.Progress;
using Infrastructure;
using UnityEngine;

namespace Services
{
    public class SaveLoadService : IGameService
    {
        private const string SavesKey = "Saves";
        private ProgressService _progressService;

        public void Initialize()
        {
            LoadProgress();
        }

        public void SaveProgress()
        {
            _progressService = GameController.Services.Get<ProgressService>();
            PlayerPrefs.SetString(SavesKey, JsonUtility.ToJson(_progressService.PlayerProgress));
            Debug.Log("[Save] \n" + JsonUtility.ToJson(_progressService.PlayerProgress));
        }
        
        public void LoadProgress()
        {
            _progressService = GameController.Services.Get<ProgressService>();
            Debug.Log("[Load2] \n" + JsonUtility.ToJson(_progressService.PlayerProgress));
            _progressService.InitializeProgress(GetOrCreate());
        }
        
        private PlayerProgress GetOrCreate()
        {
            if(PlayerPrefs.HasKey(SavesKey))
            {
                var saves = PlayerPrefs.GetString(SavesKey);
                return JsonUtility.FromJson<PlayerProgress>(saves);
            }
            
            return new PlayerProgress();
        }

        public void Cleanup()
        {
            
        }
    }
}