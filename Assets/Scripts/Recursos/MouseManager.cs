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
        _useDefaultCursor = currentState != GameManager.GameState.RUNNING;

    }
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.K))
        //{

        //        showGizmo = false;
           
        //}
        //if (Input.GetKeyUp(KeyCode.K))
        //{
        //    showGizmo = true;
        //}

        //if (_useDefaultCursor)
        //{
        //    Cursor.SetCursor(pointer, new Vector2(0, 0), CursorMode.Auto);
        //    return;
        //}

        // Raycast into scene
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 50, clickableLayer.value))
        {
            Vector3 targerPosition = Input.mousePosition;
            Debug.DrawRay(Camera.main.ScreenPointToRay(targerPosition).origin, hit.collider.transform.TransformDirection(Vector3.forward));

            bool isNPC = false;
            bool door = false;
            bool UI = false;
            if (hit.collider.gameObject.tag == "Doorway")
            {
                Cursor.SetCursor(doorway, new Vector2(16, 16), CursorMode.Auto);
                door = true;
            }

            else if (hit.collider.gameObject.tag == "NPC_Mision")
            {

                isNPC = true;
                Cursor.SetCursor(NPC_Quest, new Vector2(16, 16), CursorMode.Auto);
                Debug.Log(isNPC);
            }
            else if (hit.collider.gameObject.tag == "UI")
            {


                UI = true;
                Cursor.SetCursor(pointer, mousePointer, CursorMode.Auto);

            }
            else
            {
                if (showGizmo == false)
                {
                    Cursor.SetCursor(target, mouseTarget, CursorMode.Auto);
                }
                if (showGizmo == true)
                {
                    Cursor.SetCursor(null, Vector2.zero,  CursorMode.Auto);
                }
               
            }

            bool isAttackble = hit.collider.GetComponent(typeof(IAttackable)) != null;
            if (isAttackble)
            {
                Cursor.SetCursor(swood, mouseSword, CursorMode.Auto);

            }

            if (Input.GetMouseButton(0))
            {
                Debug.Log( hit.collider.name); 
                if (door)
                {
                    Transform doorway = hit.collider.gameObject.transform;
                    HeroOnClickEnvironment.Invoke(doorway.position + doorway.forward * 10);
                    //Si toco la puerta, pues me traslada el otro lado tomando la posicion de esta + hacia al frente * 10;
                }
                
                else if(door == false && UI == false)
                { //sino se mueve normal xd
                    HeroOnClickEnvironment.Invoke(hit.point);
                }
                else if (isNPC)
                {
                    GameObject NPC = hit.collider.gameObject;
                    NpcMision.Invoke(NPC);

                }
                else if (hit.collider.gameObject.tag == "UI")
                {


                    Cursor.SetCursor(pointer, new Vector2(16, 16), CursorMode.Auto);

                }

            }
            if (Input.GetMouseButtonDown(1))
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
                 else
                {
                    HeroOnClickEnvironment.Invoke(hit.point);
                }
            }
            if (Input.GetKeyDown(KeyCode.Q))
            {
                GameObject attackable = hit.collider.gameObject;
                OnSpellAttackable.Invoke(attackable);
            }
        }
        else
        {
            Cursor.SetCursor(pointer, Vector2.zero, CursorMode.Auto);
        }
    }
    void OnDrawGizmosSelected()
    {
        // Draws a 5 unit long red line in front of the object
        Gizmos.color = Color.red;
        Vector3 direction = transform.TransformDirection(Vector3.forward) * 5;

    }
}

[System.Serializable] //Tener informacion de vector3. 
public class EventGameObject : UnityEvent<GameObject> { }

[System.Serializable ] //Tener informacion de Gameobject. 
public class EventVector3 : UnityEvent<Vector3> { }



