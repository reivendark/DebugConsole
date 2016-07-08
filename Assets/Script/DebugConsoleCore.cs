using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace DebugConsole
{
    public static class DebugConsoleCore
    {
        public static void Initialize()
        {
#if(UNITY_STANDALONE)
            DebugConsoleGUI_Standalone.CreateConsoleGUI();
#elif (UNITY_ANDROID || UNITY_IOS || UNITY_WP_8 || UNITY_WP_8_1)
            DebugConsoleGUI_Mobile.CreateConsoleGUI();
#endif
        }

        public static void CheckString_Standalone(string command, Action onCommandReadyAction)
        {
            for(int i = 0; i < command.Length; i++)
            {
                if (command[i] != 13 && command[i] != 10) continue;
                onCommandReadyAction.Invoke();

                TryParseCommand(command);
                return;
            }
        }

        public static void CheckString_Mobile(string command, Action onCommandReadyAction)
        {
            onCommandReadyAction.Invoke();
            TryParseCommand(command);
        }

        private static void TryParseCommand(string commandLine)
        {
            var parsedCommands = DebugConsoleParser.Parse(commandLine);
            TryMakeCall(parsedCommands);
        }

        private static void TryMakeCall(List<string> parsedCommands)
        {
            try
            {
                var assembly = Assembly.GetExecutingAssembly();

                string className = parsedCommands[0];
                parsedCommands.RemoveAt(0);
                var classType = assembly.GetType("DebugConsole.DebugConsoleScenarios+" + className);

                string methodName = parsedCommands[0];
                parsedCommands.RemoveAt(0);
                var methodInfo = classType.GetMethod(methodName, BindingFlags.Static | BindingFlags.NonPublic);

                var parameters = methodInfo.GetParameters();

                object[] objects = new object[parsedCommands.Count];
                if(parameters.Length > parsedCommands.Count)
                {
                    for(int i = 0; i < parsedCommands.Count; i++)
                    {
                        Type t = parameters[i].ParameterType;
                        objects[i] = Convert.ChangeType(parsedCommands[i], t);
                    }
                    methodInfo.Invoke(null, objects);
                }
                else
                {
                    throw new ArgumentOutOfRangeException("Мы передали больше параметров чем есть у метода");
                }
            }
            catch (Exception)
            {
                
                throw;
            }
        }
    }
}