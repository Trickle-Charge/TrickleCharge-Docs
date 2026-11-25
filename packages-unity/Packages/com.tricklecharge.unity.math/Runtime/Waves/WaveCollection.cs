using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace TrickleCharge.Math.Waves
{
[Serializable]
public class WaveCollection : IList<IWave>, IWave
{
    [SerializeField]
    private bool m_enabled = true;

    [SerializeReference]
    private List<IWave> m_waves = new();

    public bool Enabled
    {
        get => m_enabled;
        set => m_enabled = value;
    }

    public List<IWave> Waves => m_waves;

    /// <inheritdoc />
    public float Evaluate(float time, float position = 0)
    {
        if(! Enabled) { return 0; }

        float sum = 0;
        foreach(IWave wave in this)
        {
            if(wave == null) { continue; }
            sum  += wave.Evaluate(time, position);
        }

        return sum;
    }

    /// <inheritdoc />
    public IEnumerator<IWave> GetEnumerator() => Waves.GetEnumerator();

    /// <inheritdoc />
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    /// <inheritdoc />
    public void Add(IWave item) => Waves.Add(item);

    /// <inheritdoc />
    public void Clear() => Waves.Clear();

    /// <inheritdoc />
    public bool Contains(IWave item) => Waves.Contains(item);

    /// <inheritdoc />
    public void CopyTo(IWave[] array, int arrayIndex) => Waves.CopyTo(array, arrayIndex);

    /// <inheritdoc />
    public bool Remove(IWave item) => Waves.Remove(item);

    /// <inheritdoc />
    public int Count => Waves.Count;

    /// <inheritdoc />
    public bool IsReadOnly => false;

    /// <inheritdoc />
    public int IndexOf(IWave item) => Waves.IndexOf(item);

    /// <inheritdoc />
    public void Insert(int index, IWave item) => Waves.Insert(index, item);

    /// <inheritdoc />
    public void RemoveAt(int index) => Waves.RemoveAt(index);

    /// <inheritdoc />
    public IWave this[int index]
    {
        get => Waves[index];
        set => Waves[index] = value;
    }
}
}
