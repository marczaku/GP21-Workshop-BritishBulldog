using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class Game : MonoBehaviour
{
    public GameObject[] skyboxPrefabs;
    public State currentState;
    public GameView view;

    void OnEnable()
    {
        view.UpdateView(currentState);
        Debug.
    }

    State CurrentState
    {
        set
        {
            currentState = value;
            view.UpdateView(currentState);
        }
    }

    void Update()
    {
        if (currentState.IsEnemyWinState() || currentState.IsPlayerWinState())
            return;
        
        if (currentState.playerTurn)
        {
            if (Input.GetKeyDown(KeyCode.A) && currentState.player.x > 0)
            {
                CurrentState = new State
                {
                    grid = currentState.grid,
                    gridWidth = currentState.gridWidth,
                    player = currentState.player + Vector2Int.left,
                    enemy = currentState.enemy,
                    goal = currentState.goal,
                    playerTurn = !currentState.playerTurn
                };
            }
            if (Input.GetKeyDown(KeyCode.S) && currentState.player.y > 0)
            {
                CurrentState = new State
                {
                    grid = currentState.grid,
                    gridWidth = currentState.gridWidth,
                    player = currentState.player + Vector2Int.down,
                    enemy = currentState.enemy,
                    goal = currentState.goal,
                    playerTurn = !currentState.playerTurn
                };
            }
            if (Input.GetKeyDown(KeyCode.D) && currentState.player.x < currentState.gridWidth -1)
            {
                CurrentState = new State
                {
                    grid = currentState.grid,
                    gridWidth = currentState.gridWidth,
                    player = currentState.player + Vector2Int.right,
                    enemy = currentState.enemy,
                    goal = currentState.goal,
                    playerTurn = !currentState.playerTurn
                };
            }
            if (Input.GetKeyDown(KeyCode.W) && currentState.player.y < currentState.GridHeight - 1)
            {
                CurrentState = new State
                {
                    grid = currentState.grid,
                    gridWidth = currentState.gridWidth,
                    player = currentState.player + Vector2Int.up,
                    enemy = currentState.enemy,
                    goal = currentState.goal,
                    playerTurn = !currentState.playerTurn
                };
            }
        }
        else
        {
            CurrentState = FindNextEnemyOption(currentState);
        }
    }

    (int, State) Minimax(State start, int depth)
    {
        if (start.IsEnemyWinState())
            return (depth, start);
        if (start.IsPlayerWinState())
            return (-depth, start);
        if (depth == 0)
            return (0, start);
        if (!start.playerTurn)
        {
            int value = int.MinValue;
            State best = default;
            foreach (var child in start.GetAdjacent())
            {
                var (childValue, _) = Minimax(child, depth - 1);
                if (childValue > value)
                {
                    value = childValue;
                    best = child;
                }
            }

            return (value, best);
        }
        else
        {
            int value = int.MaxValue;
            State best = default;
            foreach (var child in start.GetAdjacent())
            {
                var (childValue, _) = Minimax(child, depth - 1);
                if (childValue < value)
                {
                    value = childValue;
                    best = child;
                }
            }

            return (value, best);
        }
    }
    
    State FindNextEnemyOption(State start)
    {
        var (score, next) = Minimax(start, 10);
        return next;
    }

    State[] FindEnemyStrategyOld(State start)
    {
        int depth = 1;
        while (depth < 10000)
        {
            State[] result = FindEnemyStrategy(start, depth++);
            if (result != null)
                return result;
        }

        throw new Exception("Found no path!");
    }

    State[] FindEnemyStrategy(State start, int depth)
    {
        HashSet<State> visited = new HashSet<State>();
        visited.Add(start);

        Stack<State> path = new Stack<State>();
        path.Push(start);

        while (path.Count > 0)
        {
            bool foundCandidate = false;
            foreach (var neighbor in path.Peek().GetAdjacent())
            {
                if (visited.Contains(neighbor))
                    continue;
                if (depth > 0)
                {
                    visited.Add(neighbor);
                    path.Push(neighbor);
                    depth--;
                    if (neighbor.IsEnemyWinState())
                    {
                        return path.Reverse().ToArray();
                    }

                    foundCandidate = true;
                    break;
                }
            }

            if (!foundCandidate)
            {
                path.Pop();
                depth++;
            }
        }

        return null;
    }
}
