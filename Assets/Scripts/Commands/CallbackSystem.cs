using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CallbackSystem : MonoBehaviour
{
    #region Singleton
    private static CallbackSystem _instance;
    public static CallbackSystem Instance {
        get {
            if (_instance == null) {
                // Creates the new Object in the scene
                GameObject go = new GameObject("CallbackSystem");
                go.AddComponent<CallbackSystem>();
            }

            return _instance;
        }
    }
    #endregion
    public delegate void CommandSystem();
    public static event CommandSystem command;
    // Start is called before the first frame update
    void Awake() {
        _instance = this;
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // if (Input.GetKeyDown(KeyCode.Space))
        // {
            if (command != null)
            {
                command();
                command = null;
            }
        // }
    }


}
