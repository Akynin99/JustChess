#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace CasualStrategy.Tools
{
    public class LoadSceneWindow : EditorWindow
    {
        private readonly string[] _sceneNames = new[]
        {
            "Initial",
        };
        
        private const int ButtonWidth = 175; //width of a common button
        private GUILayoutOption _bw = GUILayout.Width(ButtonWidth);
        private LoadSceneNamesMethod _loadSceneNamesMethod;

        private enum LoadSceneNamesMethod
        {
            FromBuildSettings = 0,
            FromArray = 1,
        }

        [MenuItem("Tools/Open Load Scene Window")]
        private static void ShowWindow()
        {
            GetWindow<LoadSceneWindow>(false, "Load Scenes", true);
        }

        private void OnGUI()
        {
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();

            _loadSceneNamesMethod = (LoadSceneNamesMethod)EditorGUILayout.EnumPopup("Load scenes from", _loadSceneNamesMethod);
            
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();

            switch (_loadSceneNamesMethod)
            {
                case LoadSceneNamesMethod.FromBuildSettings:
                    ShowScenesFromBuildSettings();
                    break;
                
                case LoadSceneNamesMethod.FromArray:
                    ShowScenesFromArray();
                    break;
            }
        }

        private void ShowScenesFromArray()
        {
            foreach (var sceneName in _sceneNames)
            {
                ShowLoadSceneButton(sceneName);
                EditorGUILayout.Space();
            }
        }
        
        private void ShowScenesFromBuildSettings()
        {
            EditorBuildSettingsScene[] scenes = EditorBuildSettings.scenes;
            
            foreach (var scene in scenes)
            {
                string scenePath = scene.path;
                ShowLoadSceneButtonFromPath(scenePath);
                EditorGUILayout.Space();
            }
        }

        private void ShowLoadSceneButton(string sceneName)
        {
            CenterBegin();
            if (GUILayout.Button("Load " + sceneName, _bw)) OpenSceneFromScenesFolder(sceneName);
            CenterEnd();
        }
        
        private void ShowLoadSceneButtonFromPath(string scenePath)
        {
            string sceneName = scenePath.Replace(".unity", "");

            int slashIdx = -1;
            char slash = '/';
            
            for (int i = 0; i < sceneName.Length; i++)
            {
                if (sceneName[i] == slash) slashIdx = i;
            }

            if (slashIdx >= 0) sceneName = sceneName.Substring(slashIdx + 1);
            
            CenterBegin();
            if (GUILayout.Button("Load " + sceneName, _bw)) OpenSceneFromPath(scenePath);
            CenterEnd();
        }

        private void CenterBegin()
        {
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
        }

        private void CenterEnd()
        {
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }

        private void OpenSceneFromScenesFolder(string sceneName)
        {
            EditorSceneManager.OpenScene("Assets/Scenes/" + sceneName + ".unity");
        }
        
        private void OpenSceneFromPath(string sceneName)
        {
            EditorSceneManager.OpenScene(sceneName);
        }
    }
}
#endif