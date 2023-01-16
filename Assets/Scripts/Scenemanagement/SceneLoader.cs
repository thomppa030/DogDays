using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scenemanagement
{
    public class SceneLoader : MonoBehaviour
    {
        [field: SerializeField] public bool LoadSceneAsync { get; set; }
        [field: SerializeField] private string SceneName { get; set; }
    
        void Update()
        {
            if (LoadSceneAsync)
            {
                StartCoroutine(LoadSceneAsynchronously());
            }
        }
    
        private IEnumerator LoadSceneAsynchronously()
        {
            var asyncLoad = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(SceneName, LoadSceneMode.Additive);
            // Wait until the asynchronous scene fully loads
            while (!asyncLoad.isDone)
            {
                yield return null;
            }
        }
    }
}
