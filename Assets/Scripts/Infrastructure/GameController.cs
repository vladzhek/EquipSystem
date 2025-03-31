using Infrastructure;
using Services;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    private static ServiceLocator _services;
    public static ServiceLocator Services => _services;
    private void Awake()
    {
        if (_services != null)
        {
            Destroy(gameObject);
            return;
        }

        _services = new ServiceLocator();
        DontDestroyOnLoad(gameObject);
        Bootstrap();
    }
    
    private void Bootstrap()
    {
        InitializeServices();
        LoadGame();
    }

    private void InitializeServices()
    {
        _services.Register(new DataService());
        _services.Register(new ProgressService());
        _services.Register(new SaveLoadService());
    }

    private void LoadGame()
    {
        SceneManager.LoadScene("Main");
    }
}
