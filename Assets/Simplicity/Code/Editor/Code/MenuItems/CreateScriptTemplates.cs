using UnityEditor;

namespace Simplicity.Editor
{
    public static class CreateScriptTemplates
    {
        private const string TEMPLATE_ROOT_PATH = "Assets/Simplicity/Editor/Templates/";

        [MenuItem("Assets/Create/Code/MonoBehaviour", priority = 40)]
        public static void CreateMonoBehaviour()
        {
            const string templatePath = TEMPLATE_ROOT_PATH + "MonoBehaviour.cs.txt";
            ProjectWindowUtil.CreateScriptAssetFromTemplateFile(templatePath, "NewMonoBehaviour.cs");
        }

        [MenuItem("Assets/Create/Code/Scriptable Object", priority = 41)]
        public static void CreateScriptableObject()
        {
            const string templatePath = TEMPLATE_ROOT_PATH + "ScriptableObject.cs.txt";
            ProjectWindowUtil.CreateScriptAssetFromTemplateFile(templatePath, "NewScriptableObject.cs");
        }

        [MenuItem("Assets/Create/Code/StateMachineBehaviour", priority = 42)]
        public static void CreateStateMachineBehaviourObject()
        {
            const string templatePath = TEMPLATE_ROOT_PATH + "StateMachineBehaviour.cs.txt";
            ProjectWindowUtil.CreateScriptAssetFromTemplateFile(templatePath, "NewStateMachineBehaviour.cs");
        }

        [MenuItem("Assets/Create/Code/C# Script", priority = 43)]
        public static void CreateCSharp()
        {
            const string templatePath = TEMPLATE_ROOT_PATH + "CSharp.cs.txt";
            ProjectWindowUtil.CreateScriptAssetFromTemplateFile(templatePath, "NewCSharp.cs");
        }

        [MenuItem("Assets/Create/Code/Interface", priority = 44)]
        public static void CreateInterface()
        {
            const string templatePath = TEMPLATE_ROOT_PATH + "Interface.cs.txt";
            ProjectWindowUtil.CreateScriptAssetFromTemplateFile(templatePath, "NewInterface.cs");
        }

        [MenuItem("Assets/Create/Code/Struct", priority = 45)]
        public static void CreateStruct()
        {
            const string templatePath = TEMPLATE_ROOT_PATH + "Struct.cs.txt";
            ProjectWindowUtil.CreateScriptAssetFromTemplateFile(templatePath, "NewStruct.cs");
        }

        [MenuItem("Assets/Create/Code/HFSM/Action", priority = 50)]
        // ReSharper disable once InconsistentNaming
        public static void CreateHFSMAction()
        {
            const string templatePath = TEMPLATE_ROOT_PATH + "HFSM/Action.cs.txt";
            ProjectWindowUtil.CreateScriptAssetFromTemplateFile(templatePath, "NewAction.cs");
        }

        [MenuItem("Assets/Create/Code/HFSM/Decision", priority = 50)]
        // ReSharper disable once InconsistentNaming
        public static void CreateHFSMDecision()
        {
            const string templatePath = TEMPLATE_ROOT_PATH + "HFSM/Decision.cs.txt";
            ProjectWindowUtil.CreateScriptAssetFromTemplateFile(templatePath, "NewDecision.cs");
        }
    }
}