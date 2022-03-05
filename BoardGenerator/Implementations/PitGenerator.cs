using GameBoardGenerator.Enums;
using GameBoardGenerator.Interfaces;
using GameBoardGenerator.Util;

namespace GameBoardGenerator.Implementations
{
    internal class PitGenerator : IPatternGenerator
    {
        public PatternType TargetType => PatternType.Pit;

        public bool CanApply(PatternType type)
        {
            return type == TargetType;
        }

        public GameEntityType[][] Generate()
        {
            var nextPatternRand = Random.Next(0, 100);
            var nextPattern = _pitChances.Single(x => nextPatternRand >= x.Key.Start.Value && nextPatternRand <= x.Key.End.Value).Value;

            return ParseBoardFromString(nextPattern);
        }

        private readonly Dictionary<Range, string> _pitChances = new()
        {
            [0..19] = "^^^",
            [20..39] = "^^^^",
            [40..53] = "^^^---^^^",
            [54..66] = "^^^^---^^^^",
            [67..79] = "^^^^^----^^^^^",
            [80..87] = "^^^---^^^---^^^",
            [88..94] = "^^^^---^^^^---^^^^",
            [95..99] = "^^^---^^^---^^^---^^^",
        };

        private static GameEntityType[][] ParseBoardFromString(string pattern)
        {
            var GeneratedRoad = new GameEntityType[BoardConstants.BoardWidth][];

            for (var j = 0; j < BoardConstants.BoardWidth; j++)
            {
                GeneratedRoad[j] = new GameEntityType[pattern.Length];
            }

            for (var i = 0; i < pattern.Length; i++)
            {
                GeneratedRoad[BoardConstants.MidLine][i] =
                    pattern[i].Equals('-')
                    ? GameEntityType.Road
                    : GameEntityType.None;
            }

            return GeneratedRoad;
        }

        private static Random Random => new((int)DateTime.Now.Ticks);
    }
}