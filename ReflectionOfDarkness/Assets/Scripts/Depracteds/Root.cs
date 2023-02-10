using UnityEngine;
using UnityEngine.SceneManagement;

#pragma warning disable
namespace Depracteds
{
    public class Root : MonoBehaviour
    {
        public static Root instance;
        public SceneType currentScene = SceneType.SceneRoot;
         //asdsd
        private void OnEnable()
        {
            SceneManager.sceneLoaded += OperationCompletedLoadScene;
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OperationCompletedLoadScene;
        }

        private void Awake()
        {

            if (instance is null)
            {
                #region Intance
                instance = this;
                DontDestroyOnLoad(instance);
                #endregion
            }
            else
                Destroy(this);
        }

        private void Start()
        {
            LoadSceneSinglePlayer(SceneType.SceneGameMenu, LoadSceneMode.Single, LocalPhysicsMode.Physics2D);
        }

        private void OperationCompletedLoadScene(Scene scene, LoadSceneMode loadSceneMode)
        {

        }

        /// <summary>
        /// Load new Scene (SinglePlayer)
        /// </summary>
        /// <param name="sceneType"></param>
        public void LoadSceneSinglePlayer(SceneType sceneType, LoadSceneMode loadSceneMode, LocalPhysicsMode localPhysicsMode)
        {
            currentScene = sceneType;

            SceneManager.LoadSceneAsync(sceneType.ToString(), new LoadSceneParameters
            {
                loadSceneMode = loadSceneMode,
                localPhysicsMode = localPhysicsMode
            });
        }

        /// <summary>
        /// Load new Scene (singeleton)
        /// </summary>
        /// <param name="sceneType"></param>
        public void LoadSceneMultiPlayer(SceneType sceneType)
        {
            currentScene = sceneType;
            MirrorNetworking.instance.ServerChangeScene(currentScene.ToString());
        }
    }
}
#pragma warning restore