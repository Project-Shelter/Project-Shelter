using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using System.Text.RegularExpressions;
using ItemContainer;

public class DataManager : MonoSingleton<DataManager>
{
    private const string TablePattern = @"\[[^]]*\]";
    private const string DataPattern = @"\{[^}]*\}";
    public Dictionary<int, T>[] JsonToDict<T>(string filePath) where T : DBData
    {
        string jsonData = File.ReadAllText(Application.dataPath + filePath);
        jsonData =  jsonData.Substring(1, jsonData.Length - 2);
        List<string> tableList = ExtractString(jsonData, TablePattern);
        Dictionary<int, T>[] returnDict = new Dictionary<int, T>[tableList.Count];
            
        int index = 0;
        foreach (var tableString in tableList)
        {
            returnDict[index] = new Dictionary<int, T>();
            List<string> jsonList = ExtractString(tableString, DataPattern);
            foreach (var jsonString in jsonList) {
                T data = JsonConvert.DeserializeObject<T>(jsonString);
                returnDict[index].Add(data.ID, data);
            }
            index++;
        }
        
        return returnDict;
    }

    public void DictToJson<T>(Dictionary<int, T> dict, string filePath)
    {
        
    }
    
    //문자열에서 json 문자열을 추출해 list에 저장
    private List<string> ExtractString(string input, string pattern)
    {
        List<string> jsonList = new List<string>();
        MatchCollection matches = Regex.Matches(input, pattern);

        Debug.Log(matches.Count);

        foreach (Match match in matches)
        {
            jsonList.Add(match.Value);
        }

        return jsonList;
    }
}
