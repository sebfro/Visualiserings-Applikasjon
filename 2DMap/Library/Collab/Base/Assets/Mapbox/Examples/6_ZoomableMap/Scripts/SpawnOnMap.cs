using System;
using System.IO;
using UnityEngine.UI;

namespace Mapbox.Examples
{
    using UnityEngine;
    using Mapbox.Utils;
    using Mapbox.Unity.Map;
    using Mapbox.Unity.MeshGeneration.Factories;
    using Mapbox.Unity.Utilities;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using UnityEngine.EventSystems;
    using UnityEngine.SceneManagement;
    using System.Threading;
    using System.Collections;

    public class SpawnOnMap : MonoBehaviour
    {

        [SerializeField]
        AbstractMap _map;

        [SerializeField]
        [Geocode]
        //string[] _locationStrings;
        string[] _positionFish;
        Vector2d[] _locations;

        [SerializeField]
        float _spawnScale = 100f;

        [SerializeField]
        GameObject _markerPrefab;

        //[SerializeField]
        //GameObject _fish;

        public Vector3 fishPrevPosition;

        // Line Renderer

        [SerializeField]
        public static GameObject[] _waypoints;

        int num = 0;
        float minDist = 1;
        float speed = 5;
        bool rand = false;
        bool go = true;
        bool upToDate;
        bool lineSetup;
        public static string[] readFromFileData;
        bool noFiles;
        bool changedPath;

        string[] ValidDir = Directory.GetFiles(Variables.ValidPathDirectory);
        string[] InvalidDir = Directory.GetFiles(Variables.InvalidPathDirectory);

        string pathToDirectory;

        void Start()
        {

            Setup.FetchGameObjects();

            Setup.SetInputFields();

            Util.ChooseTagAndChooseAlgorithmSetup();

            Util.UpdateDirPaths();

            RefreshPathOptionsInDropdown();

            Variables.FishTag_Text.text = "Fish Tag: " + Variables.chooseTag_Dropdown.options[Variables.chooseTag_Dropdown.value].text;

            //Listen to Button click
            Variables.changeParameters_Button.onClick.AddListener(Util.EnableDisableMenu);

            if (Variables.missingTagFile)
            {
                Debug.Log("Need tag file. Locate and restart application");
            }
            else
            {
                Variables.run_Button.onClick.AddListener(RunAlgorithmWithNewParameters);
            }


            //Hide the Menu
            Variables.changeParameters_Panel.SetActive(Variables.state);

            AddListeners();


            if (ValidDir.Length != 0 && Variables.fullPath_Toggle.isOn)
            {
                pathToDirectory = ValidDir[0];
                GenerateFishRoute(pathToDirectory, false);

            }
            else if (InvalidDir.Length != 0 && Variables.fullPath_Toggle.isOn)
            {
                pathToDirectory = InvalidDir[0];
                GenerateFishRoute(pathToDirectory, false);
            }
            else
            {
                GenerateFishRoute(null, true);
                Debug.Log("Only showing release and capture");
            }

        }
        
        void GenerateFishRoute(string DirPath, bool runReleaseAndCaptureBool)
        {
            if (!Variables.missingTagFile)
            {

                StopCoroutine("DrawOneByOneMarker");

                Debug.Log("Running fish tag: " + Variables.FishTag);

                Variables.AddedListener = false;

                if (!upToDate)
                {
                    RefreshPathOptionsInDropdown();
                }

                Util.DeletePreviousPath();

                if (!runReleaseAndCaptureBool && File.Exists(DirPath))
                {
                    ReadFiles.getFishRouteFromFile(DirPath);
                }
                else
                {
                    string[] releaseAndCapture = Variables.releaseAndCaptureCoordinatesList[Variables.FishTag];

                    Debug.Log(Variables.FishTag);

                    Debug.Log(Variables.releaseAndCaptureCoordinatesList[Variables.FishTag][0]);


                    Variables._positionList = new List<Vector2d>
                {
                    new Vector2d(double.Parse(releaseAndCapture[0].Replace(",", ".")), double.Parse(releaseAndCapture[1].Replace(",", "."))),
                    new Vector2d(double.Parse(releaseAndCapture[2].Replace(",", ".")), double.Parse(releaseAndCapture[3].Replace(",", ".")))
                };

                }

                if (Variables.drawOneByOnePosition_Toggle.isOn)
                {
                    StartCoroutine("DrawOneByOneMarker");
                }
                else
                {
                    DrawMarkers();
                }

                if (Variables.line_Toggle.isOn)
                {
                    Util.DrawLines(gameObject);
                    Variables.drawLines = true;
                }
                else
                {
                    Util.RemoveLines();
                    Variables.drawLines = false;
                }

            }


        }

