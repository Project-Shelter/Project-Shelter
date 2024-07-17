using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using System.Text.RegularExpressions;
using ItemContainer;

public class DataManager : MonoSingleton<DataManager>
{
    public Dictionary<int, T> JsonToDict<T>(string filePath) where T : DBData
    {
        Dictionary<int, T> returnDict = new Dictionary<int, T>();
        string jsonData = File.ReadAllText(Application.dataPath + filePath);
        jsonData =  jsonData.Substring(1, jsonData.Length - 2);
        List<string> jsonList = ExtractJson(jsonData);
        
        Debug.Log(jsonData);

        foreach (var jsonString in jsonList)
        {
            T data = JsonConvert.DeserializeObject<T>(jsonString);
            returnDict.Add(data.id, data);
        }
        
        return returnDict;
    }

    public void DictToJson<T>(Dictionary<int, T> dict, string filePath)
    {
        
    }
    
    //문자열에서 json 문자열을 추출해 list에 저장
    private List<string> ExtractJson(string input)
    {
        List<string> jsonList = new List<string>();
        string pattern =  @"\{[^}]*\}";
        MatchCollection matches = Regex.Matches(input, pattern);

        Debug.Log(matches.Count);

        foreach (Match match in matches)
        {
            jsonList.Add(match.Value);
        }

        return jsonList;
    }
}
