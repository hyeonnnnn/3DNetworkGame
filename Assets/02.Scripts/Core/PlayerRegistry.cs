using System.Collections.Generic;
using UnityEngine;

public static class PlayerRegistry
{
    private static readonly Dictionary<int, GameObject> _players = new();

    public static void Register(int actorNumber, GameObject player)
    {
        _players[actorNumber] = player;
    }

    public static void Unregister(int actorNumber)
    {
        _players.Remove(actorNumber);
    }

    public static GameObject Find(int actorNumber)
    {
        _players.TryGetValue(actorNumber, out var player);
        return player;
    }

    public static void Clear()
    {
        _players.Clear();
    }
}
