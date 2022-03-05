using GameBoardGenerator.Enums;
using GameBoardGenerator.Implementations;
using GameBoardGenerator.Interfaces;
using GameBoardGenerator.Util;
using System.Numerics;

namespace GameBoardGenerator
{
    public class BoardGenerator
    {
        private const int StartingHorizontalIndex = 8;
        private const int StartingVerticalIndex = 5;

        private readonly int _tileHeight;
        private readonly int _tileWidth;

        private readonly PatternChooser patternChooser = new PatternChooser();

        private readonly List<IPatternModifier> modifiers;
        private readonly List<IPatternGenerator> generators;

        public BoardGenerator(int tileWidth, int tileHeight)
        {
            modifiers = new List<IPatternModifier>
            {
                new PatternReflectionModifier(),
                new PatternsGemsModifier()
            };

            generators = new List<IPatternGenerator>
            {
                new PitGenerator(),
                new RoadGenerator(),
                new SpikeGenerator()
            };

            _tileWidth = tileWidth;
            _tileHeight = tileHeight;
        }

        public GameBoard GenerateBoard(int count)
        {
            var board = new GameBoard(BoardConstants.BoardWidth, count);

            int j;

            for (j = 0; j < BoardConstants.BoardLength; j++) board.AddElement(BoardConstants.MidLine, j, CreateRoadModel(BoardConstants.MidLine, j));

            GameEntityType[][] generatedRoad;

            while (j < count - 1)
            {
                var modifierFlag = ModifierFlag.None;
                var pattern = patternChooser.Next();
                generatedRoad = GeneratePattern(pattern);

                foreach (var modifier in modifiers)
                {
                    modifierFlag |= modifier.ModifyFlags(pattern);

                    if (modifierFlag.HasFlag(ModifierFlag.Reflected) || modifierFlag.HasFlag(ModifierFlag.Gem))
                    {
                        generatedRoad = modifier.ModifyRoad(generatedRoad, pattern, modifierFlag);
                    }
                }

                board = ModifyBoard(generatedRoad, board, j);
                j += generatedRoad[0].Length;
            }

            board.AddElement(StartingVerticalIndex, StartingHorizontalIndex, new PlayerModel(new Vector2(_tileHeight * StartingHorizontalIndex, StartingVerticalIndex * _tileHeight), _tileWidth, _tileHeight));

            return board;
        }

        private GameEntityType[][] GeneratePattern(PatternType pattern)
        {
            foreach (var generator in generators)
            {
                if (generator.CanApply(pattern))
                {
                    return generator.Generate();
                }
            }

            throw new ArgumentException($"No generator was applied. Pattern:{pattern}");
        }

        private GameBoard ModifyBoard(GameEntityType[][] road, GameBoard board, int j)
        {
            for (var l = 0; l < road.Length; l++)
            {
                for (var k = 0; k < road[0].Length; k++)
                {
                    switch (road[l][k])
                    {
                        case GameEntityType.Spike:
                            board.AddElement(l, k + j, CreateSpikeModel(l, k + j));
                            break;
                        case GameEntityType.Road:
                            board.AddElement(l, k + j, CreateRoadModel(l, k + j));
                            break;
                        case GameEntityType.Gem:
                            board.AddElement(l, k + j, CreateGem(l, k + j));
                            break;
                        default:
                            break;
                    }
                }
            }

            return board;
        }

        private GemModel CreateGem(int row, int column)
        {
            return new GemModel(new Vector2(column * _tileWidth, row * _tileHeight), _tileHeight);
        }

        private SpikeModel CreateSpikeModel(int row, int column)
        {
            return new SpikeModel(new Vector2(column * _tileWidth, row * _tileHeight), _tileWidth, _tileHeight);
        }

        private RoadModel CreateRoadModel(int row, int column)
        {
            return new RoadModel(new Vector2(column * _tileWidth, row * _tileHeight), _tileWidth, _tileHeight);
        }
    }
}