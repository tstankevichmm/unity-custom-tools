using System;

namespace CustomTools.Timers
{
    public abstract class Timer
    {
        protected float initialTime;
        
        public float Time { get; protected set; }
        public bool IsRunning { get; protected set; }

        public float Progress => Time / initialTime;

        public Action onTimerStart = delegate { };
        public Action onTimerStop = delegate { };

        protected Timer(float value)
        {
            initialTime = value;
            IsRunning = false;
        }

        public void Start()
        {
            Time = initialTime;

            if (IsRunning)
                return;
            
            IsRunning = true;
            onTimerStart.Invoke();
        }

        public void Stop()
        {
            if (!IsRunning)
                return;
            
            IsRunning = false;
            onTimerStop.Invoke();
        }

        public void Pause() => IsRunning = false;
        public void Resume() => IsRunning = true;

        public abstract void Tick(float deltaTime);
    }
}