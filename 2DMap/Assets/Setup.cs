using Mapbox.Examples;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class Setup
{

    public static void FetchGameObjects()
    {
        //Fetch the Button GameObject
        Variables.changeParameters_Button = GameObject.FindGameObjectWithTag("ChangeParametersButton").GetComponent<Button>();
        Variables.run_Button = GameObject.FindGameObjectWithTag("RunWithNewParameters").GetComponent<Button>();
        Variables.PreviousMarker_Button = GameObject.FindGameObjectWithTag("PreviousMarkerButton").GetComponent<Button>();
        Variables.NextMarker_Button = GameObject.FindGameObjectWithTag("NextMarkerButton").GetComponent<Button>();
        Variables.savePath_Button = GameObject.FindGameObjectWithTag("SavePathButton").GetComponent<Button>();

        Variables.PreviousMarker_Button.interactable = false;
        Variables.NextMarker_Button.interactable = false;

        //Fetch the Dropdown GameObject the script is attached to
        Variables.acceptable_Dropdown = GameObject.FindGameObjectWithTag("DropDownAcceptableRoutes").GetComponent<Dropdown>();
        Variables.unacceptable_Dropdown = GameObject.FindGameObjectWithTag("DropDownUnacceptableRoutes").GetComponent<Dropdown>();
        Variables.chooseTag_Dropdown = GameObject.FindGameObjectWithTag("ChooseFishTag").GetComponent<Dropdown>();
        Variables.chooseAlgorithm_Dropdown = GameObject.FindGameObjectWithTag("AlgorithmDropdown").GetComponent<Dropdown>();
        Variables.saved_Dropdown = GameObject.FindGameObjectWithTag("DropDownSavedRoutes").GetComponent<Dropdown>();

        //Fetch the Panel GameObject
        Variables.changeParameters_Panel = GameObject.FindGameObjectWithTag("ChangeParametersPanel");

        //Fetch InputField GameObject
        Variables.daysBetweenTag_Field = GameObject.FindGameObjectWithTag("DaysBetweenTag").GetComponent<InputField>();
        Variables.numberOfFish_Field = GameObject.FindGameObjectWithTag("NumberOfFish").GetComponent<InputField>();
        Variables.temperatureDelta_Field = GameObject.FindGameObjectWithTag("Temperature").GetComponent<InputField>();
        Variables.depthDelta_Field = GameObject.FindGameObjectWithTag("Depth").GetComponent<InputField>();
        Variables.fishLength_Field = GameObject.FindGameObjectWithTag("Increment1").GetComponent<InputField>();
        Variables.pathWeight_Field = GameObject.FindGameObjectWithTag("Increment2").GetComponent<InputField>();
        Variables.possiblePaths_Field = GameObject.FindGameObjectWithTag("PossiblePaths").GetComponent<InputField>();

        //Fetch Text GameObject
        Variables.FishTag_Text = GameObject.FindGameObjectWithTag("FishTag").GetComponent<Text>();
        Variables.MarkerClicked_Text = GameObject.FindGameObjectWithTag("MarkerClicked").GetComponent<Text>();
        Variables.TagDataTemp_Text = GameObject.FindGameObjectWithTag("TagTemp").GetComponent<Text>();
        Variables.SeaTemp_Text = GameObject.FindGameObjectWithTag("SeaTemp").GetComponent<Text>();
        Variables.DifferenceTemp_Text = GameObject.FindGameObjectWithTag("DifferenceTemp").GetComponent<Text>();
        Variables.TagDataDepth_Text = GameObject.FindGameObjectWithTag("TagDepth").GetComponent<Text>();
        Variables.SeaDepth_Text = GameObject.FindGameObjectWithTag("SeaDepth").GetComponent<Text>();
        Variables.DifferenceDepth_Text = GameObject.FindGameObjectWithTag("DifferenceDepth").GetComponent<Text>();
        Variables.PreviousToNewPosition_Text = GameObject.FindGameObjectWithTag("Previous").GetComponent<Text>();
        Variables.ThisToNextPosition_Text = GameObject.FindGameObjectWithTag("Next").GetComponent<Text>();
        Variables.PreviousToNewSpeed_Text = GameObject.FindGameObjectWithTag("PreviousSpeed").GetComponent<Text>();
        Variables.ThisToNextSpeed_Text = GameObject.FindGameObjectWithTag("NextSpeed").GetComponent<Text>();
        Variables.Date_Text = GameObject.FindGameObjectWithTag("DateTag").GetComponent<Text>();

        //Fetch Toggle GameObject
        Variables.fullPath_Toggle = GameObject.FindGameObjectWithTag("PathToggle").GetComponent<Toggle>();
        Variables.line_Toggle = GameObject.FindGameObjectWithTag("LineToggle").GetComponent<Toggle>();
        Variables.markerIndex_Toggle = GameObject.FindGameObjectWithTag("MarkerToggle").GetComponent<Toggle>();
        Variables.drawOneByOnePosition_Toggle = GameObject.FindGameObjectWithTag("DrawOneByOnePosition").GetComponent<Toggle>();
        Variables.morePaths_Toggle = GameObject.FindGameObjectWithTag("MorePathsToggle").GetComponent<Toggle>();

        Variables.releaseAndCaptureCoordinatesList = new Dictionary<string, string[]>();
    }

    public static void SetInputFields()
    {
        //Set InputFields
        if (File.Exists(Variables.SetupFile))
        {
            SpawnOnMap.readFromFileData = ReadFiles.ReadSetupFile(Variables.SetupFile);

            Variables.daysBetweenTag_Field.text = SpawnOnMap.readFromFileData[1];
            Variables.numberOfFish_Field.text = SpawnOnMap.readFromFileData[2];
            Variables.temperatureDelta_Field.text = SpawnOnMap.readFromFileData[3];
            Variables.depthDelta_Field.text = SpawnOnMap.readFromFileData[4];
            Variables.fishLength_Field.text = SpawnOnMap.readFromFileData[5];
            Variables.pathWeight_Field.text = SpawnOnMap.readFromFileData[6];
            Variables.possiblePaths_Field.text = SpawnOnMap.readFromFileData[7];
        }
        else
        {
            Variables.daysBetweenTag_Field.text = "0";
            Variables.numberOfFish_Field.text = "0";
            Variables.temperatureDelta_Field.text = "0";
            Variables.depthDelta_Field.text = "0";
            Variables.fishLength_Field.text = "0";
            Variables.pathWeight_Field.text = "0";
            Variables.possiblePaths_Field.text = "0";
        }
    }

    public static void LineSetup(GameObject gameObject)
    {
        // Add a Line Renderer to the GameObject
        Variables.line = gameObject.AddComponent<LineRenderer>();

        // Set the width of the Line Renderer
        Variables.line.startWidth = 0.8F;
        Variables.line.endWidth = 0.8F;
        Variables.line.startColor = Color.white;
        Variables.line.endColor = Color.white;
        Material whiteDiffuseMat = new Material(Shader.Find("Unlit/Texture"));
        Variables.line.material = whiteDiffuseMat;
    }
}
