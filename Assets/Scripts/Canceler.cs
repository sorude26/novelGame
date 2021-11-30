using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// キャンセルの通達を行うクラス
/// </summary>
public class Canceler 
{
    public class Notifier
    {
        public bool IsCanceld { get => m_owner.IsCanceld; }
        Canceler m_owner;
        public Notifier(Canceler owner)
        {
            m_owner = owner;
        }
    }
    private int allCanceldCount = 0;
    public int CurrentCanceldCount { get; private set; }
    public bool IsCanceld { get; private set; }
    public Notifier CancelerNotifier { get; }
    public Canceler()
    {
        CancelerNotifier = new Notifier(this);
    }
    public Canceler(int count)
    {
        allCanceldCount = count - 1;
        CancelerNotifier = new Notifier(this);
    }
    public void Cancel()
    {
        if (CurrentCanceldCount < allCanceldCount)
        {
            CurrentCanceldCount++;
            return;
        }
        IsCanceld = true;
    }
}

