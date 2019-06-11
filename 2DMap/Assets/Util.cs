using Mapbox.Examples;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Util
{
    

    public static void EnableDisableMenu()
    {
        Variables.state = !Variables.state;
        Variables.changeParameters_Panel.SetActive(Variables.state);
    }

    public static void ClearDropdownWhenDirectoryDoesNotExist()
    {
        Variables.acceptable_Dropdown.ClearOptions();
        Variables.unacceptable_Dropdown.ClearOptions();
        Variables.saved_Dropdown.ClearOptions();

        List<string> option = new List<string>();
        option.Add("0 paths");

        Variables.acceptable_Dropdown.AddOptions(option);
        Variables.unacceptable_Dropdown.AddOptions(option);
        Variables.saved_Dropdown.AddOptions(option);
    }

    public static void ChooseTagAndChooseAlgorithmSetup()
    {
        Variables.chooseTag_Dropdown.ClearOptions();
        Variables.chooseAlgorithm_Dropdown.ClearOptions();


        if (File.Exists(Variables.AvailableTagsFile))
        {
            Variables.chooseTag_Dropdown.AddOptions(ReadFiles.ReadAvailableFishIdFromTextFile(Variables.AvailableTagsFile, File.Exists(Variables.SetupFile)));
        }
        else
        {
            List<string> noTagFile = new List<string>();
            noTagFile.Add("No tag file");
            Variables.chooseTag_Dropdown.AddOptions(noTagFile);
            Variables.missingTagFile = true;
        }

        List<string> algorithms = new List<string>();
        algorithms.Add("General");
        algorithms.Add("Merge");
        Variables.chooseAlgorithm_Dropdown.AddOptions(algorithms);
    }

    public static void DeletePreviousPath()
    {
        GameObject[] markers = GameObject.FindGameObjectsWithTag("marker");
        foreach (GameObject marker in markers)
        {
            GameObject.Destroy(marker);
        }
    }

    public static void DrawLines(GameObject gameObject)
    {
        if (!Variables.lineSetup)
        {
            Setup.LineSetup(gameObject);
            Variables.lineSetup = true;
        }

        Variables.line.positionCount = Variables._positionList.Count;

        SpawnOnMap._waypoints = new GameObject[Variables._spawnedObjects.Count];

        for (int j = 0; j < Variables._spawnedObjects.Count; j++)
        {
            SpawnOnMap._waypoints[j] = Variables._spawnedObjects[j];
        }
    }

    public static void RemoveLines()
    {
        Variables.lineSetup = false;
        GameObject.Destroy(Variables.line);
    }

    public static void ToggleLine(bool value, GameObject gameObject)
    {
        if (value)
        {
            DrawLines(gameObject);
            Variables.drawLines = true;
        }
        else
        {
            RemoveLines();
            Variables.drawLines = false;
        }
    }

    public static void UpdateDirPaths()
    {
        Variables.FishTag = Variables.chooseTag_Dropdown.options[Variables.chooseTag_Dropdown.value].text;

        Variables.InvalidPathDirectory = Variables.DirectoryPath + Variables.FishTag + @"\Uakseptabel";
        Variables.ValidPathDirectory = Variables.DirectoryPath + Variables.FishTag + @"\Akseptabel";
        Variables.SavedPathDirectory = Variables.DirectoryPath + Variables.FishTag + @"\Saved";

    }

    public static void ToggleMarkerIndex(bool value)
    {
        Color color;

        if (value)
        {
            color = Color.white;
        }
        else
        {
            color = Color.clear;
        }

        var markers = GameObject.FindGameObjectsWithTag("marker");
        foreach (var markerIndex in markers)
        {
            markerIndex.GetComponentInChildren<TextMesh>().color = color;
        }
    }

    public static void SavePath(string DirPath)
    {
        try
        {
            int fCount = Directory.GetFiles(Variables.SavedPathDirectory, "*", SearchOption.TopDirectoryOnly).Length + 1;
            File.Copy(DirPath, Variables.SavedPathDirectory + @"\" + Variables.FishTag + "_" + fCount + ".txt", false);

            Variables.drawOneByOnePosition_Toggle.isOn = false;
            Variables.PreviousSavedPath = DirPath;


            Variables.savedFile = true;
            Debug.Log(Variables.SavedPathDirectory + " " + Variables.FishTag + " " + + fCount);
        } catch(DirectoryNotFoundException dirEx)
        {
            Debug.Log("Run algorithm on this fish tag");
        }
        
    }

    public static void MorePaths(bool value)
    {
        if (value)
        {
            Debug.Log("Choose more");
        }
    }

    public static bool IsOdd(int value)
    {
        return value % 2 != 0;
    }

    
}

