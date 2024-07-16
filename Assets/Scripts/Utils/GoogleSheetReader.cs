using Google.Apis.Auth.OAuth2;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using Newtonsoft.Json;
using Unity.VisualScripting.FullSerializer;
using UnityEditor;
using UnityEngine;

namespace Utils
{
    public class GoogleSheetReader
    {
        private static readonly string[] Scopes = { SheetsService.Scope.Spreadsheets };
        private const string CredentialPath = "/Secret/client_credential.json";
        private readonly SheetsService _sheetsService;

        public GoogleSheetReader()
        {
            //Setting Connection
            string jsonPath = Application.dataPath + CredentialPath;
            Debug.Log(jsonPath);
            
            GoogleCredential credential;
            using (var stream = new FileStream(jsonPath, FileMode.Open, FileAccess.Read))
            {
                credential = GoogleCredential.FromStream(stream).CreateScoped(Scopes);
            }
            
            _sheetsService = new SheetsService(new Google.Apis.Services.BaseClientService.Initializer
            {
                HttpClientInitializer = credential,
                ApplicationName = "Project Shelter"
            });
        }

        public void UpdateJson(string sheetId, string range, string path)
        {
            SpreadsheetsResource.ValuesResource.GetRequest request =
                _sheetsService.Spreadsheets.Values.Get(sheetId, range);

            ValueRange response = request.Execute();
            IList<IList<object>> values = response.Values;

            //시트 데이터를 json 문자열로 변환
            List<Dictionary<string, object>> data = new List<Dictionary<string, object>>();
            if (values != null && values.Count > 0)
            {
                var headers = (List<object>)values[0];
                for (int i = 1; i < values.Count; i++)
                {
                    var row = (List<object>)values[i];
                    var rowData = new Dictionary<string, object>();
                    for (int j = 0; j < headers.Count; j++)
                    {
                        rowData[(string)headers[j]] = row[j];
                    }

                    data.Add(rowData);
                }
            }

            string json = JsonConvert.SerializeObject(data, Formatting.Indented);
            Debug.Log(json);
            //SaveJson(json, path);
        }

        private void SaveJson(string json, string filePath)
        {
            try
            {
                File.WriteAllText(filePath, json);
            }
            catch (IOException ex)
            {
                Debug.Log($"@ERROR@ json 저장에 실패했습니다. : {ex.Message}");
            }
        }
    }
}