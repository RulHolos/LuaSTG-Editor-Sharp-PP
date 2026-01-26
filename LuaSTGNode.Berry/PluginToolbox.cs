using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LuaSTGEditorSharp.Plugin;
using LuaSTGEditorSharp.EditorData;
using LuaSTGEditorSharp.EditorData.Node.General;
using LuaSTGEditorSharp.EditorData.Node.Advanced;
using System.Windows;
using System.Windows.Resources;
using System.IO;
using LuaSTGEditorSharp.EditorData.Node.Tween;
using LuaSTGEditorSharp.EditorData.Node.Scenes;

namespace LuaSTGEditorSharp
{
    public class PluginToolbox : AbstractToolbox
    {
        public PluginToolbox(IMainWindow mw) : base(mw) { }

        public override void InitFunc()
        {
            var tween = new Dictionary<ToolboxItemData, AddNode>();
            #region tweens
            tween.Add(new ToolboxItemData("createTween", "/LuaSTGNode.Berry;component/images/createtween.png", "Create Tween")
                , new AddNode(AddCreateTweenNode));
            tween.Add(new ToolboxItemData("tweenEase", "/LuaSTGNode.Berry;component/images/tweenease.png", "Ease Tween")
                , new AddNode(AddTweenEaseNode));
            #endregion
            ToolInfo.Add("Tweens", tween);

            var scenes = new Dictionary<ToolboxItemData, AddNode>();
            #region scenes
            scenes.Add(new ToolboxItemData("sceneGroup", "/LuaSTGNode.Berry;component/images/scenegroup.png", "Create Scene Group")
                , new AddNode(AddSceneGroup));
            scenes.Add(new ToolboxItemData("scene", "/LuaSTGNode.Berry;component/images/scene.png", "Create Scene")
                , new AddNode(AddScene));
            #endregion
            ToolInfo.Add("Scenes", scenes);
        }

        #region tweens

        private void AddCreateTweenNode()
        {
            parent.Insert(new CreateTween(parent.ActivatedWorkSpaceData));
        }

        private void AddTweenEaseNode()
        {
            parent.Insert(new TweenEase(parent.ActivatedWorkSpaceData));
        }

        #endregion
        #region scenes

        private void AddSceneGroup()
        {
            parent.Insert(new SceneGroup(parent.ActivatedWorkSpaceData));
        }

        private void AddScene()
        {
            parent.Insert(new Scene(parent.ActivatedWorkSpaceData));
        }

        #endregion
    }
}
