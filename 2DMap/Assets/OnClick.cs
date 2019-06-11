using Mapbox.Examples;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnClick : MonoBehaviour {

    
    GameObject game;


    // Use this for initialization
    void Start () {
        if (!Variables.AddedListener)
        {

            Variables.PreviousMarker_Button.onClick.AddListener(delegate { ChangeActiveMarker(0); });
            Variables.NextMarker_Button.onClick.AddListener(delegate { ChangeActiveMarker(1); });
            Variables.AddedListener = true;
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void ChangeActiveMarker(int index)
    {
        int i;

        if (index == 0)
        {
            i = -1;
        } else
        {
            i = 1;
        }

        object[] obj = FindObjectsOfType(typeof(GameObject));
        foreach (object o in obj)
        {
            GameObject g = (GameObject) o;

            if (g.name.Equals("CustomMarkerPrefab(Clone)") && int.Parse(g.GetComponentInChildren<TextMesh>().text) == (Variables.Key + (1 * i)))
            {
                Variables.GameObjectFromButton = g;
                DisplayInfo(true);
                return;
            }
        }
    }

    void DisplayInfo(bool GameObjectFromButton)
    {        

        if (GameObjectFromButton)
        {
            game = Variables.GameObjectFromButton;
        } else
        {
            game = gameObject;
        }


        Variables.Key = int.Parse(game.GetComponentInChildren<TextMesh>().text);

        int key = Variables.Key;
        double distance = 0;
        string[] setupArgs = ReadFiles.ReadSetupFile(Variables.SetupFile);
        double setupIncrement = double.Parse(setupArgs[1]);

        if (key == 1)
        {
            Variables.PreviousMarker_Button.interactable = false;
        } else if(key != 1 && !Variables.PreviousMarker_Button.interactable)
        {
            Variables.PreviousMarker_Button.interactable = true;
        }

        if (key == Variables._spawnedObjects.Count)
        {
            Variables.NextMarker_Button.interactable = false;
        } else if (key != Variables._spawnedObjects.Count)
        {
            Variables.NextMarker_Button.interactable = true;
        }

        RemovePreviousMarkerColor();

        Variables.PreviousClickedMarker = game;
        FishRouteInfo fishInfo = Variables.FishRouteInfoDictionary[key];

        Variables.MarkerClicked_Text.text = "Position: " + game.GetComponentInChildren<TextMesh>().text + " / " + Variables._spawnedObjects.Count;
        Variables.Date_Text.text = "Date: " + fishInfo.date.Substring(6, 2) + "." + fishInfo.date.Substring(4, 2) + "." + fishInfo.date.Substring(0, 4);
        game.GetComponentInChildren<SphereCollider>().GetComponent<Renderer>().material.color = Color.blue;

        //Temperature
        Variables.TagDataTemp_Text.text = fishInfo.tagDataTemp.ToString() + " °C";
        Variables.SeaTemp_Text.text = fishInfo.seaTemp.ToString() + " °C";
        Variables.DifferenceTemp_Text.text = CalculateDifference(fishInfo.tagDataTemp, fishInfo.seaTemp).ToString() + " °C";

        //Depth
        Variables.TagDataDepth_Text.text = (-fishInfo.tagDataDepth).ToString() + " m";
        Variables.SeaDepth_Text.text = fishInfo.seaDepth.ToString() + " m";
        Variables.DifferenceDepth_Text.text = CalculateDifference(-fishInfo.tagDataDepth, fishInfo.seaDepth).ToString() + " m";

        //Distance
        distance = CalculateDistance(fishInfo.latitude, fishInfo.longitude, key - 1);
        Variables.PreviousToNewPosition_Text.text = distance.ToString() + " km";
        Variables.PreviousToNewSpeed_Text.text = CalculateSpeed(distance, setupIncrement).ToString() + " km/h";

        distance = CalculateDistance(fishInfo.latitude, fishInfo.longitude, key + 1);
        Variables.ThisToNextPosition_Text.text = distance.ToString() + " km";
        Variables.ThisToNextSpeed_Text.text = CalculateSpeed(distance, setupIncrement).ToString() + " km/h";
    }

    private void OnMouseDown()
    {
        Variables.PreviousMarker_Button.interactable = true;
        Variables.NextMarker_Button.interactable = true;
        DisplayInfo(false);
        
    }

    void RemovePreviousMarkerColor()
    {
        if (Variables.PreviousClickedMarker != null && Variables.PreviousClickedMarker.GetComponentInChildren<TextMesh>().text != game.GetComponentInChildren<TextMesh>().text)
        {
            Variables.PreviousClickedMarker.GetComponentInChildren<SphereCollider>().GetComponent<Renderer>().material.color = Color.white;
        }

    }

    double CalculateDifference(double tag, double sea)
    {
        return Math.Round(Math.Abs(tag - sea), 1);
    }

    double CalculateDistance(double latitude1, double longitude1, int key)
    {
        if (Variables.FishRouteInfoDictionary.ContainsKey(key))
        {
            double latitude2 = Variables.FishRouteInfoDictionary[key].latitude;
            double longitude2 = Variables.FishRouteInfoDictionary[key].longitude;

            var R = 6371; // Radius of the earth in km
            var dLat = Deg2rad(latitude1 - latitude2);  // deg2rad below
            var dLon = Deg2rad(longitude1 - longitude2);
            var a =
                    Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                    Math.Cos(Deg2rad(latitude1)) * Math.Cos(Deg2rad(latitude2)) *
                    Math.Sin(dLon / 2) * Math.Sin(dLon / 2)
                ;
            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            var distance = R * c; // Distance in km
            return Math.Round(distance, 0);
        } else
        {
            return 0;
        }
    }

    double CalculateSpeed(double distance, double setupIncrement)
    {
        return Math.Round(distance / (setupIncrement * 24), 1);
    }
    
    public static double Deg2rad(double deg)
    {
        return deg * (Math.PI / 180);
    }
}
