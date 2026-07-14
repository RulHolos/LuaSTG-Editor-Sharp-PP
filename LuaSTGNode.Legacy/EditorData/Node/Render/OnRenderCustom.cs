using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using LuaSTGEditorSharp.EditorData.Message;
using LuaSTGEditorSharp.EditorData.Node.NodeAttributes;

namespace LuaSTGEditorSharp.EditorData.Node.Render
{
    [Serializable, NodeIcon("onrender.png")]
    [RequireParent(typeof(ObjectPoolTypeAlikeTypes))]
    public class OnRenderCustom : TreeNode, ICallBackFunc
    {
        [JsonConstructor]
        private OnRenderCustom() : base() { }

        public OnRenderCustom(DocumentData workSpaceData) : this(workSpaceData, "render") { }

        public OnRenderCustom(DocumentData workSpaceData, string ev) : base(workSpaceData) {
            EventType = ev;
            //attributes.Add(new AttrItem("Event type", ev, this, "event"));
        }

        [JsonIgnore, NodeAttribute]
        public string EventType {
            get => DoubleCheckAttr(0, "event", "Event type").attrInput;
            set => DoubleCheckAttr(0, "event", "Event type").attrInput = value;
        }

        public override IEnumerable<string> ToLua(int spacing) {
            string sp = Indent(spacing);
            TreeNode Parent = GetLogicalParent();
            string parentName = "";

            if (Parent?.attributes != null && Parent.AttributeCount >= 2)
                parentName = Lua.StringParser.ParseLua(Parent.NonMacrolize(0) +
                   (Parent.NonMacrolize(1) == "All" ? "" : ":" + Parent.NonMacrolize(1)));

            yield return sp + "_editor_class[\"" + parentName + "\"]." + NonMacrolize(0) + " = function(self)\n";

            foreach (var a in base.ToLua(spacing + 1))
                yield return a;

            yield return sp + "end\n";
        }

        public override string ToString() {
            return "On " + NonMacrolize(0) + "()";
        }

        public override object Clone() {
            var n = new OnRenderCustom(parentWorkSpace);
            n.DeepCopyFrom(this);
            return n;
        }

        public override IEnumerable<Tuple<int, TreeNode>> GetLines() {
            yield return new Tuple<int, TreeNode>(1, this);

            foreach (Tuple<int, TreeNode> t in base.GetChildLines())
                yield return t;

            yield return new Tuple<int, TreeNode>(1, this);
        }

        [JsonIgnore]
        public string FuncName {
            get => NonMacrolize(0);
        }

        public override List<MessageBase> GetMessage()
        {
            var a = new List<MessageBase>();
            TreeNode p = GetLogicalParent();

            if (p?.attributes == null || p.AttributeCount < 2)
                a.Add(new CannotFindAttributeInParent(2, this));

            return a;
        }
    }
}
