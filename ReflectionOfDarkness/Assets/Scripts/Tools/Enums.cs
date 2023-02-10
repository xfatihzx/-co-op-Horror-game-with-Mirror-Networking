
using System;

public enum EntityType
{
    None = 0x1,
    InteractionalObject = 0x2,
    InspectObject = 0x4,
    Player = 0x8,
    Enemy = 0x16,
    Readable = 0x32,
    Holdableobject = 0x64,
    CollectibleObject = 0x128,
    TaskItem = 0x256
}

/// <summary>
/// audioClip types for AudioSystem
/// </summary>
public enum AudioType
{
    None,
    Click,
    Win,
    Lose
}
[Serializable]
/// <summary>
/// All Scenes
/// </summary>
public enum SceneType
{
    None,
    SceneRoot,
    SceneIntro,
    SceneGameMenu,
    SceneSettings,
    SceneNewGame,
    SceneJoinGame,
    SceneLobby,
    SceneGame,
    SceneGameEnd,
    SceneCredits
    /*
     CutScene
     */
}

public static class Tags
{
    public const string Untagged = "Untagged";
    public const string Respawn = "Respawn";
    public const string Finish = "Finish";
    public const string EditorOnly = "EditorOnly";
    public const string MainCamera = "MainCamera";
    public const string Player = "Player";
    public const string PlayerBody = "PlayerBody";
    public const string GameController = "GameController";
    public const string Ground = "Ground";
    public const string Target = "Target";
    public const string FearParameter = "FearParameter";
    public const string DoorAreaFront = "DoorAreaFront";
    public const string DoorAreaBack = "DoorAreaBack";
    public const string DirectionalLight = "DirectionalLight";
    public const string Networking = "Networking";
    public const string CanvasGameMenu = "CanvasGameMenu";
    public const string CanvasSettings = "CanvasSettings";
    public const string CanvasCreateGame= "CanvasCreateGame";
    public const string CanvasJoinGame = "CanvasJoinGame";
    public const string CanvasLobby = "CanvasLobby";
    public const string CanvasGame = "CanvasGame";
    public const string SceneScript = "SceneScript";
    public const string CinemaMachineTarget = "CinemaMachineTarget";
    public const string PlayerFollowCamera = "PlayerFollowCamera";
}