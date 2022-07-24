using UnityEngine;

namespace Base.Tools
{
    public class MonoBehaviourSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance;
        private static bool _dontDestroyOnLoad;

        public static bool IsInstantiated => _instance != null;

        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    var objects = FindObjectsOfType<T>();
                    switch (objects.Length)
                    {
                        case 0:
                            {
                                GameObject newObject = new GameObject();
                                _instance = newObject.AddComponent<T>();
                                _instance.name = typeof(T).Name;

                                if (_dontDestroyOnLoad)
                                {
                                    DontDestroyOnLoad(_instance);
                                }

                                break;
                            }

                        case 1:
                            {
                                _instance = objects[0];

                                if (_dontDestroyOnLoad)
                                {
                                    DontDestroyOnLoad(_instance);
                                }

                                break;
                            }

                        default:
                            {
                                Debug.LogAssertion("You must have at most one " + typeof(T).Name + " in the scene.");
                                break;
                            }
                    } // switch
                }

                return _instance;
            }
        }

        public MonoBehaviourSingleton(bool dontDestroyOnLoad = false)
        {
            _dontDestroyOnLoad = dontDestroyOnLoad;
        }

        private void Awake()
        {

            if (_instance == null)
            {
                _instance = this as T;

                if (_dontDestroyOnLoad)
                {
                    DontDestroyOnLoad(_instance);
                }

                SingletonAwake();
            }
            else if (_instance != this)
            {
                Destroy(gameObject);
            }
        }

        private void SingletonAwake()
        {

        }
    }
}
