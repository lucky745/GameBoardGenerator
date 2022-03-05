using GameBoardGenerator.Enums;
using GameBoardGenerator.Interfaces;

namespace GameBoardGenerator.Implementations
{
    internal class PatternChooser : IPatternChooser
    {
        private readonly LinkedList<PatternType> BlockedPatterns;

        public PatternChooser()
        {
            BlockedPatterns = new LinkedList<PatternType>();
            BlockedPatterns.AddLast(PatternType.Road);
            BlockedPatterns.AddLast(PatternType.Pit);
        }

        public PatternType Next()
        {
            BlockedPatterns.RemoveFirst();

            if (BlockedPatterns.Last.Value.Equals(PatternType.Road))
            {
                var nextPatternRand = Random.Next(0, 100);
                var patternType = _chances.Single(x => nextPatternRand >= x.Key.Start.Value && nextPatternRand <= x.Key.End.Value).Value;

                BlockedPatterns.AddLast(patternType);
                return patternType;
            }
            else
            {
                BlockedPatterns.AddLast(PatternType.Road);
                return PatternType.Road;
            }
        }

        private readonly Dictionary<Range, PatternType> _chances = new()
        {
            [0..14] = PatternType.Pit,
            [15..29] = PatternType.Spike,
            [30..99] = PatternType.Road,
        };

        private static Random Random => new((int)DateTime.Now.Ticks);
    }
}