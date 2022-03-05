using GameBoardGenerator.Enums;

namespace GameBoardGenerator.Interfaces
{
    internal interface IPatternGenerator
    {
        PatternType TargetType { get; }

        bool CanApply(PatternType type);

        GameEntityType[][] Generate();
    }
}