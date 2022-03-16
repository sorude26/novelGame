using System;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataLoadController : MonoBehaviour
{
    Entity_Sheet1 _es = default;
    List<StoryCode> _storyCode = default;
    TextControl _texts;
    ActorControl _actors;
    BackgroundControl _background;
    public int StoryCount { get => _storyCode.Count; }
   
    class StoryCode
    {
        public int _id;
        public int _type;
        public string _code;
        public StoryCode(int id,int type, string code)
        {
            _id = id;
            _type = type;
            _code = code;
        }
    }
    public void SetControl(TextControl text,ActorControl actors,BackgroundControl background)
    {
        _texts = text;
        _actors = actors;
        _background = background;
    }
    private void Awake()
    {
        LoadData();
    }
    void LoadData()
    {
        _storyCode = new List<StoryCode>();
        _es = Resources.Load("storyText") as Entity_Sheet1;
        foreach (var item in _es.sheets[0].list)
        {
            _storyCode.Add(new StoryCode(item.id, item.type, item.code));
        }
    }
    public IEnumerator GetEvent(int id)
    {
        var x = _storyCode.Where(s => s._id == id).FirstOrDefault();
        if (x == null)
        {
            return null;
        }
        return GetTypeAsync(x._code);
    }
    IEnumerator GetTypeAsync(string codeText)
    {
        string code = default;
        List<string> actions = new List<string>();
        bool end = false;
        int count = 0;
        int actionCount = 0;
        while (count < codeText.Length)
        {
            if (codeText[count] == '/')
            {
                if (end)
                {
                    actionCount++;
                }
                end = true;
                actions.Add("");
            }
            else
            {
                if (!end)
                {
                    code += codeText[count];
                }
                else
                {
                    actions[actionCount] += codeText[count];
                }
            }
            count++;
        }
        switch (code)
        {
            case "any":
                return WaitAnyAsync(GetActionAsyncs(actions));
            case "all":
                return WaitAllAsync(GetActionAsyncs(actions));
            default:
                break;
        }
        return null;
    }
    IEnumerator[] GetActionAsyncs(IEnumerable<string> asyncCode) 
    {
        List<IEnumerator> enumerators = new List<IEnumerator>();
        foreach (var code in asyncCode)
        {
            enumerators.Add(GetSelectAsyncs(code));
        }
        return enumerators.ToArray();
    }
    IEnumerator GetSelectAsyncs(string code)
    {
        int count = 0;
        string type = default;
        string asyncCode = default;
        for (int i = 0; i < code.Length; i++)
        {
            if (code[i] == '%')
            {
                count++;
                continue;
            }
            if (count == 0)
            {
                type += code[i];
            }
            else if (count == 1)
            {
                asyncCode += code[i];
            }
        }
        switch (type)
        {
            case "text":
                return _texts.ViewText(asyncCode);
            case "actor":
                return GetAcotorAsync(asyncCode);
            case "back":
                return GetBackgroundAsync(asyncCode);
            case "wait":
                return null;
            default:
                break;
        }
        return null;
    }
    IEnumerator GetAcotorAsync(string code)
    {
        int count = 0;
        string type = default;
        string actor = default;
        string target = default;
        string time = default;
        for (int i = 0; i < code.Length; i++)
        {
            if (code[i] == '#')
            {
                count++;
                continue;
            }
            if (count == 0)
            {
                type += code[i];
            }
            else if (count == 1)
            {
                actor += code[i];
            }
            else if (count == 2)
            {
                target += code[i];
            }
            else if (count == 3)
            {
                time += code[i];
            }
        }
        switch (type)
        {
            case "fadeIn":
                return _actors.SelectActor(int.Parse(actor)).FadeIn(float.Parse(time));
            case "fadeOut":
                return _actors.SelectActor(int.Parse(actor)).FadeOut(float.Parse(time));
            case "move":
                return _actors.SelectActorMove(int.Parse(actor), int.Parse(target), float.Parse(time));
            default:
                break;
        }
        return null;
    }
    IEnumerator GetBackgroundAsync(string code)
    {
        int count = 0;
        string type = default;
        string target = default;
        string time = default;
        for (int i = 0; i < code.Length; i++)
        {
            if (code[i] == '#')
            {
                count++;
                continue;
            }
            if (count == 0)
            {
                type += code[i];
            }
            else if (count == 1)
            {
                target += code[i];
            }
            else if (count == 2)
            {
                time += code[i];
            }
        }
        switch (type)
        {
            case "fade":
                return _background.FadeChangeBackground(float.Parse(time), int.Parse(target));
            case "cross":
                return _background.CrossFadeChange(float.Parse(time), int.Parse(target));
            default:
                break;
        }
        return null;
    }
    IEnumerator WaitAllAsync(IEnumerator[] asyncs, Action[] actions = null)
    {
        var canceler = new Canceler(asyncs.Length);
        for (int i = 0; i < asyncs.Length; i++)
        {
            StartCoroutine(Await(asyncs[i], canceler.Cancel));
        }
        while (!canceler.IsCanceld)
        {
            yield return null;
        }
        if (actions != null)
        {
            foreach (var action in actions)
            {
                action?.Invoke();
            }
        }
    }
    IEnumerator Await(IEnumerator async, Action action)
    {
        yield return async;
        action?.Invoke();
    }
    IEnumerator WaitAnyAsync(IEnumerator[] asyncs, Action[] actions = null)
    {
        var canceler = new Canceler(0);
        for (int i = 0; i < asyncs.Length; i++)
        {
            StartCoroutine(Await(asyncs[i], canceler.Cancel));
        }
        while (!canceler.IsCanceld)
        {
            yield return null;
        }
        if (actions != null)
        {
            foreach (var action in actions)
            {
                action?.Invoke();
            }
        }
    }
}
