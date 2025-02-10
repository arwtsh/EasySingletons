using UnityEngine;
using EasySingletons;

public sealed class GameManager : Singleton<GameManager>
{
    // OnSingletonInit is called once before awake and start.
    protected override void OnSingletonInit()
    {
        
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
