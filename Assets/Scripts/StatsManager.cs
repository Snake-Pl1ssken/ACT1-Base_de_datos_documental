using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using System;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Security.Policy;

public class StatsManager : MonoBehaviour
{    

    public struct Stat
    {
        public float time;
        public float speed;

    };
    
    public string serverAddress = "mongodb://localhost:27017";
    public string database = "racer";
    public string collection = "races";

    Dictionary<int, Stat> gateToStat;
    BsonElement e;
    MongoClient connection;
    BsonDocument doc;
    static IMongoDatabase databaseMongo;
    static IMongoCollection<BsonDocument> collectionMongo;


    // Start is called before the first frame update
    void Start()
    {
        gateToStat = new Dictionary<int, Stat>();
        

        // Connect to database

        connection = new MongoClient(serverAddress);
        databaseMongo = connection.GetDatabase(database);
        collectionMongo = databaseMongo.GetCollection<BsonDocument>(collection);

        if (connection != null)
        {
            Debug.Log("Database conected");
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnRaceStarted()
    {
        // Create race and register start date in database
        doc = new BsonDocument();
        e = new BsonElement("StartTime", new BsonDateTime(DateTime.Now));
        doc.Add(e);
        collectionMongo.InsertOne(doc);
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
        doc = new BsonDocument();
        e = new BsonElement("FinishTime", new BsonDateTime(DateTime.Now));
        doc.Add(e);
        collectionMongo.InsertOne(doc);

        Stat stats = new Stat();

        //_id: ObjectId,
        //startTime: Date,

        //gates: [ {  gate: Int32,
        //            time: (double)stats.time,
        //            speed: (double)stats.speed
        //            },
        //            {
        //                gate: Int32,
        //                time: (double)stats.time,
        //                speed: (double)stats.speed
        //            },
        //            {
        //                gate: Int32,
        //                time: (double)stats.time,
        //                speed: (double)stats.speed
        //            },
        //            {
        //                gate: Int32,
        //                time: (double)stats.time,
        //                speed: (double)stats.speed
        //            }
        //       ]

    }


    public int GetStats(Stat[] stats)
    {
        // Get race stats from database

        int count = 0;

        foreach (int key in gateToStat.Keys)
        {
            Debug.Log("Getting gate " + key);
            stats[key] = gateToStat[key];
            count++;
        }

        return count;
    }

    public void ClearStats()
    {
        gateToStat.Clear();
    }
}
