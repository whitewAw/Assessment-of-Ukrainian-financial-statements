namespace AFS.Core.Models
{
    public class CurrentPrevious : TrackedEntity
    {
        private double current;
        private double previous;
        public double Current
        {
            get => current;
            set => SetProperty(ref current, AfsConstraints.RoundStat(value));
        }
        public double Previous
        {
            get => previous;
            set => SetProperty(ref previous, AfsConstraints.RoundStat(value));
        }

        internal void Init(CurrentPrevious fild)
        {
            current = fild.Current;
            previous = fild.Previous;
        }
    }
}