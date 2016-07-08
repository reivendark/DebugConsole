using UnityEngine;

namespace DebugConsole
{
    public class DebugConsoleGUI_Standalone : MonoBehaviour
    {
        private const int MAX_LENGTH = 200;
        private const float TEXT_HEIGHT = 20;
        private const float TEXT_LEFT_MARGIN = 10;

        private const string DEFAULT_CONSOLE_TEXT = "> ";

        public string commandLine;

        private Rect m_Rect;
        private GUIStyle m_Style;

        public static DebugConsoleGUI_Standalone Instance { get; private set; }

        public static void CreateConsoleGUI()
        {
            if(Instance != null)
                return;

            var go = new GameObject("$DebugConsole");
            DontDestroyOnLoad(go);
            go.AddComponent<DebugConsoleGUI_Standalone>();
        }

        public void Awake()
        {
            Instance = this;
            float top = Screen.height - 20;
            float width = Screen.width - 20;
            m_Rect = new Rect(TEXT_LEFT_MARGIN, top, width, TEXT_HEIGHT);

            m_Style = new GUIStyle
            {
                richText = false,
                active =
                {
                    textColor = Color.yellow,
                    background = Texture2D.blackTexture
                },
                normal =
                {
                    textColor = Color.yellow,
                    background = Texture2D.blackTexture
                }
            };

            EraseString();
        }

        public void OnGUI()
        {
            commandLine = GUI.TextField(m_Rect, commandLine, MAX_LENGTH, m_Style);
            DebugConsoleCore.CheckString_Standalone(commandLine, EraseString);
        }

        private void EraseString()
        {
            commandLine = DEFAULT_CONSOLE_TEXT;
        }
    }
}