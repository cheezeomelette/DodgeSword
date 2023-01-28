using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ObjectTime 
{
    public static float timeScale = 1f;
    public static float deltaTime => Time.deltaTime * timeScale;
}
