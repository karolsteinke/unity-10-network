using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(WeatherManager))]

public class Managers : MonoBehaviour {
    public static WeatherManager Weather {get; private set;}

    private List<IGameManager> _startSequence;

    void Awake() {
        Weather = GetComponent<WeatherManager>();

        _startSequence = new List<IGameManager>();
        _startSequence.Add(Weather);

        StartCoroutine(StartupManagers());
    }

    private IEnumerator StartupManagers() {     
        NetworkService network = new NetworkService();
        
        //Call startup for all modules
        foreach (IGameManager manager in _startSequence) {
            manager.Startup(network);
        }

        yield return null; //pause for one frame

        //repeat while loop unitl all modules are ready
        int numReady = 0;
        int numModules = _startSequence.Count;
        while (numReady < numModules) {
            int lastReady = numReady;
            numReady = 0;
            foreach (IGameManager manager in _startSequence) {
                if (manager.status == ManagerStatus.Started) {
                    numReady++;
                }
            }
            if (numReady > lastReady) {
                Debug.Log("Managers progress: " + numReady + "/" + numModules);
            }
            yield return null; //pause for one frame
        }

        Debug.Log("All managers started up");
    }
}
