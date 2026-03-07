namespace AFS.Core.Models
{
    public class BeginEnd : TrackedEntity
    {
        private double begin;
        private double end;
        public double Begin
        {
            get => begin;
            set => SetProperty(ref begin, AfsConstraints.RoundStat(value));
        }
        public double End
        {
            get => end;
            set => SetProperty(ref end, AfsConstraints.RoundStat(value));
        }

        internal void Init(BeginEnd fild)
        {
            begin = fild.Begin;
            end = fild.End;
        }
    }
}