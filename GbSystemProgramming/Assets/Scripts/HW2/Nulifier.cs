using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

public struct Nulifier : IJob
{
    public NativeArray<int> m_array;

    public void Execute()
    {
        Debug.Log($"{ArrayToSingleString(m_array)} is input array");

        for (int i = 0; i < m_array.Length; i++)
        {
            if (m_array[i] > 10) m_array[i] = 0;
        }

        Debug.Log($"{ArrayToSingleString(m_array)} is output array");
    }

    private string ArrayToSingleString(NativeArray<int> nativeArray)
    {
        string temp = "";
        for (int i = 0;i < nativeArray.Length; i++) temp += nativeArray[i].ToString() + " ";
        return temp;
    }
}
