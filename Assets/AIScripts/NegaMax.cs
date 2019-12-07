using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// AI for player one (BLACK)
/// </summary>
public class NegaMAX : AIscript
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
        //Debug.Log(string.Join(",", availableMoves));
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
            //Debug.Log(move.Key + "," + move.Value);
            newer_board[move.Key][move.Value] = ourColor;
            List<KeyValuePair<int, int>> changed = BoardScript.GetPointsChangedFromMove(newer_board, turn_number, move.Value, move.Key);
            //Debug.Log(string.Join(",", changed));
            foreach (KeyValuePair<int, int> change in changed) {
                newer_board[change.Key][change.Value] = ourColor;
            }
            float candidate = negaMax(newer_board, 1, Maxdepth, turn_number+1);
            if(candidate > score) {
                result = new KeyValuePair<int, int>(move.Key, move.Value);
            }

        }
        return result;
    }

    private float negaMax(BoardSpace[][] currentBoard, int current_depth, int Max_depth, uint turn_number) {
        BoardSpace enemyColor = turn_number % 2 == 0 ? BoardSpace.WHITE : BoardSpace.BLACK;
        BoardSpace ourColor = turn_number % 2 == 0 ? BoardSpace.BLACK : BoardSpace.WHITE;
        uint current_player = turn_number % 2;
        List<KeyValuePair<int, int>> possible_moves = BoardScript.GetValidMoves(currentBoard, turn_number);
        if (current_depth >= Max_depth || possible_moves.Count == 0) {
            return Evaluation(currentBoard, turn_number);
        }
        List<float> score = new List<float>();
        //Debug.Log("current depth: " + current_depth + " move size: " + possible_moves.Count);
        foreach (KeyValuePair<int, int> move in possible_moves) {
            BoardSpace[][] newer_board = new BoardSpace[8][];
            for (int i = 0; i < 8; ++i) {
                newer_board[i] = new BoardSpace[8];
                for (int j = 0; j < 8; ++j) {
                    newer_board[i][j] = currentBoard[i][j];
                }
            }
            newer_board[move.Key][move.Value] = ourColor;
            List<KeyValuePair<int, int>> changed = BoardScript.GetPointsChangedFromMove(newer_board, turn_number, move.Value, move.Key);
            //Debug.Log("changed: " + string.Join(",", changed));
            foreach (KeyValuePair<int, int> change in changed) {
                newer_board[change.Key][change.Value] = ourColor;
            }
            score.Add(-negaMax(newer_board, current_depth + 1, Max_depth, turn_number + 1));
        }
        //Debug.Log("score: " + string.Join(",", score));
        return score.Max();
    }

    public override float Evaluation(BoardSpace[][] currentBoard, uint turn_number)
    {
        int blackCount = 0;
        int whiteCount = 0;
        foreach (BoardSpace[] row in currentBoard) {
            foreach (BoardSpace space in row) {
                switch (space) {
                    case (BoardSpace.BLACK):
                        blackCount++;
                        break;
                    case (BoardSpace.WHITE):
                        whiteCount++;
                        break;
                }
            }
        }
        if(color == BoardSpace.BLACK) {
            if(turn_number % 2 == 0) {
                return (blackCount - whiteCount) / (blackCount + whiteCount);
            } else {
                return (whiteCount - blackCount) / (blackCount + whiteCount);
            }
        } else {
            if (turn_number % 2 == 1) {
                return (blackCount - whiteCount) / (blackCount + whiteCount);
            } else {
                return (whiteCount - blackCount) / (blackCount + whiteCount);
            }
        }
    }
}
