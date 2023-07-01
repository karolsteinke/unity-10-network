using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;

public class WeatherManager : MonoBehaviour, IGameManager
{
    public ManagerStatus status {get; private set;}
    public float cloadValue {get; private set;}
    private NetworkService _network;

    public void Startup(NetworkService service) {
        Debug.Log("Weather manager starting..."); 
        _network = service;

        //Call coroutine which requests data from server and calls 'OnXMLDataLoaded' callback once ready
        StartCoroutine(_network.GetWeatherXML(OnXMLDataLoaded));
        
        status = ManagerStatus.Initializing;
    }

    //Callback method to call once the data is loaded
    public void OnXMLDataLoaded(string data) {
        //Parsing XML
        XmlDocument doc = new XmlDocument();
        doc.LoadXml(data);
        XmlNode root = doc.DocumentElement;

        //Pull out a single node from data and convert value to a 0-1 float
        XmlNode node = root.SelectSingleNode("clouds");
        string value = node.Attributes["value"].Value;
        cloadValue = XmlConvert.ToInt32(value) / 100.0f;
        Debug.Log("Value: " + cloadValue);

        Messenger.Broadcast(GameEvent.WEATHER_UPDATED);

        status = ManagerStatus.Started;
    }
}
