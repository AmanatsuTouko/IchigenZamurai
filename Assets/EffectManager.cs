using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    public static EffectManager Instance;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);

            // オブジェクトプールの生成
            InitObjectPool();
        }
        else
        {
            Destroy(this);   
        }
    }

    public List<GameObject> EffectsVFX;

    // オブジェクトプールの個数
    [SerializeField] int _effectPoolCount = 15;

    // オブジェクトプールで指定したエフェクトを再生できるようにする
    public enum EffectType
    {
        Slash,         // 通常の牌を斬ったとき
        IncorrectSlash // 斬ってはいけない牌を斬ったとき
    }

    // 各オブジェクトプールにある各々のオブジェクトを管理する
    public List<List<Effect>> _effects = new List<List<Effect>>();
    // 各エフェクトプールの次の再生予定のエフェクトのインデックス
    List<int> _nextEffectIdx = new List<int>();

    // 指定したエフェクトを指定した場所で再生する
    public void PlayEffect(EffectType effectType, Vector3 position)
    {
        int i = (int)effectType;

        // 使えるエフェクトを探す
        int invalidCount = 0;
        while(true)
        {
            if(_effects[i][_nextEffectIdx[i]].CanPlay())
            {
                break;
            }

            // 全てのエフェクトが再生中の時は何もしない
            invalidCount += 1;
            if(invalidCount >= _effectPoolCount)
            {
                break;
            }

            // インクリメント
            _nextEffectIdx[i] += 1;
            if(_nextEffectIdx[i] >= _effectPoolCount)
            {
                _nextEffectIdx[i] = 0;
            }
        }
        // 座標の指定
        _effects[i][_nextEffectIdx[i]].transform.position = position;  
        // エフェクトの再生
        _effects[i][_nextEffectIdx[i]].Play();
    }

    // オブジェクトプールの初期化
    private void InitObjectPool()
    {
        for(int i=0; i<EffectsVFX.Count; i++)
        {
            _nextEffectIdx.Add(0);
            _effects.Add(new List<Effect>());

            for(int j=0; j<_effectPoolCount; j++)
            {
                GameObject gameObject = Instantiate(EffectsVFX[i]);
                gameObject.transform.SetParent(this.transform);
                gameObject.transform.position = new Vector3(0, -10, 0);
                Effect effect = gameObject.GetComponent<Effect>();
                _effects[i].Add(effect);
            }
        }   
    }
}
