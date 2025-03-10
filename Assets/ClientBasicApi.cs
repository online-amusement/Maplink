using System;
using System.Text;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.Networking;
using System.Threading;

public class ClientBasicApi
{
    private static readonly string _baseUrl = "http://local.navi/api";

    //postリクエストの共通処理
    public static async UniTask<T> SendPostRequestAsync<T>(string endpoint, string jsonData, CancellationToken cancellationToken)
    {
        string url = $"{ _baseUrl }{ endpoint }";
        using (UnityWebRequest request = new UnityWebRequest(url, "POST"))
        {
            byte[] body = Encoding.UTF8.GetBytes(jsonData);
            //サーバーに送信するデータを扱う
            request.uploadHandler = new UploadHandlerRaw(body);
            //サーバーから受信するデータを扱う
            request.downloadHandler = new DownloadHandlerBuffer();
            //リクエストヘッダを設定
            request.SetRequestHeader("content-Type", "application/json");
            //requestを投げて待機
            await request.SendWebRequest().ToUniTask(cancellationToken: cancellationToken);
            //次のフレームに処理を譲る
            await UniTask.Yield(cancellationToken);

            if (request.result == UnityWebRequest.Result.Success)
            {
                string responseText = request.downloadHandler.text;

                if (typeof(T) == typeof(string))
                {
                    return (T)(object)responseText;
                }
                else
                {
                    T result = JsonConvert.DeserializeObject<T>(responseText);
                    return result;
                }
            }
            else
            {
                Debug.LogError($"POST Request Failed: {request.error}");
                throw new Exception(request.error);
            }
        }
    }

    //getリクエストの共通処理
    public static async UniTask<T> sendGetRequestAsync<T>(string endpoint, CancellationToken cancellationToken)
    {
        string url = $"{ _baseUrl }{ endpoint }";
        Debug.Log(url);
        using (UnityWebRequest request = new UnityWebRequest(url, "GET"))
        {
            //requestを投げて待機
            await request.SendWebRequest().ToUniTask(cancellationToken: cancellationToken);
            //次のフレームに処理を譲る
            await UniTask.Yield(cancellationToken);

            if (request.result == UnityWebRequest.Result.Success)
            {
                string responseText = request.downloadHandler.text;

                if (typeof(T) == typeof(string))
                {
                    return (T)(object)responseText;
                }
                else
                {
                    T result = JsonConvert.DeserializeObject<T>(responseText);
                    return result;
                }
            }
            else
            {
                Debug.LogError($"GET Request Failed: {request.error}");
                throw new Exception(request.error);
            }
        }
    }
}
