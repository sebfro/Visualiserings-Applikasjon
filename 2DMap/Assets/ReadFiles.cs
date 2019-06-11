using Mapbox.Examples;
using Mapbox.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;

public class ReadFiles {

    public static List<string> ReadFishRoutesFromFileAndGenerateDropDownOptions(string fishRouteFilePath)
    {
        string[] fishRouteFilePaths = Directory.GetFiles(fishRouteFilePath);
        List<string> dropOptions = new List<string>();

        if (fishRouteFilePaths.Length == 1)
        {
            dropOptions.Add(string.Format("{0} path", fishRouteFilePaths.Length));
        }
        else
        {
            dropOptions.Add(string.Format("{0} paths", fishRouteFilePaths.Length));
        }


        foreach (string s in fishRouteFilePaths)
        {
            string[] strArray;
            strArray = s.Split('\\');
            dropOptions.Add(strArray[5]);
        }
        
        return dropOptions;
    }

    public static List<string> ReadAvailableFishIdFromTextFile(string AvailableIdFile, bool setupExists)
    {
        List<string> dropOptions = new List<string>();
        string[] lines = File.ReadAllLines(AvailableIdFile);
        Variables.releaseAndCaptureCoordinatesList = new Dictionary<string, string[]>();

        if (setupExists) { dropOptions.Add(SpawnOnMap.readFromFileData[0]); }

        foreach (string line in lines)
        {
            string[] strArray;
            strArray = line.Split('\t');

            if (setupExists)
            {
                if (SpawnOnMap.readFromFileData[0].Equals(strArray[0]))
                {
                    Variables.releaseAndCapture = new string[4];
                    Variables.releaseAndCapture[0] = strArray[1];
                    Variables.releaseAndCapture[1] = strArray[2];
                    Variables.releaseAndCapture[2] = strArray[3];
                    Variables.releaseAndCapture[3] = strArray[4];
                    Variables.releaseAndCaptureCoordinatesList.Add(strArray[0], Variables.releaseAndCapture);
                }

                if (!SpawnOnMap.readFromFileData[0].Equals(strArray[0]) && !strArray[0].Equals("ID"))
                {
                    dropOptions.Add(strArray[0]);
                    Variables.releaseAndCapture = new string[4];
                    Variables.releaseAndCapture[0] = strArray[1];
                    Variables.releaseAndCapture[1] = strArray[2];
                    Variables.releaseAndCapture[2] = strArray[3];
                    Variables.releaseAndCapture[3] = strArray[4];

                    Variables.releaseAndCaptureCoordinatesList.Add(strArray[0], Variables.releaseAndCapture);
                    Debug.Log(strArray[0]);

                }
            }
            else
            {
                if (!strArray[0].Equals("ID"))
                {
                    Variables.releaseAndCapture = new string[4];


                    dropOptions.Add(strArray[0]);
                    Variables.releaseAndCapture[0] = strArray[1];
                    Variables.releaseAndCapture[1] = strArray[2];
                    Variables.releaseAndCapture[2] = strArray[3];
                    Variables.releaseAndCapture[3] = strArray[4];

                    Variables.releaseAndCaptureCoordinatesList.Add(strArray[0], Variables.releaseAndCapture);

                }
            }
        }
        return dropOptions;
    }

    public static string[] ReadSetupFile(string setupFile)
    {
        StreamReader file = new StreamReader(setupFile);
        string lineFromFile;
        int count = 0;
        var culture = CultureInfo.InvariantCulture;
        string[] strArray = new string[7];

        while ((lineFromFile = file.ReadLine()) != null)
        {
            if (count > 0)
            {
                strArray = lineFromFile.Split('\t');
            }
            count++;
        }
        file.Close();

        return strArray;
    }

    public static void getFishRouteFromFile(string DirPath)
    {

        if (!Variables.morePaths_Toggle.isOn)
        {
            Variables._positionList = new List<Vector2d>();
            
        }

        
        StreamReader file = new StreamReader(DirPath);
        
        string lineFromFile;
        int count = 0;
        var culture = CultureInfo.InvariantCulture;
        Variables.FishRouteInfoDictionary = new Dictionary<int, FishRouteInfo>();


        while ((lineFromFile = file.ReadLine()) != null)
        {
            if (count > 0)
            {
                string[] strArray;
                strArray = lineFromFile.Split('\t');
                strArray[0] = strArray[0].Replace(",", ".");
                strArray[1] = strArray[1].Replace(",", ".");
                strArray[2] = strArray[2].Replace(",", ".");
                strArray[3] = strArray[3].Replace(",", ".");
                strArray[4] = strArray[4].Replace(",", ".");
                strArray[5] = strArray[5].Replace(",", ".");

                Variables._positionList.Add(new Vector2d(double.Parse(strArray[0], culture), double.Parse(strArray[1], culture)));

                
                Variables.FishRouteInfoDictionary.Add(count, new FishRouteInfo(Math.Round(double.Parse(strArray[2], culture), 0), Math.Round(double.Parse(strArray[3], culture), 0), 
                    Math.Round(double.Parse(strArray[4], culture), 1), Math.Round(double.Parse(strArray[5], culture), 1), double.Parse(strArray[0], culture), double.Parse(strArray[1], culture), strArray[6]));
            }
            count++;
        }
        file.Close();
    }
}