        private void Update()
        {
            if (!Variables.missingTagFile)
            {

                int count;

                count = Variables._spawnedObjects.Count;


                int j = 1;


                Variables.FishTag_Text.text = "Fish tag: " + Variables.chooseTag_Dropdown.options[Variables.chooseTag_Dropdown.value].text;
                //_fish.transform.localPosition = _map.GeoToWorldPosition(_locations[0], true);
                //_fish.transform.localScale = new Vector3 (_spawnScale, _spawnScale, _spawnScale);

                for (int i = 0; i < count; i++)
                {
                    if (GameObject.FindGameObjectsWithTag("marker").Length != 0)
                    {
                        var spawnedObject = Variables._spawnedObjects[i];
                        var location = Variables._positionList[i];
                        spawnedObject.transform.localPosition = _map.GeoToWorldPosition(location, true);
                        spawnedObject.transform.localScale = new Vector3(_spawnScale, _spawnScale, _spawnScale);


                        if (i + 1 < count && Variables.drawLines)
                        {
                            // Update position of the two vertex of the Line Renderer
                            Variables.line.SetPosition(i, Variables._spawnedObjects[i].transform.position + new Vector3(0, 1, 0));
                            Variables.line.SetPosition(i + 1, Variables._spawnedObjects[i + 1].transform.position + new Vector3(0, 1, 0));
                        }

                        j++;
                    }

                }
            }

        }

        void AddListeners()
        {
            Variables.fullPath_Toggle.onValueChanged.AddListener((value) =>
            {
                ToggleFullPath(value);
            }
            );

            Variables.line_Toggle.onValueChanged.AddListener((value) =>
            {
                Util.ToggleLine(value, gameObject);
            });

            Variables.markerIndex_Toggle.onValueChanged.AddListener((value) =>
            {
                Util.ToggleMarkerIndex(value);
            });

            Variables.drawOneByOnePosition_Toggle.onValueChanged.AddListener((value) =>
            {
                Variables.line_Toggle.isOn = false;
                if (!Variables.fullPath_Toggle.isOn)
                {
                    GenerateFishRoute(pathToDirectory, true);
                }
                else
                {
                    GenerateFishRoute(pathToDirectory, false);
                }
            });

            Variables.chooseTag_Dropdown.onValueChanged.AddListener(delegate
            {
                FishTagChanged();
            });

            Variables.acceptable_Dropdown.onValueChanged.AddListener(delegate
            {
                DropdownValueChanged(Variables.acceptable_Dropdown, Variables.ValidPathDirectory);
            });

            Variables.unacceptable_Dropdown.onValueChanged.AddListener(delegate
            {
                DropdownValueChanged(Variables.unacceptable_Dropdown, Variables.InvalidPathDirectory);
            });
        }

        void FishTagChanged()
        {
            Util.UpdateDirPaths();

            try
            {
                ValidDir = Directory.GetFiles(Variables.ValidPathDirectory);
                InvalidDir = Directory.GetFiles(Variables.InvalidPathDirectory);
                upToDate = false;

                if (ValidDir.Length != 0 && Variables.fullPath_Toggle.isOn)
                {
                    pathToDirectory = ValidDir[0];
                    GenerateFishRoute(pathToDirectory, false);

                }
                else if (InvalidDir.Length != 0 && Variables.fullPath_Toggle.isOn)
                {
                    pathToDirectory = InvalidDir[0];
                    GenerateFishRoute(pathToDirectory, false);
                }
                else
                {
                    GenerateFishRoute(null, true);
                    Debug.Log("Only showing release and capture");
                }
            }
            catch (DirectoryNotFoundException ex)
            {
                Debug.Log("Run algorithm on this fish tag");

                if (GameObject.FindGameObjectsWithTag("marker").Length > 2)
                {
                    Util.DeletePreviousPath();
                }

                Util.ClearDropdownWhenDirectoryDoesNotExist();
            }
        }

