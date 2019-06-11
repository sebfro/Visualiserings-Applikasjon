using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishRouteInfo {
    public double tagDataDepth { get; set; }
    public  double seaDepth { get; set; }
    public double tagDataTemp { get; set; }
    public double seaTemp { get; set; }
    public double latitude { get; set; }
    public double longitude { get; set; }
    public string date { get; set; }

    public   FishRouteInfo(double tagDataDepth, double seaDepth, double tagDataTemp, double seaTemp, double latitude, double longitude, string date)
    {
        this.tagDataDepth = tagDataDepth;
        this.seaDepth = seaDepth;
        this.tagDataTemp = tagDataTemp;
        this.seaTemp = seaTemp;
        this.latitude = latitude;
        this.date = date;

    }
}
