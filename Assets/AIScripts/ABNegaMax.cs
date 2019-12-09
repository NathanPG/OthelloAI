using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// AI for player two (White)
/// </summary>
public class ABNegaMax : AIscript
{
    public int Maxdepth = 5;
    /// <summary>
    /// This shows how to override the abstract definition of makeMove. All this one
    /// does is stupidly a random, yet legal, move.
    /// </summary>
    /// <param name="availableMoves"></param>
    /// <param name="currentBoard"></param>
    /// <returns></returns>

    public override KeyValuePair<int, int> makeMove(List<KeyValuePair<int, int>> availableMoves, BoardSpace[][] currentBoard, uint turn_number) {
        //Debug.Log(string.Join(",", availableMoves));
        BoardSpace enemyColor = turn_number % 2 == 0 ? BoardSpace.WHITE : BoardSpace.BLACK;
        BoardSpace ourColor = turn_number % 2 == 0 ? BoardSpace.BLACK : BoardSpace.WHITE;
        KeyValuePair<int, int> result;
        List<KeyValuePair<int, int>> result_candidates = new List<KeyValuePair<int, int>>();
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
            float candidate = ABnegaMax(newer_board, 1, Maxdepth, turn_number + 1, float.NegativeInfinity, float.PositiveInfinity);
            //Debug.Log("candidate: " + candidate);
            if (candidate >= score) {
                if (candidate > score)
                {
                    result_candidates.Clear();
                    result = new KeyValuePair<int, int>(move.Key, move.Value);
                    score = candidate;
                    result_candidates.Add(result);
                }
                else
                {
                    result_candidates.Add(result);
                }
            }
        }
        //Debug.Log("turn number: " + turn_number);
        //Debug.Log("final score: " + score);
        return result_candidates[Random.Range(0, result_candidates.Count)];
    }

    private float ABnegaMax(BoardSpace[][] currentBoard, int current_depth, int Max_depth, uint turn_number, float alpha, float beta) {
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
            //add a variable that stores alpha
            //instead of compiling scores, compare with beta. if >=, just return
            //negaMax w/ alpha and beta being -beta, -<variable>
            float value = (-ABnegaMax(newer_board, current_depth + 1, Max_depth, turn_number + 1, -beta, -alpha));
            score.Add(value);
            if (value >= beta) break;
            if (value >= alpha) alpha = value;
        }
        //Debug.Log("score: " + string.Join(",", score));
        return score.Max();
    }

    public override float Evaluation(BoardSpace[][] currentBoard, uint turn_number) {
        float score = 0f;
        int swap = 44;
        BoardSpace enemy_color = BoardSpace.BLACK;
        if (color == BoardSpace.BLACK)
        {
            enemy_color = BoardSpace.WHITE;
        }
        //gets the number of tiles of the AI's color minus the number of the opposing color.
        for ( int i = 0; i < currentBoard.Length; i++ ) {
            for (int j = 0; j < currentBoard[i].Length; j++) {
                if (currentBoard[i][j] == color)
                    score += 1f;
                else if (currentBoard[i][j] == enemy_color)
                    score -= 1f;
            }
        }
        //if we are not super late in the game and this choice leads
        //to an increase in our own tiles, we prioritize lower increases.
        //Decreases are still bad, so don't invert those.
        if (turn_number < swap && score > 0f)
            score = 1f / score;
        //counts out colored corners/adjacents for both colors.
        int our_corners = 0;
        int our_adjacents = 0;
        int their_corners = 0;
        int their_adjacents = 0;
        //prioritize moves that end up earning a corner, and
        //lower priority to moves that end up giving up a corner.
        //calculate corner priorities. If a space is empty, then check
        //the spaces adjacent to that corner and update accordingly.
        //Adjacents are not very relevant when a corner is taken.
        for (int i = 0; i < currentBoard.Length; i += currentBoard.Length-1)
        {
            for(int j = 0; j < currentBoard[i].Length; j += currentBoard[i].Length-1)
            {
                if (currentBoard[i][j] == color)
                    our_corners++;
                else if (currentBoard[i][j] == enemy_color)
                    their_corners++;
                //check adjacents (i-1 to i+1, j-1 to j+1) - should only actually run 4 times
                else {
                    for(int i2 = Mathf.Max(i - 1, 0); i2 < Mathf.Min(i + 1, currentBoard.Length); i2++)
                    {
                        for(int j2 = Mathf.Max(j - 1, 0); j2 < Mathf.Min(j + 1, currentBoard[i2].Length); j2++)
                        {
                            if (i2 == i && j2 == j)
                                continue;
                            if (currentBoard[i2][j2] == color)
                                our_adjacents++;
                            else if (currentBoard[i2][j2] == enemy_color)
                                their_adjacents++;
                        }
                    }
                }
            }
        }
        score += (2f * our_corners - 1.3f*our_adjacents - 3f * their_corners + 1f * their_adjacents);
        return score;
    }
}