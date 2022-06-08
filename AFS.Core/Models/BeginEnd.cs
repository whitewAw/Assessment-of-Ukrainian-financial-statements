using AFS.Core.Models;

namespace AFS.Core.Model
{
    public class BeginEnd: TrackedEntity
    {
        private double begin;
        private double end;
        public double Begin
        {
            get => begin;
            set
            {
                if (!begin.Equals(value))
                {
                    begin = AFSConstraints.RoundStat(value);
                    NotifyStateChanged();
                }
            }
        }
        public double End
        {
            get => end;
            set
            {
                if (!end.Equals(value))
                {
                    end = AFSConstraints.RoundStat(value);
                    NotifyStateChanged();
                }
            }
        }

        internal void Init(BeginEnd fild)
        {
            begin = fild.Begin;
            end = fild.End;
        }
    }
}