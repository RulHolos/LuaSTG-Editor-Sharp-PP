using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LuaSTGEditorSharp.EditorData;
using LuaSTGEditorSharp.EditorData.Message;
using LuaSTGEditorSharp.EditorData.Document;
using LuaSTGEditorSharp.EditorData.Node.NodeAttributes;
using Newtonsoft.Json;

namespace LuaSTGEditorSharp.EditorData.Node.Object
{
    [Serializable, NodeIcon("defaultaction.png")]
    [RequireAncestor(typeof(CallBackFunc), typeof(Data.Function), typeof(Render.OnRender), typeof(Render.OnRenderCustom),
        typeof(Bullet.PlayerBulletRender), typeof(Bullet.PlayerBulletFrame), typeof(Bullet.PlayerBulletColli),
        typeof(Bullet.PlayerBulletKill), typeof(Bullet.PlayerBulletDel), typeof(Render.ItemOnRender), typeof(ObjectInit))]
    [LeafNode]
    public class DefaultAction : TreeNode
    {
        [JsonConstructor]
        private DefaultAction() : base() { }

        public DefaultAction(DocumentData workSpaceData)
            : this(workSpaceData, "", "false") { }

        public DefaultAction(DocumentData workSpaceData, string code, string fixParency)
            : base(workSpaceData)
        {
            CodeAddon = code;
            FixParency = fixParency;
        }

        [JsonIgnore, NodeAttribute]
        public string CodeAddon
        {
            get => DoubleCheckAttr(0, "event", "Event type").attrInput;
            set => DoubleCheckAttr(0, "event", "Event type").attrInput = value;
        }

        [JsonIgnore, NodeAttribute]
        public string FixParency {
            get => DoubleCheckAttr(1, "bool", "From parent class?").attrInput;
            set => DoubleCheckAttr(1, "bool", "From parent class?").attrInput = value;
        }

        public override IEnumerable<string> ToLua(int spacing)
        {
            string sp = Indent(spacing);
            TreeNode callBackFunc = this;
            string curClass = "self.class";

            if (NonMacrolize(1) == "true") {
                TreeNode node = Parent;

                while (node is not null and not ObjectDefine)
                    node = node.Parent;

                if (node is ObjectDefine def && !string.IsNullOrEmpty(def.Name))
                    curClass = "_editor_class[\"" + def.Name + "\"]";
            }

            if (!string.IsNullOrEmpty(NonMacrolize(0)))
            {
                yield return sp + curClass + ".base." + Macrolize(0) + "(self)\n";
            }
            else
            {
                while (!(callBackFunc is ICallBackFunc or ObjectInit) && callBackFunc != null)
                {
                    callBackFunc = callBackFunc.Parent;
                }

                if (callBackFunc is ICallBackFunc func)
                {
                    string other = func.FuncName == "colli" ? ", other" : "";
                    yield return sp + curClass + ".base." + func.FuncName + "(self" + other + ")\n";
                }
                else if (callBackFunc is ObjectInit) {
                    yield return sp + curClass + ".base.init(self, self.x, self.y)\n";
                }
                else // Keep this for GetLines or it becomes fucky.
                {
                    yield return "\n";
                }
            }
        }

        public override IEnumerable<Tuple<int,TreeNode>> GetLines()
        {
            yield return new Tuple<int, TreeNode>(2, this);
        }

        public override string ToString()
        {
            return "Do default action";
        }

        public override object Clone()
        {
            var n = new DefaultAction(parentWorkSpace);
            n.DeepCopyFrom(this);
            return n;
        }

        public override List<MessageBase> GetMessage()
        {
            var a = new List<MessageBase>();
            TreeNode callBackFunc = this;
            while (!(callBackFunc is ICallBackFunc or ObjectInit) && callBackFunc != null)
            {
                callBackFunc = callBackFunc.Parent;
            }
            if (callBackFunc == null && string.IsNullOrEmpty(NonMacrolize(0))) 
            {
                a.Add(new CannotFindAncestorTypeOf("CallBackFunc or ObjectInit", this));
            }
            return a;
        }
    }
}
