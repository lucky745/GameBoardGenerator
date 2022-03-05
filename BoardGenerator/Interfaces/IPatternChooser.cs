using GameBoardGenerator.Enums;

namespace GameBoardGenerator.Interfaces
{
    internal interface IPatternChooser
    {
        PatternType Next();
    }
}