using UnityEngine.Events;

public class GameEvents
{
    // The event of the game state changes and handle a transition between two game states
    [System.Serializable] public class EventGameState : UnityEvent<GameManager.GameState, GameManager.GameState> { }


    
}
