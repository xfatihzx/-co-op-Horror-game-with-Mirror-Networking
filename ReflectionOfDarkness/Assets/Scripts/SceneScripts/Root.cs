using UnityEngine;
using UnityEngine.SceneManagement;

#pragma warning disable
public class Root : CacheBehaviour<Root>
{
    //public const bool singlePlayer = false;
    //public SceneType currentScene = SceneType.SceneRoot;


    //private void OnEnable()
    //{
    //    // Scene load manage event import
    //    SceneManager.sceneLoaded += OperationCompletedLoadScene;
    //}

    //private void OnDisable()
    //{
    //    // Scene load manage event export
    //    SceneManager.sceneLoaded -= OperationCompletedLoadScene;
    //}

    //private void Start()
    //{
    //    DontDestroyOnLoad(
    //        Tags
    //            .DirectionalLight
    //            .With()
    //    );
    //    DontDestroyOnLoad(
    //        Tags
    //            .MainCamera
    //            .With()
    //    );

    //    //Load first Scene
    //    LoadSceneSinglePlayer(SceneType.SceneGameMenu, LoadSceneMode.Single, LocalPhysicsMode.Physics2D);
    //}

    //private void OperationCompletedLoadScene(Scene scene, LoadSceneMode loadSceneMode)
    //{
    //    return;
    //    //Wait any code
    //    if (currentScene == SceneType.SceneGameMenu)
    //    {
    //        Tags
    //            .CanvasGameMenu
    //            .With()
    //            .Then<Canvas>()
    //            .worldCamera = Tags
    //                               .MainCamera
    //                               .With()
    //                               .Then<Camera>();
    //    }
    //    else if (currentScene == SceneType.SceneNewGame)
    //    {

    //        Tags
    //            .CanvasGameMenu
    //            .With()
    //            .Then<Canvas>()
    //            .worldCamera = Tags
    //                               .MainCamera
    //                               .With()
    //                               .Then<Camera>();
    //    }
    //    else if (currentScene == SceneType.SceneJoinGame)
    //    {

    //        Tags
    //            .CanvasGameMenu
    //            .With()
    //            .Then<Canvas>()
    //            .worldCamera = Tags
    //                               .MainCamera
    //                               .With()
    //                               .Then<Camera>();
    //    }
    //}

    ///// <summary>
    ///// Load new Scene (SinglePlayer)
    ///// </summary>
    ///// <param name="sceneType"></param>
    //public void LoadSceneSinglePlayer(SceneType sceneType, LoadSceneMode loadSceneMode, LocalPhysicsMode localPhysicsMode)
    //{
    //    currentScene = sceneType;

    //    SceneManager.LoadSceneAsync(sceneType.ToString(), new LoadSceneParameters
    //    {
    //        loadSceneMode = loadSceneMode,
    //        localPhysicsMode = localPhysicsMode
    //    });
    //}

    ///// <summary>
    ///// Load new Scene (singeleton)
    ///// </summary>
    ///// <param name="sceneType"></param>
    //public void LoadSceneMultiPlayer(SceneType sceneType)
    //{
    //    currentScene = sceneType;

    //    network.ServerChangeScene(currentScene.ToString());
    //}
}
#pragma warning restore