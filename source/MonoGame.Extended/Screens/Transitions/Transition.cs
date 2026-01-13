using System;
using Microsoft.Xna.Framework;

namespace MonoGame.Extended.Screens.Transitions
{
    public enum TransitionState { In, Out }

    public abstract class Transition : IDisposable
    {
        private readonly float _halfDuration;
        public float CurrentSeconds;

        protected Transition(float duration)
        {
            Duration = duration;
            _halfDuration = Duration / 2f;
        }

        public abstract void Dispose();

        public TransitionState State { get; set; } = TransitionState.Out;
        public float Duration { get; }
        public virtual float Value => MathHelper.Clamp(CurrentSeconds / _halfDuration, 0f, 1f);

        public virtual event EventHandler StateChanged;
        public virtual event EventHandler Completed;

        public virtual void Update(GameTime gameTime)
        {
            var elapsedSeconds = gameTime.GetElapsedSeconds();

            switch (State)
            {
                case TransitionState.Out:
                    CurrentSeconds += elapsedSeconds;

                    if (CurrentSeconds >= _halfDuration)
                    {
                        State = TransitionState.In;
                        StateChanged?.Invoke(this, EventArgs.Empty);
                    }
                    break;
                case TransitionState.In:
                    CurrentSeconds -= elapsedSeconds;

                    if (CurrentSeconds <= 0.0f)
                    {
                        Completed?.Invoke(this, EventArgs.Empty);
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public abstract void Draw(GameTime gameTime);
    }
}
