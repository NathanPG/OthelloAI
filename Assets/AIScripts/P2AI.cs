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
    public override KeyValuePair<int, int> makeMove(List<KeyValuePair<int, int>> availableMoves, BoardSpace[][] currentBoard, uint turn_number)
    {
        
        return availableMoves[Random.Range(0, availableMoves.Count)];
    }



    public override float Evaluation(BoardSpace[][] currentBoard, uint turn_number) {
        BoardSpace enemyColor = turn_number % 2 == 0 ? BoardSpace.WHITE : BoardSpace.BLACK;
        BoardSpace ourColor = turn_number % 2 == 0 ? BoardSpace.BLACK : BoardSpace.WHITE;
        int ourCount = 0;
        int enemyCount = 0;
        foreach (BoardSpace[] row in currentBoard) {
            foreach (BoardSpace space in row) {
                if (space == enemyColor) {
                    enemyCount++;
                }
                if (space == ourColor) {
                    ourCount++;
                }

            }
        }
        return ourCount - enemyCount;
    }

}
