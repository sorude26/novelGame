using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    IEnumerator _coru = default;
    IEnumerator _coru2 = default;
    void Start()
    {
        _coru = RotateAsync();
    }

    // Update is called once per frame
    void Update()
    {
        if (_coru == null) { return; }
        if (!TestCoru2(_coru))
        {
            //_coru = null;
        }       
    }
    
    bool TestCoru2(IEnumerator enumerator)
    {
        var a = (IEnumerator)enumerator.Current;
        if (a != null && InEnumerator(a))
        {
            return true;
        }
        return enumerator.MoveNext();
    }
    bool InEnumerator(IEnumerator enumerator)
    {
        var b = (IEnumerator)enumerator.Current;
        if (b != null)
        {
            return InEnumerator(b);
        }
        else
        {
            return enumerator.MoveNext();
        }
    }
    IEnumerator RotateAsync()
    {
        yield return RotateAxisAsync(180, Vector3.right);
        yield return RotateAxisAsync(180, Vector3.up);
        yield return RotateAxisAsync(180, Vector3.forward);
    }
    IEnumerator RotateAxisAsync(int count, Vector3 axis)
    {
        for (int i = 0; i < count; i++)
        {
            transform.Rotate(axis);
            yield return null;
        }
        yield return WaitSecAsync(1);
    }
    private IEnumerator WaitSecAsync(float sec)
    {
        for (var t = 0F; t < sec; t += Time.deltaTime)
        {
            yield return null;
        }
        yield return WaitSecAsync2(1);
        Debug.Log("a");
    }
    private IEnumerator WaitSecAsync2(float sec)
    {
        for (var t = 0F; t < sec; t += Time.deltaTime)
        {
            yield return null;
        }
        Debug.Log("b");
    }
}
