using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System;

public class NetworkService {
    private const string xmlApi = "https://api.openweathermap.org/data/2.5/weather?q=Warsaw&mode=xml&appid=77baa391b2670e4ce261f2653c7ed7e2";

    private IEnumerator CallAPI(string url, Action<string> callback) {
        using (UnityWebRequest request = UnityWebRequest.Get(url)) {

            yield return request.Send();

            if (request.isNetworkError) {
                Debug.LogError("network problem: " + request.error);
            }
            else if (request.responseCode != (long) System.Net.HttpStatusCode.OK) {
                Debug.LogError("responser error: " + request.responseCode);
            }
            else {
                callback(request.downloadHandler.text);
            }
        }
    }

    public IEnumerator GetWeatherXML(Action<string> callback) {
        return CallAPI(xmlApi, callback);
    }
}
