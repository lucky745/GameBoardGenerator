using GameBoardGenerator.Enums;
using GameBoardGenerator.Interfaces;
using GameBoardGenerator.Util;

namespace GameBoardGenerator.Implementations
{
    internal class RoadGenerator : IPatternGenerator
    {
        public PatternType TargetType => PatternType.Road;

        public bool CanApply(PatternType type)
        {
            return type == TargetType;
        }

        public GameEntityType[][] Generate()
        {
            var nextPatternRand = Random.Next(0, 100);
            var nextPattern = _roadChances.Single(x => nextPatternRand >= x.Key.Start.Value && nextPatternRand <= x.Key.End.Value).Value;

            return ParseBoardFromString(nextPattern);
        }

        private readonly Dictionary<Range, string> _roadChances = new()
        {
            [0..14] = "---",
            [15..39] = "----",
            [40..54] = "-----",
            [55..59] = "------",
            [60..64] = "-------",
            [65..69] = "--------",
            [70..84] = "---------",
            [85..99] = "----------",
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
                GeneratedRoad[BoardConstants.MidLine][i] = GameEntityType.Road;
            }

            return GeneratedRoad;
        }

        private static Random Random => new((int)DateTime.Now.Ticks);
    }
}