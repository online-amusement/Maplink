using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class PlaneDetection : MonoBehaviour
{
    // 配置する車のプレハブ。Inspectorで必ず割り当てること
    [SerializeField]
    private GameObject carPrefab;

    // AR空間でのレイキャストを管理するコンポーネント
    [SerializeField]
    private ARRaycastManager arRaycastManager;

    // レイキャストのヒット結果
    private List<ARRaycastHit> hitResults = new List<ARRaycastHit>();

    // 既に配置された車の参照（なければnull）
    private GameObject placedCar;

    private float initialDistance;

    private Vector3 initialScale;

    void Awake()
    {
        // Inspectorで割り当てられていない場合、子オブジェクトから取得する
        if (arRaycastManager == null)
        {
            arRaycastManager = GetComponentInChildren<ARRaycastManager>();
            if (arRaycastManager == null)
            {
                Debug.LogError("ARRaycastManagerが見つかりません。");
            }
        }
    }

    void Update()
    {
        // タッチ入力があるか確認
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            // タッチ開始時に処理を行う
            if (touch.phase == TouchPhase.Began)
            {
                // 既に車が配置されている場合は何もしない
                if (placedCar != null)
                {
                    Debug.Log("車は既に配置されています。");
                    return;
                }

                // タッチ位置から平面のヒットを試みる
                if (arRaycastManager.Raycast(touch.position, hitResults, TrackableType.PlaneWithinPolygon))
                {
                    // ヒットした最初の結果を利用
                    Pose hitPose = hitResults[0].pose;
                    Debug.Log("ヒットした位置: " + hitPose.position);

                    // carPrefabが正しく割り当てられていれば、生成する
                    if (carPrefab != null)
                    {
                        placedCar = Instantiate(carPrefab, hitPose.position, hitPose.rotation);
                    }
                    else
                    {
                        Debug.LogError("carPrefabが割り当てられていません。");
                    }
                }
                else
                {
                    Debug.Log("タッチ位置に平面が検出されませんでした。");
                }
            }
        }
        else if(Input.touchCount == 2 && placedCar != null)
        {
            Debug.Log("2本指");
            Touch touchCount1 = Input.GetTouch(0);
            Touch touchCount2 = Input.GetTouch(1);
            //２本指の状態が移動してたら
            if (touchCount1.phase == TouchPhase.Moved && touchCount2.phase == TouchPhase.Moved)
            {
                //2本の指の距離を測定
                float currentDistance = Vector2.Distance(touchCount1.position, touchCount2.position);

                if (initialDistance == 0)
                {
                    initialDistance = currentDistance;
                    initialScale = placedCar.transform.localScale;
                }
                else
                {
                    float scaleFactor = currentDistance / initialDistance;
                    placedCar.transform.localScale = initialScale * scaleFactor;
                }
            }
            if (touchCount1.phase == TouchPhase.Ended && touchCount2.phase == TouchPhase.Ended)
            {
                initialDistance = 0;
            }
        }
    }
}
