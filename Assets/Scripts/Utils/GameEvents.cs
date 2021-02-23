using UnityEngine.Events;

public class GameEvents
{
    // The event of the game state changes and handle a transition between two game states
    [System.Serializable] public class EventGameState : UnityEvent<GameManager.GameState, GameManager.GameState> { }

    // The event of the score changes, and sends new score
    [System.Serializable] public class EventScoreChanges : UnityEvent<int> { }
    
    // The event of new scene loads, and sends loaded scene(level) name
    [System.Serializable] public class EventSceneChanges : UnityEvent<string> { }

    // The event of Level Data loaded
    [System.Serializable] public class EventLevelData : UnityEvent { }
}
