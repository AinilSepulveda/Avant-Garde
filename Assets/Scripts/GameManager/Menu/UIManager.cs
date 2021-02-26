using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] private MainMenu _mainMenu;
    [SerializeField] private PauseMenu _pauseMenu;

    [SerializeField] private Camera _dummyCamera;
    [SerializeField] private GameObject UnitFrame;
    //[SerializeField] private GameObject Inventory;
    [SerializeField] private GameObject instrucciones;
    [SerializeField] private GameObject logo;
    //[SerializeField] private GameObject panelstats;
    //   [SerializeField] private  QuestTrackerPanel TrackerPanel;
    //  [SerializeField] private GameObject InventoryDisplay;

    [SerializeField] private Image healhBar;
    [SerializeField] private Image ManaBar;
    [SerializeField] private TMPro.TextMeshProUGUI levelTex;

    [Space(5)]
    [SerializeField] private Image spellsCooldown;

    public Events.EventFadeComplete OnMainMenuFadeComplete;

    private void Start()
    {
      //  DontDestroyOnLoad(gameObject);

        _mainMenu.OnFadeComplete.AddListener(HandleMainMenuFadeComplete);

        GameManager.Instance.OnGameStateChanged.AddListener(HandleGameStateChanged);


    }

    private void Update()
    {
        // Debug.Log(GameManager.Instance.CurrentGameState);
        if (GameManager.Instance.CurrentGameState == GameManager.GameState.PREGAME)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                GameManager.Instance.StartGame();
            }
        }
        //if (Input.GetKeyDown(KeyCode.C) && !panelstats.activeInHierarchy)
        //{
        //    panelstats.gameObject.SetActive(true);
        //}
        //else if (Input.GetKeyDown(KeyCode.C) && panelstats.gameObject.activeInHierarchy)
        //{
        //    panelstats.gameObject.SetActive(false);
        //}
    }

    private void HandleMainMenuFadeComplete(bool fadeIn)
    {
        // pass it on
        OnMainMenuFadeComplete.Invoke(fadeIn);

    }

    private void HandleGameStateChanged(GameManager.GameState currentState, GameManager.GameState previousState)
    {


        _pauseMenu.gameObject.SetActive(currentState == GameManager.GameState.PAUSED);
        _dummyCamera.gameObject.SetActive(currentState == GameManager.GameState.PREGAME);

        UnitFrame.gameObject.SetActive( currentState == GameManager.GameState.RUNNING || currentState == GameManager.GameState.PAUSED);

        //Inventory.gameObject.SetActive(currentState == GameManager.GameState.RUNNING);
        //panelstats.gameObject.SetActive(currentState == GameManager.GameState.RUNNING);

        instrucciones.gameObject.SetActive(currentState == GameManager.GameState.PREGAME);
        logo.gameObject.SetActive(currentState == GameManager.GameState.PREGAME);
        InitUnitFrame();
        //  TrackerPanel.gameObject.SetActive(currentState == GameManager.GameState.RUNNING);
        //  InventoryDisplay.gameObject.SetActive(currentState == GameManager.GameState.RUNNING);
        Debug.Log(currentState + " " + previousState);
    }

    public void SetDummyCameraActive(bool active)
    {
        _dummyCamera.gameObject.SetActive(active);
    }

    public void InitUnitFrame()
    {
        levelTex.text = "Nivel: 1";
        healhBar.fillAmount = 1;
        spellsCooldown.fillAmount = 1;
    }

    public void UpdateUnitFrame(HeroController hero)
    {
        int curHeath = hero.GetCurrentHealth();
        int maxHeath = hero.GetMaxHealth();

        int maxMana = hero.GetMaxMana();
        int curMana = hero.GetCurrentMana();
        float heroSpell1 = hero.Spell.Cooldown;

        healhBar.fillAmount = (float)curHeath / maxHeath;
        ManaBar.fillAmount = (float)curMana / maxMana;

        //spellsCooldown.fillAmount = hero.timercooldown / hero.Spell.Cooldown ;
        levelTex.text = "Nivel: " + hero.GetCurrentLevel().ToString();
    }
}
