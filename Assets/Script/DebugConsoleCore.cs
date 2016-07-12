using System;
using System.Collections.Generic;
using System.Linq;
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
            //TryMakeCall(parsedCommands);
            TryMakeNamedCall(parsedCommands);
        }

        private static void TryMakeNamedCall(List<string> parsedCommands)
        {
            try
            {
                var assembly = Assembly.GetExecutingAssembly();

                Type classType = null;

                classType = assembly.GetType("DebugConsole.DebugConsoleScenarios");

                if(parsedCommands.Count == 0 || parsedCommands[0] == "/?")
                {
                    // print all commands
                    foreach (var nestedClass in classType.GetNestedTypes())
                    {
                        Debug.Log(nestedClass.Name);
                    }
                    foreach (var method in classType.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic))
                    {
                        Debug.Log(method.Name);
                    }
                    return;
                }

                string className = parsedCommands[0];
                var type = assembly.GetType("DebugConsole.DebugConsoleScenarios+" + className);

                if(type != null)
                {
                    classType = type;
                    parsedCommands.RemoveAt(0);

                    if(parsedCommands.Count == 0 || parsedCommands[0] == "/?")
                    {
                        // print all subcommands
                        var helpParams = new object[] { classType.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic) };
                        var helpMehod = classType.GetMethod("getHelp", BindingFlags.NonPublic | BindingFlags.Static);
                        helpMehod.Invoke(null, helpParams);
                        return;
                    }
                }

                string methodName = parsedCommands[0];
                parsedCommands.RemoveAt(0);
                var methodInfo = classType.GetMethod(methodName, BindingFlags.Public | BindingFlags.Static | BindingFlags.NonPublic);
                var parameters = methodInfo.GetParameters();

                if(parsedCommands.Count == 1 && parsedCommands[0] == "/?")
                {
                    // print all subcommand params

                    string logString = "'"+  methodInfo.Name + "' params: ";
                    logString = parameters.Aggregate(logString, (current, parameterInfo) => current + (parameterInfo.Name + ", "));
                    Debug.Log(logString);
                    return;
                }
                
                object[] objects = new object[parameters.Length];
                
                if(parameters.Length >= parsedCommands.Count)
                {
                    for(int i = 0; i < objects.Length; i++)
                    {
                        if(i < parsedCommands.Count)
                        {
                            Type t = parameters[i].ParameterType;
                            objects[i] = Convert.ChangeType(parsedCommands[i], t);
                        }
                        else
                        {
                            objects[i] = parameters[i].DefaultValue;
                        }
                    }
                    methodInfo.Invoke(null, objects);
                }
                else
                {
                    throw new ArgumentOutOfRangeException("You pass more params that needed");
                }
            }
            catch (ArgumentException e)
            {
                Debug.LogError(e.Message);
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
            }
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
        
                object[] objects = new object[parameters.Length];
        
                if(parameters.Length >= parsedCommands.Count)
                {
                    for(int i = 0; i < objects.Length; i++)
                    {
                        if(i < parsedCommands.Count)
                        {
                            Type t = parameters[i].ParameterType;
                            objects[i] = Convert.ChangeType(parsedCommands[i], t);
                        }
                        else
                        {
                            objects[i] = parameters[i].DefaultValue;
                        }
                    }
                    methodInfo.Invoke(null, objects);
                }
                else
                {
                    throw new ArgumentOutOfRangeException("You pass more params that needed");
                }
            }
            catch (ArgumentException e)
            {
                Debug.LogError(e.Message);
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
            }
        }
    }
}