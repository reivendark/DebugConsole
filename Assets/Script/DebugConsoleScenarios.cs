using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace DebugConsole
{
    public static class DebugConsoleScenarios
    {
        public static void help()
        {
            
        }

        public static class create
        {
            private static void helicopter(byte team, bool isAh1)
            {
                var go = GameObject.CreatePrimitive(PrimitiveType.Cube);
                go.transform.position = Vector3.zero;
                Debug.Log("Team: " + team);
                Debug.Log("Is AH1: " + isAh1);
            }

            private static void getHelp(MethodInfo[] methods)
            {
                foreach (var methodInfo in methods)
                {
                    Debug.Log(methodInfo.Name);
                }
            }
        }
    }
}