        void DrawMarkers()
        {
            Color color;
            _spawnScale = 8;
            _locations = new Vector2d[Variables._positionList.Count];
            Variables._spawnedObjects = new List<GameObject>();
            float offset = 0.0f;
            for (int i = 0; i < Variables._positionList.Count; i++)
            {
                var locationString = Variables._positionList[i];
                var instance = Instantiate(_markerPrefab);
                instance.transform.localPosition = _map.GeoToWorldPosition(Variables._positionList[i], true);
                instance.transform.localScale = new Vector3(_spawnScale, _spawnScale, _spawnScale);
                instance.gameObject.GetComponentInChildren<TextMesh>().text = (i + 1).ToString();
                if (Variables.markerIndex_Toggle.isOn)
                {
                    color = Color.white;
                }
                else
                {
                    color = Color.clear;
                }
                offset = Util.IsOdd(i) ? -2.0f : 2.0f;
                instance.gameObject.GetComponentInChildren<TextMesh>().transform.position += new Vector3(offset, 0.5f, 0);


                if (Variables._positionList.Count != 2)
                {
                    instance.AddComponent<OnClick>();
                    instance.AddComponent<BoxCollider>();
                }

                instance.gameObject.GetComponentInChildren<TextMesh>().color = color;
                instance.tag = "marker";
                Variables._spawnedObjects.Add(instance);

            }
        }

        IEnumerator DrawOneByOneMarker()
        {

            Color color;
            _spawnScale = 8;
            _locations = new Vector2d[Variables._positionList.Count];
            Variables._spawnedObjects = new List<GameObject>();
            float offset = 0.0f;
            for (int i = 0; i < Variables._positionList.Count; i++)
            {
                var locationString = Variables._positionList[i];
                var instance = Instantiate(_markerPrefab);
                instance.transform.localPosition = _map.GeoToWorldPosition(Variables._positionList[i], true);
                instance.transform.localScale = new Vector3(_spawnScale, _spawnScale, _spawnScale);
                instance.gameObject.GetComponentInChildren<TextMesh>().text = (i + 1).ToString();
                if (Variables.markerIndex_Toggle.isOn)
                {
                    color = Color.white;
                }
                else
                {
                    color = Color.clear;
                }
                offset = Util.IsOdd(i) ? -2.0f : 2.0f;
                instance.gameObject.GetComponentInChildren<TextMesh>().transform.position += new Vector3(offset, 0.5f, 0);


                if (Variables._positionList.Count != 2)
                {
                    instance.AddComponent<OnClick>();
                    instance.AddComponent<BoxCollider>();
                    //instance.AddComponent<Renderer>();
                }

                instance.gameObject.GetComponentInChildren<TextMesh>().color = color;
                instance.tag = "marker";
                Variables._spawnedObjects.Add(instance);

                yield return new WaitForSeconds(0.05f);
            }
        }

        void ToggleFullPath(bool value)
        {
            Util.UpdateDirPaths();

            if (Variables.line_Toggle.isOn)
            {
                Variables.line_Toggle.isOn = false;
            }

            try
            {
                ValidDir = Directory.GetFiles(Variables.ValidPathDirectory);
                InvalidDir = Directory.GetFiles(Variables.InvalidPathDirectory);

                if (value)
                {
                    if (pathToDirectory == null)
                    {
                        if (ValidDir.Length != 0)
                        {
                            pathToDirectory = ValidDir[0];
                            GenerateFishRoute(pathToDirectory, false);
                        }
                        else if (InvalidDir.Length != 0)
                        {
                            pathToDirectory = InvalidDir[0];
                            GenerateFishRoute(pathToDirectory, false);
                        }
                        else
                        {
                            GenerateFishRoute(null, true);
                        }
                    }
                    else
                    {
                        GenerateFishRoute(pathToDirectory, false);
                    }
                }
                else
                {
                    GenerateFishRoute(null, true);
                }
            }
            catch (DirectoryNotFoundException)
            {
                Debug.Log("Run algorithm on this fish tag");
            }

        }

