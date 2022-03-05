using GameBoardGenerator.Enums;
using GameBoardGenerator.Interfaces;
using GameBoardGenerator.Util;

namespace GameBoardGenerator.Implementations
{
    internal class PatternsGemsModifier : IPatternModifier
    {
        private static Random Random => new((int)DateTime.Now.Ticks);

        public ModifierFlag ModifyFlags(PatternType type)
        {
            if (type.Equals(PatternType.Pit) || type.Equals(PatternType.Spike))
            {
                var nextPieceRand = Random.Next(0, 10);
                if (nextPieceRand < 3)
                {
                    return ModifierFlag.Gem;
                }
            }

            return ModifierFlag.None;
        }

        public GameEntityType[][] ModifyRoad(GameEntityType[][] road, PatternType ptype, ModifierFlag modifier)
        {
            if (!modifier.HasFlag(ModifierFlag.Gem))
            {
                return road;
            }

            switch (ptype)
            {
                case PatternType.Spike:
                    {
                        var nextPieceRand = Random.Next(0, 10);
                        if (nextPieceRand < 3)
                        {
                            for (var i = 0; i < road[0].Length; i++)
                            {
                                if (!road[BoardConstants.TopLine][i].Equals(GameEntityType.Spike))
                                {
                                    road[BoardConstants.TopLine][i] = GameEntityType.Gem;
                                }
                            }

                            return road;
                        }
                        else
                        {
                            for (var i = 0; i < road[0].Length; i++)
                            {
                                if (modifier.HasFlag(ModifierFlag.Reflected) && !road[BoardConstants.BottomLine][i].Equals(GameEntityType.Spike))
                                {
                                    road[BoardConstants.BottomLine][i] = GameEntityType.Gem;
                                }
                                else if (!modifier.HasFlag(ModifierFlag.Reflected) && !road[BoardConstants.TopLine][i].Equals(GameEntityType.Spike))
                                {
                                    road[BoardConstants.TopLine][i] = GameEntityType.Gem;
                                }
                            }

                            return road;
                        }
                    }
                case PatternType.Pit:
                    {
                        var nextPieceRand = Random.Next(0, 10);
                        if (nextPieceRand < 8)
                        {
                            for (var i = 0; i < road[0].Length; i++)
                            {
                                if (road[BoardConstants.MidLine][i].Equals(GameEntityType.None))
                                {
                                    road[BoardConstants.MidLine][i] = GameEntityType.Gem;
                                }
                            }

                            return road;
                        }
                        else
                        {
                            for (var i = 0; i < road[0].Length; i++)
                            {
                                if (road[BoardConstants.MidLine][i].Equals(GameEntityType.None))
                                {
                                    road[BoardConstants.MidLine][i] = GameEntityType.Gem;
                                }
                            }

                            return road;
                        }
                    }
                default:
                    throw new ArgumentException($"PatternType not a Pit and not a Spike. actual type:{ptype}");
            }
        }
    }
}