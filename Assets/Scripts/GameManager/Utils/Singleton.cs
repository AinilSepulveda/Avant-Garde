using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    private static T instance;
    public static T Instance
    {
        get
        {
            return instance;
        }
        set
        {
            if(null == instance)
            {
                instance = value;
                DontDestroyOnLoad(instance.gameObject);
            }
            else if (instance != value)
            {
                Destroy(value.gameObject);
            }
        }
    }
    public virtual void Awake()
    {
        Instance = this as T;
    }

    //public static bool IsInitialized
    //{
    //    get
    //    {
    //        return instance != null;
    //    }
    //}
	
    //protected virtual void Awake()
    //{
    //    if (instance != null)
    //    {
    //        Debug.LogErrorFormat("[Singleton] Trying to instantiate a second instance of singleton class {0}", GetType().Name);
    //    }
    //    else
    //    {
    //        instance = (T) this;
    //    }
    //}

    //protected virtual void OnDestroy()
    //{
    //    if (instance == this)
    //    {
    //        instance = null;
    //    }
    //}
}
