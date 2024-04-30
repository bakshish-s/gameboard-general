using Gameboard.EventArgs;
using System;
using System.Collections.Generic;
namespace Hashbyte.GameboardGeneral
{
public class EditorTest
{
    internal List<string> alignment = new List<string>() {
            "Lawful Good",
            "Lawful Neutral",
            "Lawful Evil",
            "Neutral Good",
            "True Neutral",
            "Neutral Evil",
            "Chaotic Good",
            "Chaotic Neutral",
            "Chaotic Evil",
        };

    internal List<string> creature = new List<string>() {
            "Dragon",
            "Owlbear",
            "Bulette",
            "Rust Monster",
            "Gelatinous Cube",
            "Hill Giant",
            "Stone Giant",
            "Frost Giant",
            "Fire Giant",
            "Cloud Giant",
            "Storm Giant",
            "Kobold",
            "Kuo-Toa",
            "Lich",
        };
    internal List<BoardUserPosition> mockBoardPositions = new List<BoardUserPosition>()
            {
                new BoardUserPosition { x = 0, y = 0.25F, coordinateSystem = "TOP_LEFT" },
                new BoardUserPosition { x = 0, y = 0.75F, coordinateSystem = "TOP_LEFT" },
                new BoardUserPosition { x = 0.25F, y = 1, coordinateSystem = "TOP_LEFT" },
                new BoardUserPosition { x = 0.75F, y = 1, coordinateSystem = "TOP_LEFT" },
                new BoardUserPosition { x = 1, y = 0.75F, coordinateSystem = "TOP_LEFT" },
                new BoardUserPosition { x = 1, y = 0.25F, coordinateSystem = "TOP_LEFT" },
                new BoardUserPosition { x = 0.75F, y = 0, coordinateSystem = "TOP_LEFT" },
                new BoardUserPosition { x = 0.25F, y = 0, coordinateSystem = "TOP_LEFT" },
            };

    private Stack<BoardUserPosition> mockBoardPositionsStack;
    public EditorTest()
    {
        mockBoardPositionsStack = new Stack<BoardUserPosition>(mockBoardPositions);
        mockBoardPositions.Reverse();
    }

    public ePlayerDirection GetUser(out PlayerPresence playerPresence)
    {
        var id = Guid.NewGuid().ToString();
        var alignmentName = alignment[UnityEngine.Random.Range(0, alignment.Count - 1)];
        var creatureName = creature[UnityEngine.Random.Range(0, creature.Count - 1)];

        playerPresence = new PlayerPresence()
        {
            id = $"{id}",
            userId = id,
            userName = $"{alignmentName} {creatureName}",                                    
            isLoggedIn = true,
        };        
        return mockBoardPositionsStack.Pop().GetDirection();
    }
}
}
