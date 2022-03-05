using GameBoardGenerator.Enums;
using GameBoardGenerator.Interfaces;
using GameBoardGenerator.Util;

namespace GameBoardGenerator.Implementations
{
    internal class SpikeGenerator : IPatternGenerator
    {
        public PatternType TargetType => PatternType.Spike;

        public bool CanApply(PatternType type)
        {
            return type == TargetType;
        }

        public GameEntityType[][] Generate()
        {
            var nextPatternRand = Random.Next(0, 100);
            var nextPattern = _spikeChances.Single(x => nextPatternRand >= x.Key.Start.Value && nextPatternRand <= x.Key.End.Value).Value;

            return ParseBoardFromString(nextPattern);
        }

        private readonly Dictionary<Range, string> _spikeChances = new()
        {
            [0..14] = "*-----*-----*-----*-----*-----*",
            [15..29] = "**-----**-----**-----**-----**",
            [30..44] = "***------***------***------***",
            [45..49] = "****-------***------*------***",
            [50..54] = "*------***-------****",
            [55..59] = "****-------*-------****",
            [60..74] = "***------**-----*-----**",
            [75..89] = "**-----***------****",
            [90..99] = "*--",
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
                GeneratedRoad[BoardConstants.TopLine][i] =
                    pattern[i].Equals('*')
                    ? GameEntityType.Spike
                    : GameEntityType.None;
                GeneratedRoad[BoardConstants.MidLine][i] = GameEntityType.Road;
            }

            return GeneratedRoad;
        }

        private static Random Random => new((int)DateTime.Now.Ticks);
    }
}