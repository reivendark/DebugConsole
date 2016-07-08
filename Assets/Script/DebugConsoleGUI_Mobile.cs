using JetBrains.Annotations;
using UnityEngine;

namespace DebugConsole
{
    public class DebugConsoleGUI_Mobile : MonoBehaviour
    {
        private const float TEXT_HEIGHT = 20;
        private const float TEXT_LEFT_MARGIN = 10;

        private const string DEFAULT_CONSOLE_TEXT = "";

        public string commandLine;

        private Rect m_Rect;

        private TouchScreenKeyboard m_Keyboard;

        public static DebugConsoleGUI_Mobile Instance { get; private set; }

        public static void CreateConsoleGUI()
        {
            if(Instance != null)
                return;

            var go = new GameObject("$DebugConsole");
            DontDestroyOnLoad(go);
            go.AddComponent<DebugConsoleGUI_Mobile>();
        }

        [UsedImplicitly]
        private void Awake()
        {
            Instance = this;
            float top = Screen.height - 40;
            float width = Screen.width - 20;
            m_Rect = new Rect(TEXT_LEFT_MARGIN, top, width, TEXT_HEIGHT);

            EraseString();
        }

        [UsedImplicitly]
        private void OnGUI()
        {
            if(GUI.Button(m_Rect, "Console"))
            {
                m_Keyboard = TouchScreenKeyboard.Open(commandLine, TouchScreenKeyboardType.Default, false, false, false);
            }
        }

        [UsedImplicitly]
        private void Update()
        {
            if(m_Keyboard != null && m_Keyboard.done)
            {
                commandLine = m_Keyboard.text;
                m_Keyboard = null;
                DebugConsoleCore.CheckString_Mobile(commandLine, EraseString);
            }
        }

        private void EraseString()
        {
            commandLine = DEFAULT_CONSOLE_TEXT;
        }
    }
}