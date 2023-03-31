using UnityEditor;

namespace Simplicity.Editor
{
    public static class CreateScriptTemplates
    {
        private const string TEMPLATE_ROOT_PATH = "Assets/Simplicity/Code/Editor/Templates/";
        private const string MENU_ITEM_ROOT_PATH = "Assets/Create/Code/";

        [MenuItem(MENU_ITEM_ROOT_PATH + "MonoBehaviour", priority = 40)]
        public static void CreateMonoBehaviour()
        {
            const string templatePath = TEMPLATE_ROOT_PATH + "MonoBehaviour.cs.txt";
            ProjectWindowUtil.CreateScriptAssetFromTemplateFile(templatePath, "NewMonoBehaviour.cs");
        }

        [MenuItem(MENU_ITEM_ROOT_PATH + "Scriptable Object", priority = 41)]
        public static void CreateScriptableObject()
        {
            const string templatePath = TEMPLATE_ROOT_PATH + "ScriptableObject.cs.txt";
            ProjectWindowUtil.CreateScriptAssetFromTemplateFile(templatePath, "NewScriptableObject.cs");
        }

        [MenuItem(MENU_ITEM_ROOT_PATH + "StateMachineBehaviour", priority = 42)]
        public static void CreateStateMachineBehaviourObject()
        {
            const string templatePath = TEMPLATE_ROOT_PATH + "StateMachineBehaviour.cs.txt";
            ProjectWindowUtil.CreateScriptAssetFromTemplateFile(templatePath, "NewStateMachineBehaviour.cs");
        }

        [MenuItem(MENU_ITEM_ROOT_PATH + "C# Script", priority = 43)]
        public static void CreateCSharp()
        {
            const string templatePath = TEMPLATE_ROOT_PATH + "CSharp.cs.txt";
            ProjectWindowUtil.CreateScriptAssetFromTemplateFile(templatePath, "NewCSharp.cs");
        }

        [MenuItem(MENU_ITEM_ROOT_PATH + "Interface", priority = 44)]
        public static void CreateInterface()
        {
            const string templatePath = TEMPLATE_ROOT_PATH + "Interface.cs.txt";
            ProjectWindowUtil.CreateScriptAssetFromTemplateFile(templatePath, "NewInterface.cs");
        }

        [MenuItem(MENU_ITEM_ROOT_PATH + "Struct", priority = 45)]
        public static void CreateStruct()
        {
            const string templatePath = TEMPLATE_ROOT_PATH + "Struct.cs.txt";
            ProjectWindowUtil.CreateScriptAssetFromTemplateFile(templatePath, "NewStruct.cs");
        }
    }
}