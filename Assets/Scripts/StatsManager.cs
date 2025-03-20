using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using System;
//using MongoDB.Driver;

public class StatsManager : MonoBehaviour
{    

    public struct Stat
    {
        public float time;
        public float speed;

    };
    
    public string serverAddress = "192.168.113.25:27017";
    public string database = "racer";
    public string collection = "races";

    Dictionary<int, Stat> gateToStat;

    //MongoClient connection;

    // Start is called before the first frame update
    void Start()
    {
        gateToStat = new Dictionary<int, Stat>();

        // Connect to database
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnRaceStarted()
    {
        // Create race and register start date in database
    }

    public void OnGatePassed(int index, float time, float speed)
    {
        Debug.Log("Registering gate " + index);

        Stat s = new Stat();
        s.time = time;
        s.speed = speed;

        gateToStat[index] = s;

        // Register gate in database

    }

    public void OnRaceFinished()
    {
        Debug.Log("Race finished");

    }


    public int GetStats(Stat[] stats)
    {
        // Get race stats from database

        int count = 0;

        foreach(int key in gateToStat.Keys)
        {
            Debug.Log("Getting gate " + key);
            stats[key] = gateToStat[key];
            count ++;
        }

        return count;
    }

    public void ClearStats()
    {
        gateToStat.Clear();
    }
}
