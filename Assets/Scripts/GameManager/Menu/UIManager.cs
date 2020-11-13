using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] private MainMenu _mainMenu;
    [SerializeField] private PauseMenu _pauseMenu;

    [SerializeField] private Camera _dummyCamera;
    [SerializeField] private GameObject UnitFrame;
    [SerializeField] private GameObject Inventory;
    [SerializeField] private GameObject instrucciones;
    [SerializeField] private GameObject logo;
    [SerializeField] private GameObject panelstats;
    //   [SerializeField] private  QuestTrackerPanel TrackerPanel;
    //  [SerializeField] private GameObject InventoryDisplay;

    [SerializeField] private Image healhBar;
    [SerializeField] private TMPro.TextMeshProUGUI levelTex;

    public Events.EventFadeComplete OnMainMenuFadeComplete;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);

        _mainMenu.OnFadeComplete.AddListener(HandleMainMenuFadeComplete);

        GameManager.Instance.OnGameStateChanged.AddListener(HandleGameStateChanged);
    }

    private void Update()
    {
        if (GameManager.Instance.CurrentGameState == GameManager.GameState.PREGAME)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                GameManager.Instance.StartGame();
            }
        }
        if (Input.GetKeyDown(KeyCode.C) && !panelstats.activeInHierarchy)
        {
            panelstats.gameObject.SetActive(true);
        }
        else if (Input.GetKeyDown(KeyCode.C) && panelstats.gameObject.activeInHierarchy)
        {
            panelstats.gameObject.SetActive(false);
        }
    }

    private void HandleMainMenuFadeComplete(bool fadeIn)
    {
        // pass it on
        OnMainMenuFadeComplete.Invoke(fadeIn);

    }

    private void HandleGameStateChanged(GameManager.GameState currentState, GameManager.GameState previousState)
    {
        //switch (currentState)
        //{
        //    case GameManager.GameState.PAUSED:
        //        _pauseMenu.gameObject.SetActive(true);
        //        break;

        //    default:
        //        _pauseMenu.gameObject.SetActive(false);
        //        break;
        //}

        _pauseMenu.gameObject.SetActive(currentState == GameManager.GameState.PAUSED);

        UnitFrame.gameObject.SetActive( currentState == GameManager.GameState.RUNNING || currentState == GameManager.GameState.PAUSED);

        Inventory.gameObject.SetActive(currentState == GameManager.GameState.RUNNING);
        panelstats.gameObject.SetActive(currentState == GameManager.GameState.RUNNING);


        instrucciones.gameObject.SetActive(currentState == GameManager.GameState.PREGAME);
        logo.gameObject.SetActive(currentState == GameManager.GameState.PREGAME);

        //  TrackerPanel.gameObject.SetActive(currentState == GameManager.GameState.RUNNING);
        //  InventoryDisplay.gameObject.SetActive(currentState == GameManager.GameState.RUNNING);
    }

    public void SetDummyCameraActive(bool active)
    {
        _dummyCamera.gameObject.SetActive(active);
    }

    public void InitUnitFrame()
    {
        levelTex.text = "1";
        healhBar.fillAmount = 1;
    }

    public void UpdateUnitFrame(HeroController hero)
    {
        int curHeath = hero.GetCurrentHealth();
        int maxHeath = hero.GetMaxHealth();

        healhBar.fillAmount = (float)curHeath / maxHeath;
        levelTex.text = hero.GetCurrentLevel().ToString();
    }
}
