using System.Collections.Generic;
using UnityEngine.Events;

public class GameEvents
{
    // The class of events of the game state changes and handle a transition between two game states
    [System.Serializable] public class EventGameState : UnityEvent<GameManager.GameState, GameManager.GameState> { }

    
    // The class of events with one integer parameter: score, etc.
    [System.Serializable] public class IntParameterEvent : UnityEvent<int> { }
    

    // The class of events with one string parameter: scene(level) name, player name, etc.
    [System.Serializable] public class StringParameterEvent : UnityEvent<string> { }

    
    // The class of events with List of Player Results as parameter: LeaderBoard, etc.
    [System.Serializable] public class EventLeaderboard : UnityEvent<List<PlayerResult>> { }
}
