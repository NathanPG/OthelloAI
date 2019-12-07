using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// AI for player one (BLACK)
/// </summary>
public class P1AI : AIscript
{
    public int Maxdepth = 5;
    /// <summary>
    /// This shows how to override the abstract definition of makeMove. All this one
    /// does is stupidly a random, yet legal, move.
    /// </summary>
    /// <param name="availableMoves"></param>
    /// <param name="currentBoard"></param>
    /// <returns></returns>

    public override KeyValuePair<int, int> makeMove(List<KeyValuePair<int, int>> availableMoves, BoardSpace[][] currentBoard, uint turn_number)
    {
        BoardSpace enemyColor = turn_number % 2 == 0 ? BoardSpace.WHITE : BoardSpace.BLACK;
        BoardSpace ourColor = turn_number % 2 == 0 ? BoardSpace.BLACK : BoardSpace.WHITE;
        KeyValuePair<int, int> result;
        float score = float.NegativeInfinity;
        foreach (KeyValuePair<int, int> move in availableMoves) {
            BoardSpace[][] newer_board = new BoardSpace[8][];
            for (int i = 0; i < 8; ++i) {
                newer_board[i] = new BoardSpace[8];
                for (int j = 0; j < 8; ++j) {
                    newer_board[i][j] = currentBoard[i][j];
                }
            }
            newer_board[move.Key][move.Value] = ourColor;
            List<KeyValuePair<int, int>> changed = BoardScript.GetPointsChangedFromMove(currentBoard, turn_number, move.Key, move.Value);
            foreach (KeyValuePair<int, int> change in changed) {
                newer_board[change.Key][change.Value] = ourColor;
            }
            float candidate = negaMax(newer_board, 1, Maxdepth, turn_number);
            if(candidate > score) {
                result = new KeyValuePair<int, int>(move.Key, move.Value);
            }

        }
        return result;
    }

    private float negaMax(BoardSpace[][] currentBoard, int current_depth, int Max_depth, uint turn_number) {
        BoardSpace enemyColor = turn_number % 2 == 0 ? BoardSpace.WHITE : BoardSpace.BLACK;
        BoardSpace ourColor = turn_number % 2 == 0 ? BoardSpace.BLACK : BoardSpace.WHITE;
        if (current_depth == Max_depth) {
            return Evaluation(currentBoard);
        }
        uint current_player = turn_number % 2;
        List<KeyValuePair<int, int>> possible_moves = BoardScript.GetValidMoves(currentBoard, turn_number);
        List<float> score = new List<float>();
        foreach (KeyValuePair<int, int> move in possible_moves) {
            BoardSpace[][] newer_board = new BoardSpace[8][];
            for (int i = 0; i < 8; ++i) {
                newer_board[i] = new BoardSpace[8];
                for (int j = 0; j < 8; ++j) {
                    newer_board[i][j] = currentBoard[i][j];
                }
            }
            newer_board[move.Key][move.Value] = ourColor;
            List<KeyValuePair<int, int>> changed = BoardScript.GetPointsChangedFromMove(currentBoard, turn_number, move.Key, move.Value);
            foreach (KeyValuePair<int, int> change in changed) {
                newer_board[change.Key][change.Value] = ourColor;
            }
            score.Add(negaMax(newer_board, current_depth + 1, Max_depth, turn_number + 1));

        }
        if(current_depth % 2 == 1) {
            return score.Min();
        } else {
            return score.Max();
        }

        
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
        return blackCount - whiteCount;
    }
}
