using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]public class UnityEventFlag:UnityEvent<bool>{}
[System.Serializable]public class UnityEventInt:UnityEvent<int>{}
[System.Serializable]public class UnityEventFloat:UnityEvent<float>{}
[System.Serializable]public class UnityEventVector3:UnityEvent<Vector3>{}
[System.Serializable]public class UnityEventGameObject:UnityEvent<GameObject>{}

public delegate void RebootCallback();