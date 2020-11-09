using UnityEngine;
using System.IO;

public class InitProject : MonoBehaviour
{
    [SerializeField] bool m_createGitReady = false;

    void Start()
    {
        #region ----- Character
        CreateDir(@"Character\#Prefabs");
        #region - Aniamtions
        CreateDir(@"Character\Animations\#Controllers");
        CreateDir(@"Character\Animations\Clips");
        #endregion
        CreateDir(@"Character\Models & Rigs");
        CreateDir(@"Character\Textures & Materials");
        #endregion
        #region ----- Level
        CreateDir(@"Level\#Prefabs");
        CreateDir(@"Level\Materials");
        #region - Scenes
        #region -- Lvls
        CreateScene(@"Level\Scenes\Lvls\01", @"Level_01");
        #endregion
        #region -- Main
        CreateScene(@"Level\Scenes\Main", @"Main");
        #endregion
        #region -- Menu
        CreateScene(@"Level\Scenes\Menu", @"Menu");
        #endregion
        #region -- Overlays
        CreateScene(@"Level\Scenes\Overlays", @"Menu");
        CreateScene(@"Level\Scenes\Overlays", @"EndScreen_Overlay");
        CreateScene(@"Level\Scenes\Overlays", @"GUI_Overlay");
        CreateScene(@"Level\Scenes\Overlays", @"Menu_Overlay");
        CreateScene(@"Level\Scenes\Overlays", @"Settings_Overlay");
        #endregion
        #endregion
        CreateDir(@"Level\SkyBoxs");
        CreateDir(@"Level\VFXs");
        #endregion
        #region ----- Media
        CreateDir(@"Media\Audios");
        CreateDir(@"Media\Videos");
        #endregion
        #region ----- Resources
        CreateDir(@"Resources\Assets");
        CreateDir(@"Resources\Plugins");
        #endregion
        #region ----- Scripts
        CreateDir(@"Scripts\Input");
        CreateDir(@"Scripts\Gameplay");
        CreateDir(@"Scripts\Manager");
        CreateDir(@"Scripts\Main");
        CreateDir(@"Scripts\UI");
        CreateDir(@"Scripts\Scriptables\Scripts");
        CreateDir(@"Scripts\Shader");
        #endregion
        #region ----- UI
        CreateDir(@"UI\#Prefabs");
        CreateDir(@"UI\Backgrounds");
        CreateDir(@"UI\Fonts & Materials");
        CreateDir(@"UI\References");
        #region - Sprites
        CreateDir(@"UI\Sprites\Icons");
        CreateDir(@"UI\Sprites\Images");
        CreateDir(@"UI\Sprites\Primitves");
        CreateDir(@"UI\Sprites\Reference");
        #endregion
        #endregion

        Debug.Log("Finished Successfully");
    }

    #region ----- Utilities
    void CreateDir(string _pathName)
    {
        Directory.CreateDirectory(Directory.GetCurrentDirectory() + @"\Assets\" + _pathName);
        if (m_createGitReady)
            CreateFile(_pathName, "Txt.txt");
    }
    void CreateScene(string _pathName, string _sceneName)
    {
        CreateDir(_pathName);
        File.Create(Directory.GetCurrentDirectory() + @"\Assets\" + _pathName + @"\" + _sceneName + @".unity");
    }
    void CreateFile(string _pathName, string _fileName)
    {
        File.Create(Directory.GetCurrentDirectory() + @"\Assets\" + _pathName + @"\" + _fileName);
    }
    #endregion
}
