using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// AI for player two (White)
/// </summary>
public class P2AI : AIscript
{
    public int Maxdepth = 1;
    /// <summary>
    /// This shows how to override the abstract definition of makeMove. All this one
    /// does is stupidly a random, yet legal, move.
    /// </summary>
    /// <param name="availableMoves"></param>
    /// <param name="currentBoard"></param>
    /// <returns></returns>

    public override KeyValuePair<int, int> makeMove(List<KeyValuePair<int, int>> availableMoves, BoardSpace[][] currentBoard, int depth)
    {

        return availableMoves[Random.Range(0, availableMoves.Count)];
    }

    public override float Evaluation(BoardSpace[][] currentBoard)
    {
        int blackCount = 0;
        int whiteCount = 0;
        foreach (BoardSpace[] row in currentBoard)
        {
            foreach (BoardSpace space in row)
            {
                switch (space)
                {
                    case (BoardSpace.BLACK):
                        blackCount++;
                        break;
                    case (BoardSpace.WHITE):
                        whiteCount++;
                        break;
                }
            }
        }
        return whiteCount - blackCount;
    }
}
