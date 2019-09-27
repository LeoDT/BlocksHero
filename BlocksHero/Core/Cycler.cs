using System;

using Microsoft.Xna.Framework;

namespace BlocksHero.Core
{
    public enum CyclerState
    {
        Idle,
        Busy,
        Ready
    }

    public class Cycler
    {
        public int Cycle { get; private set; }
        public CyclerState State { get; private set; }
        public double Progress
        {
            get => (double)_cycle / (double)Cycle;
        }

        private double _cycle;
        private bool _started;

        public Cycler(int cycle)
        {
            Cycle = cycle;
            State = CyclerState.Idle;
        }

        public void Start()
        {
            this._started = true;
        }

        public void Stop()
        {
            this._started = false;
            this._cycle = 0;
        }

        public void Reset()
        {
            this.Stop();

            this.State = CyclerState.Idle;
        }

        public void Update(GameTime gameTime)
        {
            if (this.State == CyclerState.Idle && this._started)
            {
                this.State = CyclerState.Busy;
            }
            else if (this.State == CyclerState.Busy)
            {
                this._cycle += gameTime.ElapsedGameTime.TotalMilliseconds;

                if (this._cycle >= this.Cycle)
                {
                    this.State = CyclerState.Ready;
                }
            }
        }
    }
}
