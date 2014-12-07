using UnityEngine;

// Simple implementation of a singleton class.
public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{

    private static T _instance;
    private static object _lock = new object();
    private static bool _applicationIsQuitting;

    public static T Instance
    {
        get
        {
            if (_applicationIsQuitting)
            {
                Debug.Log("<Instance> - Application already destroyed");
                return null;
            }
            if (_instance == null)
            {
                _instance = (T)FindObjectOfType(typeof(T));

                if (FindObjectsOfType(typeof(T)).Length > 1)
                {
                    Debug.Log("<Instance> - Something went really wrong " +
                        " - there should never be more than 1 singleton!" +
                        " Reopenning the scene might fix it.");
                    return _instance;
                }
                if (_instance == null)
                {
                    GameObject singleton = new GameObject();
                    _instance = singleton.AddComponent<T>();
                    singleton.name = "(singleton) " + typeof(T).ToString();

                    DontDestroyOnLoad(singleton);

                    Debug.Log("<Instance> - An instance of " + typeof(T) +
                        " is needed in the scene, so '" + singleton +
                        "' was created with DontDestroyOnLoad.");
                }
                else
                {
                    Debug.Log("<Instance> Using instance already created: " +
                        _instance.gameObject.name);
                }
            }
            return _instance;
        }
    }

    public void OnDestroy()
    {
        _applicationIsQuitting = true;
    }


}

