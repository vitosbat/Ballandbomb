using UnityEngine.Events;

public class GameEvents
{
    // The event of the game state changes and handle a transition between two game states
    [System.Serializable] public class EventGameState : UnityEvent<GameManager.GameState, GameManager.GameState> { }

    // The event of points changes
    [System.Serializable] public class EventPointsChange : UnityEvent<int> { }
     
}
