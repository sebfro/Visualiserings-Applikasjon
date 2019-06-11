using Mapbox.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Variables {

    public static GameObject PreviousClickedMarker;
    public static Dictionary<int, FishRouteInfo> FishRouteInfoDictionary = new Dictionary<int, FishRouteInfo>();
    public static GameObject GameObjectFromButton = null;
    public static bool AddedListener = false;
    public static int Key = 1;

    public static string[] releaseAndCapture = new string[4];

    public static List<GameObject> _spawnedObjects;

    public static LineRenderer line;

    public static Button changeParameters_Button;
    public static Button run_Button;
    public static Button PreviousMarker_Button;
    public static Button NextMarker_Button;
    public static Button savePath_Button;

    public static Dropdown acceptable_Dropdown;
    public static Dropdown unacceptable_Dropdown;
    public static Dropdown chooseTag_Dropdown;
    public static Dropdown chooseAlgorithm_Dropdown;
    public static Dropdown saved_Dropdown;

    public static GameObject changeParameters_Panel;

    public static InputField daysBetweenTag_Field;
    public static InputField numberOfFish_Field;
    public static InputField temperatureDelta_Field;
    public static InputField depthDelta_Field;
    public static InputField fishLength_Field;
    public static InputField pathWeight_Field;
    public static InputField possiblePaths_Field;

    public static List<Vector2d> _positionList;

    public static Text FishTag_Text;
    public static Text MarkerClicked_Text;
    public static Text Date_Text;
    public static Text TagDataTemp_Text;
    public static Text SeaTemp_Text;
    public static Text DifferenceTemp_Text;
    public static Text TagDataDepth_Text;
    public static Text SeaDepth_Text;
    public static Text DifferenceDepth_Text;

    public static Text PreviousToNewPosition_Text;
    public static Text ThisToNextPosition_Text;
    public static Text PreviousToNewSpeed_Text;
    public static Text ThisToNextSpeed_Text;

    public static Toggle fullPath_Toggle;
    public static Toggle line_Toggle;
    public static Toggle markerIndex_Toggle;
    public static Toggle drawOneByOnePosition_Toggle;
    public static Toggle morePaths_Toggle;

    public static Dictionary<string, string[]> releaseAndCaptureCoordinatesList;

    public static string FishTag = "742";
    public static string PreviousSavedPath = "";


    public static string DirectoryPath = @"C:\NCdata\fishData\";
    public static string InvalidPathDirectory = DirectoryPath + FishTag + @"\Uakseptabel";
    public static string ValidPathDirectory = DirectoryPath + FishTag + @"\Akseptabel";
    public static string SavedPathDirectory = DirectoryPath + FishTag + @"\Saved";
    public static string SetupFile = DirectoryPath + @"\setup.txt";
    public static string AvailableTagsFile = DirectoryPath + @"\availableTags.txt";

    public static bool state;
    public static bool lineSetup;
    public static bool drawLines;
    public static bool missingTagFile;
    public static bool savedFile;


}
