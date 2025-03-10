# Game Events
[![Unity 2020.1+](https://img.shields.io/badge/unity-2020.1%2B-blue.svg)](https://unity3d.com/get-unity/download)
[![License: MIT](https://img.shields.io/badge/License-MIT-brightgreen.svg)](https://github.com/qhenshaw/GameEvents/blob/main/LICENSE.md)

GameEvents is a `ScriptableObject` driven event system for decoupling `Monobehaviour` components in Unity. This system was largely inspired by the following Unite presentations:

[Richard Fine - Overthrowing the MonoBehaviour Tyranny in a Glorious Scriptable Object Revolution](https://www.youtube.com/watch?v=6vmRwLYWNRo)  
[Ryan Hipple - Game Architecture with Scriptable Objects](https://www.youtube.com/watch?v=raQ3iHhE_Kk)

Each event is represented by a `GameEventAsset` object that is saved in the project assets, which can then be referenced in your own code or through the included `GameEventCaller` and `GameEventListener` components.

A custom editor is included that automates the creation of new GameEvent Asset/Caller/Listener classes.

## System Requirements
Unity 2020.1 or later, GameEvents uses the Serializable Generics functionality added in 2020.1 and will absolutely not function in any version before.

> [!WARNING]
> Requires Odin Inspector for custom editor functionality, feel free to fork and rewrite with Naughty Attributes or similar.

## Installation
Use the Package Manager and use Add package from git URL, using the following: 
```
https://github.com/qhenshaw/GameEvents.git
```

## Usage
### Creating GameEventAssets
Create a `GameEventAsset` of the desired type through:
```
Project View => Create => Events => [SomeType] Event Asset
```

### Invoking GameEvents
There are two options for invoking GameEvents

1. Use the included `GameEventCaller` component of the appropriate type. Select the desired Unity event to initiate, and assign the value and `GameEventAsset`.

2. Reference the GameEventAsset and call its Invoke method:
```csharp
[SerializeField] private TransformEventAsset _playerSpawnedEventAsset;
[SerializeField] private Transform _playerTransform;

private void Start()
{
      _playerSpawnedEventAsset.Invoke(_playerTransform);
}
```

### Subscribing to GameEvents
There are again two options for subscribing to GameEvents

1. Use the included `GameEventListener` component of the appropriate type. Assign the `GameEventAsset` to listen to, and then the `OnGameEventInvoked` UnityEvent to add subscriber methods.

2. Reference the GameEventAsset and add listeners to its OnInvoked event:
```csharp
[SerializeField] private TransformEventAsset _playerSpawnedEventAsset;

private void OnEnable()
{
      _playerSpawnedEventAsset.OnInvoked.AddListener(PlayerSpawned);
}

private void OnDisable()
{
      _playerSpawnedEventAsset.OnInvoked.RemoveListener(PlayerSpawned);
}

private void PlayerSpawned(Transform player)
{
      Debug.Log($"Player Spawned: {player.gameObject.name}");
}
```

### Adding Additional Types
Additional GameEvent types can be added through an editor window found here:
```
Tools => Game Events => Create New Event Scripts
```
This tool will create the Asset, Caller, and Listener scripts all at once.
