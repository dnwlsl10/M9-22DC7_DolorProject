using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;
using System.IO;
public class DataManager
{
    private static DataManager instance;
    public Dictionary<int, RobotData> dicRobotDatas = new Dictionary<int, RobotData>();

    public static DataManager GetInstance()
    {
        if (DataManager.instance == null)
        {
            DataManager.instance = new DataManager();
        }

        return DataManager.instance;
    }

    public void LoadDatas()
    {
        
        var json = Resources.Load<TextAsset>("Datas/robot_data").text;
        if (json.Length <= 0) throw new System.Exception("JSONLENGTH");
        var arrItemDatas = JsonConvert.DeserializeObject<RobotData[]>(json);
        
        foreach (var data in arrItemDatas)
        {
            Debug.LogFormat("{0}, {1}, {2}", data.id, data.name, data.desc);
        }
        this.dicRobotDatas = arrItemDatas.ToDictionary(x => x.id);
    }
}

