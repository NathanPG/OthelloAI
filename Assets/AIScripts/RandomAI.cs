﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomAI : AIscript
{

    /// <summary>
    /// This shows how to override the abstract definition of makeMove. All this one
    /// does is stupidly a random, yet legal, move.
    /// </summary>
    /// <param name="availableMoves"></param>
    /// <param name="currentBoard"></param>
    /// <returns></returns>

    public override KeyValuePair<int, int> makeMove(List<KeyValuePair<int, int>> availableMoves, BoardSpace[][] currentBoard, uint turn_number) {

        return availableMoves[Random.Range(0, availableMoves.Count)];
    }
    public override float Evaluation(BoardSpace[][] currentBoard, uint turn_number) { return 0; }
}
