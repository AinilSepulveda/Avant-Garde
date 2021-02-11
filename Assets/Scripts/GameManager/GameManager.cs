using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public enum GameState
    {
        PREGAME,
        RUNNING,
        PAUSED
    }

    public GameObject[] SystemPrefabs;
    public EventGameState OnGameStateChanged;

    List<AsyncOperation> _loadOperations;
    List<GameObject> _instancedSystemPrefabs;
    GameState _currentGameState = GameState.PREGAME;

    string _currentLevelName;

    public string NameLevelRunnig;

    public GameState CurrentGameState
    {
        get { return _currentGameState; }
        private set { _currentGameState = value; }
    }

    private HeroController heroController;

    private HeroController hero //Se hace asi para que no ponerlo en un cada frame. 
    {
        get
        {
            if(null == heroController)
            {
                heroController = FindObjectOfType<HeroController>();
            }
            return heroController;
        }
    }

    void Start()
    {
      //  DontDestroyOnLoad(gameObject);
        
        _instancedSystemPrefabs = new List<GameObject>();
        _loadOperations = new List<AsyncOperation>();
        MusicManager.Instance.PlaySoundEffect(MusicEnum.Menu);
        UIManager.Instance.OnMainMenuFadeComplete.AddListener(HandleMainMenuFadeComplete);

        OnGameStateChanged.Invoke(GameState.PREGAME, _currentGameState);
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.P))
        {
            TogglePause();
        }

        if (_currentGameState == GameState.PREGAME)
        {
            return;
        }


    }

    void OnLoadOperationComplete(AsyncOperation ao)
    {
        if (_loadOperations.Contains(ao))
        {
            _loadOperations.Remove(ao);

            if (_loadOperations.Count == 0)
            {
              //  UpdateState(GameState.RUNNING);
            }
        }
    }

    void OnUnloadOperationComplete(AsyncOperation ao)
    {
        // Clean up level is necessary, go back to main menu
    }

    void UpdateState(GameState state)
    {
        GameState previousGameState = _currentGameState;
        _currentGameState = state;

        switch (CurrentGameState)
        {
            case GameState.PREGAME:
                // Initialize any systems that need to be reset
                Time.timeScale = 1.0f;
                LoadLevel("Boot");
                MusicManager.Instance.PlaySoundEffect(MusicEnum.Menu);
              //  UIManager.Instance.SetDummyCameraActive(true);
                break;

            case GameState.RUNNING:
                //  Unlock player, enemies and input in other systems, update tick if you are managing time
                Time.timeScale = 1.0f;

                break;

            case GameState.PAUSED:
                // Pause player, enemies etc, Lock other input in other systems
                Time.timeScale = 0.0f;
                break;

            default:
                break;
        }
    //    Debug.Log(_currentGameState + "<- Current - Previous -> " + previousGameState);        
        OnGameStateChanged.Invoke(_currentGameState, previousGameState);
    }

    void InstantiateSystemPrefabs()
    {
        GameObject prefabInstance;
        for (int i = 0; i < SystemPrefabs.Length; ++i)
        {
            prefabInstance = Instantiate(SystemPrefabs[i]);
            _instancedSystemPrefabs.Add(prefabInstance);
        }
    }

    void HandleMainMenuFadeComplete(bool fadeIn)
    {
        if (!fadeIn)
        {
           // UnloadLevel(_currentLevelName);
        }
    }

    public void LoadLevel(string levelName)
    {

        AsyncOperation ao = SceneManager.LoadSceneAsync(levelName, LoadSceneMode.Single);
        if (ao == null)
        {
            Debug.LogError("[GameManager] Unable to load level " + levelName);
            return;
        }

        ao.completed += OnLoadOperationComplete;
        _loadOperations.Add(ao);

        _currentLevelName = levelName;
    }

    //Esta hacia por que hacia bugs feos
    //public void UnloadLevel(string levelName)
    //{
    //    AsyncOperation ao = SceneManager.UnloadSceneAsync(levelName);
    //    ao.completed += OnUnloadOperationComplete;
    //}

    public void TogglePause()
    {
        UpdateState(_currentGameState == GameState.RUNNING ? GameState.PAUSED : GameState.RUNNING);
    }

    public void RestartGame()
    {
      //  CharacterInventory.Instance.ResetInventory();
        UpdateState(GameState.PREGAME);
        
    }

    public void StartGame()
    {
      //  CharacterInventory.Instance.ResetInventory();
      //  CharacterInventory.Instance.isReset = true;
        LoadLevel(NameLevelRunnig);
        UpdateState(GameState.RUNNING);
        MusicManager.Instance.PlaySoundEffect(MusicEnum.Ambient);
    }

    public void QuitGame()
    {
        // Clean up application as necessary
        // Maybe save the players game

        Debug.Log("[GameManager] Quit Game.");

        Application.Quit();
    }

    public void OnHeroLeveledUp(int newLevel)
    {
        UIManager.Instance.UpdateUnitFrame(hero);

    }
    public void OnHeroDamaged(int damage)
    {
        UIManager.Instance.UpdateUnitFrame(hero);
        SoundManager.Instance.PlaySoundEffect(SoundEffect.HeroHit);
    }
    public void OnHeroGainedHealth(int health)
    {
        UIManager.Instance.UpdateUnitFrame(hero);
    }
    public void OnHeroGainedMana(int Mana)
    {
        UIManager.Instance.UpdateUnitFrame(hero);
    }

    protected void OnDestroy()
    {
        if (_instancedSystemPrefabs == null)
            return;

        for(int i = 0; i < _instancedSystemPrefabs.Count; i++)
        {
            Destroy(_instancedSystemPrefabs[i]);
        }
        _instancedSystemPrefabs.Clear();
    }

    public void OnHeroDied()
    {
        UIManager.Instance.UpdateUnitFrame(hero);
       // SoundManager.Instance.PlaySoundEffect(SoundEffect.MobDeath);
    }
    public void OnHeroInit()
    {
        UIManager.Instance.InitUnitFrame();
    }

    [System.Serializable] public class EventGameState : UnityEvent<GameState, GameState> { }
}
