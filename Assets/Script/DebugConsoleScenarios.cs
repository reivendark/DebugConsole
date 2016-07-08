using UnityEngine;

namespace DebugConsole
{
    public static class DebugConsoleScenarios
    {
        public static class create
        {
            private static void cube(int i1, int i2, string str1 = "hello", float f1 = 0.9f)
            {
                var go = GameObject.CreatePrimitive(PrimitiveType.Cube);
                go.transform.position = Vector3.zero;
                Debug.Log(i1);
                Debug.Log(i2);
                Debug.Log(str1);
                Debug.Log(f1);
            }
        }
    }
}