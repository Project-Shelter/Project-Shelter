using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Utils;

public class DataManager : MonoSingleton<DataManager>
{
    private GoogleSheetReader sheetReader = null;
    void Awake()
    {
        #if  UNITY_EDITOR
        sheetReader = new GoogleSheetReader();
        SaveSheet();
#endif
    }

    //sheet의 정보를 로컬 json파일로 저장한다
    public void SaveSheet()
    {
        sheetReader.UpdateJson("1XH1ztMeNetul76bCy8OJpx4JiQBNnk_S","ItemTable_Data!B2:J30","Assets/Data/ItemTable.json");
    }

    //json 파일을 읽어와 Dictionary로 만든다
    public void ReadJson()
    {
        
    }
    
    //Dictionary를 json으로 만든다
    public void WriteJson()
    {
        
    }
}
