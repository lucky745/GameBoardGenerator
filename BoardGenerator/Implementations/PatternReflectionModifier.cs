using GameBoardGenerator.Enums;
using GameBoardGenerator.Interfaces;
using GameBoardGenerator.Util;

namespace GameBoardGenerator.Implementations
{
    internal class PatternReflectionModifier : IPatternModifier
    {
        public PatternReflectionModifier() { }

        private static Random Random => new((int)DateTime.Now.Ticks);

        public ModifierFlag ModifyFlags(PatternType type)
        {
            if (type.Equals(PatternType.Spike))
            {
                var nextPieceRand = Random.Next(0, 10);
                if (nextPieceRand < 8)
                {
                    return ModifierFlag.Reflected;
                }
            }

            return ModifierFlag.None;
        }

        public GameEntityType[][] ModifyRoad(GameEntityType[][] road, PatternType ptype, ModifierFlag modifier)
        {
            if (!modifier.HasFlag(ModifierFlag.Reflected))
            {
                return road;
            }

            switch (ptype)
            {
                case PatternType.Spike:
                    {
                        var nextPieceRand = Random.Next(0, 10);
                        if (nextPieceRand < 5)
                        {
                            for (var i = 0; i < road[0].Length; i++)
                            {
                                if (road[BoardConstants.TopLine][i].Equals(GameEntityType.Spike))
                                {
                                    road[BoardConstants.BottomLine][i] = GameEntityType.Spike;
                                }
                            }

                            return road;
                        }
                        else
                        {
                            var modifiedRoad = new GameEntityType[BoardConstants.BoardWidth][];

                            for (var i = 0; i < BoardConstants.BoardWidth; i++)
                            {
                                modifiedRoad[i] = new GameEntityType[road[0].Length + 3];
                            }

                            modifiedRoad[BoardConstants.MidLine][0] = GameEntityType.Road;
                            modifiedRoad[BoardConstants.MidLine][1] = GameEntityType.Road;
                            modifiedRoad[BoardConstants.MidLine][2] = GameEntityType.Road;

                            for (var i = 0; i < road[0].Length; i++)
                            {
                                if (road[BoardConstants.TopLine][i].Equals(GameEntityType.Spike))
                                {
                                    modifiedRoad[BoardConstants.TopLine][i] = GameEntityType.Spike;
                                    modifiedRoad[BoardConstants.BottomLine][i + 3] = GameEntityType.Spike;
                                }
                                modifiedRoad[BoardConstants.MidLine][i + 3] = GameEntityType.Road;
                            }

                            return modifiedRoad;
                        }
                    }
                default:
                    throw new ArgumentException($"PatternType not a Pit and not a Spike. actual type:{ptype}");
            }
        }
    }
}