        void DropdownValueChanged(Dropdown change, string DirPath)
        {
            if (change.value != 0)
            {
                if (Variables.line_Toggle.isOn)
                {
                    Variables.line_Toggle.isOn = false;
                }

                changedPath = true;

                pathToDirectory = DirPath + "\\" + change.options[change.value].text;

                Debug.Log("Her");
                GenerateFishRoute(pathToDirectory, false);
            }
        }

        void RefreshPathOptionsInDropdown()
        {
            //Clear the old options of the Dropdown menu
            Variables.acceptable_Dropdown.ClearOptions();
            Variables.unacceptable_Dropdown.ClearOptions();

            //Add the options created in the method called below
            Variables.acceptable_Dropdown.AddOptions(ReadFiles.ReadFishRoutesFromFileAndGenerateDropDownOptions(Variables.ValidPathDirectory));
            Variables.unacceptable_Dropdown.AddOptions(ReadFiles.ReadFishRoutesFromFileAndGenerateDropDownOptions(Variables.InvalidPathDirectory));

            upToDate = true;
        }

        void RunAlgorithmWithNewParameters()
        {
            int numb;
            double number;

            if (!double.TryParse(Variables.daysBetweenTag_Field.text, out number))
            {
                Debug.Log("Days must be double");
            }
            else if (!double.TryParse(Variables.fishLength_Field.text, out number))
            {
                Debug.Log("Fish length must be double");
            }
            else if (!int.TryParse(Variables.numberOfFish_Field.text, out numb))
            {
                Debug.Log("Number of simulations must be integer");
            }
            else if (!double.TryParse(Variables.temperatureDelta_Field.text, out number))
            {
                Debug.Log("The temperature delta must be double");
            }
            else if (!int.TryParse(Variables.depthDelta_Field.text, out numb))
            {
                Debug.Log("The depth delta must be integer");
            }
            else if (!double.TryParse(Variables.pathWeight_Field.text, out number))
            {
                Debug.Log("The weighting of path must be double");
            }
            else if (!int.TryParse(Variables.possiblePaths_Field.text, out numb))
            {
                Debug.Log("Possible paths must be integer");
            }
            else
            {
                System.Diagnostics.Process mProcess = new System.Diagnostics.Process();
                mProcess.StartInfo.FileName = @"C:\Users\Torbastian\Documents\GitHub\SDSLiteVS2017\SpagettiMetoden\bin\x64\Debug\SpagettiMetoden.exe";
                mProcess.StartInfo.Arguments = Variables.chooseTag_Dropdown.options[Variables.chooseTag_Dropdown.value].text + " " + Variables.daysBetweenTag_Field.text + " "
                    + Variables.numberOfFish_Field.text + " " + Variables.temperatureDelta_Field.text + " " + Variables.depthDelta_Field.text + " "
                    + Variables.fishLength_Field.text + " " + Variables.pathWeight_Field.text + " " + Variables.possiblePaths_Field.text + " " + Variables.chooseAlgorithm_Dropdown.value;
                Debug.Log(Variables.possiblePaths_Field.text);
                mProcess.Start();
                mProcess.WaitForExit();

                ValidDir = Directory.GetFiles(Variables.ValidPathDirectory);
                InvalidDir = Directory.GetFiles(Variables.InvalidPathDirectory);

                upToDate = false;
                noFiles = false;

                if (ValidDir.Length != 0)
                {
                    pathToDirectory = ValidDir[0];
                    GenerateFishRoute(pathToDirectory, false);
                    Variables.state = !Variables.state;
                    Variables.changeParameters_Panel.SetActive(Variables.state);
                }
                else if (InvalidDir.Length != 0)
                {
                    pathToDirectory = InvalidDir[0];
                    GenerateFishRoute(pathToDirectory, false);
                    Variables.state = !Variables.state;
                    Variables.changeParameters_Panel.SetActive(Variables.state);
                }
                else
                {
                    noFiles = true;
                    Debug.Log("Did not find any paths with the parameters");
                }
            }
        }
    }
}