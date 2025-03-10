using System;
using System.Text;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.Networking;
using System.Threading;

public class GooglePlaceEntities
{
    public int Id { get; set; }
    public float lat { get; set; }
    public float lng { get; set; }
    public string icon { get; set; }
    public string name { get; set; }
    public bool open_now { get; set; }
    public string photos { get; set; }
    public string place_id { get; set; }
    public string plus_code { get; set; }
    public int price_level { get; set; }
    public float rating { get; set; }
    public int user_ratings_total { get; set; }
    public string vicinity { get; set; }
    public string types { get; set; }
}

public class GooglePlaceApi
{
    //場所の情報を全て取得する
    public async UniTask<GooglePlaceEntities> AllPlaceInfo(CancellationToken cancellationToken)
    {
        return await ClientBasicApi.sendGetRequestAsync<GooglePlaceEntities>("/places/meal-info", cancellationToken);
    }
}
