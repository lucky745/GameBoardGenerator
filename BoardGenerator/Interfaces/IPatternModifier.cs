using GameBoardGenerator.Enums;

namespace GameBoardGenerator.Interfaces
{
    internal interface IPatternModifier
    {
        ModifierFlag ModifyFlags(PatternType type);

        GameEntityType[][] ModifyRoad(GameEntityType[][] road, PatternType ptype, ModifierFlag modifier);
    }
}