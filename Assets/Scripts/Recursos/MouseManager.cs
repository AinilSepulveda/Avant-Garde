using UnityEngine;
using UnityEngine.Events;

public class MouseManager : MonoBehaviour
{
    public LayerMask clickableLayer; // layermask used to isolate raycasts against clickable layers

    public Texture2D pointer; // normal mouse pointer
    public Texture2D target; // target mouse pointer
    public Texture2D doorway; // doorway mouse pointer
    public Texture2D swood;
    public Texture2D NPC_Quest;

    public EventVector3 HeroOnClickEnvironment;
    public EventGameObject OnClickAttackble;
    public EventGameObject OnSpellAttackable;
    public EventGameObject NpcMision;
    public EventGameObject BuildCraftingSpells;

    private bool _useDefaultCursor = false;

    [SerializeField]
    public bool showGizmo;

    private Vector2 mouseTarget;
    private Vector2 mousePointer = new Vector2(340, 376) ;
    private Vector2 mouseSword = new Vector2(373, 492) ;
    private void Start()
    {   
        if(GameManager.Instance != null) //Se inicia la mismo tiempo que Gamager y sino, no se inicia
        GameManager.Instance.OnGameStateChanged.AddListener(HandleGameStateChanged);

    }

    void HandleGameStateChanged(GameManager.GameState currentState, GameManager.GameState previosState)
    {
        _useDefaultCursor = (currentState != GameManager.GameState.RUNNING);

    }
    void Update()
    {

        if (_useDefaultCursor)
        {
            Cursor.SetCursor(pointer, new Vector2(0, 0), CursorMode.Auto);
            return;
        }

        // Raycast into scene
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 50, clickableLayer.value))
        {
            Vector3 targerPosition = Input.mousePosition;
            Debug.DrawRay(Camera.main.ScreenPointToRay(targerPosition).origin, hit.collider.transform.TransformDirection(Vector3.forward));

            Cursor.SetCursor(target, mousePointer, CursorMode.Auto);

            bool CraftingSpells = false;
            bool UI = false;
            bool door = false;
            if (hit.collider.gameObject.tag == "Doorway")
            {
                Cursor.SetCursor(doorway, new Vector2(16, 16), CursorMode.Auto);
                door = true;
            }

            bool isNPC = false;
            if (hit.collider.gameObject.tag == "NPC_Mision")
            {

                isNPC = true;
                Cursor.SetCursor(NPC_Quest, new Vector2(16, 16), CursorMode.Auto);
                Debug.Log(isNPC);
            }

            if (hit.collider.gameObject.tag == "CraftingSpells")
            {

                CraftingSpells = true;
                Cursor.SetCursor(NPC_Quest, new Vector2(16, 16), CursorMode.Auto);
                Debug.Log(isNPC);
            }
            if (hit.collider.gameObject.tag == "UI")
            {
                Debug.Log("UI TAG" + UI);

                UI = true;
                Cursor.SetCursor(pointer, mousePointer, CursorMode.Auto);

            }

            bool isAttackble = hit.collider.GetComponent(typeof(IAttackable)) != null;
            if (isAttackble)
            {
                Cursor.SetCursor(swood, mouseSword, CursorMode.Auto);

            }

            if (Input.GetMouseButton(0))
            {
               // Debug.Log( hit.collider.name); 
                if (door)
                {
                    Transform doorway = hit.collider.gameObject.transform;
                    HeroOnClickEnvironment.Invoke(doorway.position + doorway.forward * 10);
                    //Si toco la puerta, pues me traslada el otro lado tomando la posicion de esta + hacia al frente * 10;
                }
                
                else if(door == false && UI == false)
                { //sino se mueve normal xd
                    HeroOnClickEnvironment.Invoke(hit.point);
                  //  Debug.Log("Door " + door + "ui " + UI);
                    BuildCraftingSpells.Invoke(hit.collider.gameObject);

                }
                else if (isNPC)
                {
                    GameObject NPC = hit.collider.gameObject;
                    NpcMision.Invoke(NPC);

                }

            }
            else if (Input.GetMouseButtonDown(1))
            {
                 if (isAttackble)
                {
                    GameObject attackable = hit.collider.gameObject;
                    //    Vector3 asd = hit.point;
                    //    asd = attackable.transform.position;
                    transform.LookAt(attackable.transform.position);
                    Debug.Log(attackable.name);
                    OnClickAttackble.Invoke(attackable);
                }
                 else if (isAttackble == false && UI == false)
                {
                    HeroOnClickEnvironment.Invoke(hit.point);
                }
            }
        }
    }
}

[System.Serializable] //Tener informacion de vector3. 
public class EventGameObject : UnityEvent<GameObject> { }

[System.Serializable ] //Tener informacion de Gameobject. 
public class EventVector3 : UnityEvent<Vector3> { }



