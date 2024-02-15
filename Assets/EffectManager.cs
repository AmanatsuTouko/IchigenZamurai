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
        Slash // 通常の牌を斬ったとき
    }

    // 各オブジェクトプールにある各々のオブジェクトを管理する
    public List<Effect> _effects;
    // 
    int _nextEffectIdx = 0;

    // 指定したエフェクトを指定した場所で再生する
    public void PlayEffect(EffectType effectType, Vector3 position)
    {
        // 使えるエフェクトを探す
        int invalidCount = 0;
        while(true)
        {
            if(_effects[_nextEffectIdx].CanPlay())
            {
                break;
            }

            // 全てのエフェクトが再生中の時は何もしない
            invalidCount += 1;
            if(invalidCount >= _effects.Count)
            {
                break;
            }

            // インクリメント
            _nextEffectIdx += 1;
            if(_nextEffectIdx >= _effects.Count)
            {
                _nextEffectIdx = 0;
            }
        }
        // 座標の指定
        _effects[_nextEffectIdx].transform.position = position;  
        // エフェクトの再生
        _effects[_nextEffectIdx].Play();
    }

    private void InitObjectPool()
    {
         for(int j=0; j<EffectsVFX.Count; j++)
        {
            // オブジェクトプールを生成する
            for(int i=0; i<_effectPoolCount; i++)
            {
                GameObject gameObject = Instantiate(EffectsVFX[j]);
                gameObject.transform.SetParent(this.transform);
                Effect effect = gameObject.GetComponent<Effect>();
                _effects.Add(effect);
            }
        }   
    }
}
