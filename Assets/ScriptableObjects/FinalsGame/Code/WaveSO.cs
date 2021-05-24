using System;
using RotaryHeart.Lib.SerializableDictionary;
using UnityEngine;

[CreateAssetMenu(fileName = "New Wave", menuName = "SO/Waves")]
public class WaveSO : ScriptableObject
{
    [Serializable]
    public class WaveDictionary : SerializableDictionaryBase<EnemySO, int> {}
    
    public WaveDictionary waveSettings;
    public float delayBetweenSpawn;
    public float waveDuration;
}
