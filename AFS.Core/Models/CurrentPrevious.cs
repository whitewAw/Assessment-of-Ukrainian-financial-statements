using AFS.Core.Models;

namespace AFS.Core.Model
{
    public class CurrentPrevious : TrackedEntity
    {
        private double current;
        private double previous;
        public double Current
        {
            get => current;
            set
            {
                if (!current.Equals(value))
                {
                    current = AFSConstraints.RoundStat(value);
                    NotifyStateChanged();
                }
            }
        }
        public double Previous
        {
            get => previous;
            set
            {
                if (!previous.Equals(value))
                {
                    previous = AFSConstraints.RoundStat(value);
                    NotifyStateChanged();
                }
            }
        }

        internal void Init(CurrentPrevious fild)
        {
            current = fild.Current;
            previous = fild.Previous;
        }
    }
}