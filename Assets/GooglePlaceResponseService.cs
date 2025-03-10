using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;
using Cysharp.Threading.Tasks;

public class GooglePlaceResponseService
{
    //シングルトン
    //遅延初期化でGooglePlaceResponseServiceが必要な時にinstanceを生成するようにしてる
    private static readonly Lazy<GooglePlaceResponseService> _instance = new Lazy<GooglePlaceResponseService>(CreateInstance);
    //まだGooglePlaceResponseServiceのinstanceが生成されてなければ生成する
    public static GooglePlaceResponseService Instance => _instance.Value;
    //このクラスのみでGooglePlaceApiを生成
    private readonly GooglePlaceApi _googlePlaceApi;
    //Lazy<T>が初回に生成する際に呼び出されるメソッド
    private GooglePlaceResponseService()
    {
        _googlePlaceApi = new GooglePlaceApi();
    }

    private static GooglePlaceResponseService CreateInstance()
    {
        return new GooglePlaceResponseService();
    }

    //場所の情報の全てを取得
    public async UniTask<GooglePlaceEntities> GetAllPlaceInfo(CancellationToken cancellationToken)
    {
        try
        {
            GooglePlaceEntities response = await _googlePlaceApi.AllPlaceInfo(cancellationToken);
            return response;

        }
        catch (Exception ex)
        {
            Debug.LogError("GetUserInfo Error: " + ex.Message);
            throw;
        }
    }
}
