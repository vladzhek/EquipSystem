using System;
using Data.Progress;
using Infrastructure;

namespace Services
{
    public class ProgressService : IGameService
    {
        public PlayerProgress PlayerProgress { get; private set; }
        public bool IsLoaded { get; set; } = false;
        public event Action OnLoaded;

        public void InitializeProgress(PlayerProgress playerProgress)
        {
            PlayerProgress = playerProgress;

            IsLoaded = true;
            OnLoaded?.Invoke();
        }
        
        public void Initialize()
        {
            
        }
        
        public void Cleanup()
        {
           
        }
    }
}