using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Xml.Serialization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LuaSTGEditorSharp.EditorData.Document;
using LuaSTGEditorSharp.EditorData.Message;
using LuaSTGEditorSharp.EditorData.Document.Meta;
using LuaSTGEditorSharp.EditorData.Node.NodeAttributes;
using Newtonsoft.Json;

namespace LuaSTGEditorSharp.EditorData.Node.Object
{
    [Serializable, NodeIcon("objectdefine.png")]
    [ClassNode]
    [CreateInvoke(0), RCInvoke(0)]
    //[XmlType(TypeName = "BulletDefine")]
    public class ObjectDefine : TreeNode
    {
        [JsonConstructor]
        private ObjectDefine() : base() { }

        public ObjectDefine(DocumentData workSpaceData)
            : this(workSpaceData, "", "All", "_object") { }

        public ObjectDefine(DocumentData workSpaceData, string name, string difficulty, string type)
            : base(workSpaceData)
        {
            /*
            attributes.Add(new AttrItem("Name", name, this));
            attributes.Add(new AttrItem("Difficulty", difficulty, this, "objDifficulty"));
            attributes.Add(new AttrItem("Type", type, this, "objType"));
            */
            Name = name;
            Difficulty = difficulty;
            Type = type;
        }

        [JsonIgnore, NodeAttribute, XmlAttribute("Name")]
        //[DefaultValue("")]
        public string Name
        {
            get => DoubleCheckAttr(0).attrInput;
            set => DoubleCheckAttr(0).attrInput = value;
        }

        [JsonIgnore, NodeAttribute, XmlAttribute("Difficulty")]
        //[DefaultValue("All")]
        public string Difficulty {
            get => DoubleCheckAttr(1, "objDifficulty").attrInput;
            set => DoubleCheckAttr(1, "objDifficulty").attrInput = value;
        }

        [JsonIgnore, NodeAttribute, XmlAttribute("Class")]
        //[DefaultValue("_object")]
        public string Type
        {
            get => DoubleCheckAttr(2, "objType", "Base Class").attrInput;
            set => DoubleCheckAttr(2, "objType", "Base Class").attrInput = value;
        }

        public override IEnumerable<string> ToLua(int spacing)
        {
            string sp = Indent(spacing);
            string difficultyS = NonMacrolize(1) == "All" ? "" : ":" + NonMacrolize(1);

            string typeInput = NonMacrolize(2);
            string typeStr = string.IsNullOrEmpty(typeInput) ? "_object" : (typeInput + " or _editor_class[\"" + typeInput + "\"]");

            yield return sp + "_editor_class[\"" + Lua.StringParser.ParseLua(NonMacrolize(0) + difficultyS) + "\"] = Class(" + typeStr + ")\n";
            foreach (var a in base.ToLua(spacing))
            {
                yield return a;
            }
        }

        public override IEnumerable<Tuple<int,TreeNode>> GetLines()
        {
            yield return new Tuple<int, TreeNode>(1, this);
            foreach(Tuple<int,TreeNode> t in GetChildLines())
            {
                yield return t;
            }
        }

        public override string ToString()
        {
            string difficultyS = NonMacrolize(1) == "All" ? "" : ":" + NonMacrolize(1);

            string typeInput = NonMacrolize(2);
            string typeStr = (typeInput == "_object" || string.IsNullOrEmpty(typeInput)) ? "object" : typeInput;
            return "Define " + typeStr + " type \"" + NonMacrolize(0) + difficultyS + "\"";
        }

        public override object Clone()
        {
            var n = new ObjectDefine(parentWorkSpace);
            n.DeepCopyFrom(this);
            return n;
        }

        public override MetaInfo GetMeta()
        {
            return new ObjectDefineMetaInfo(this);
        }

        public override string GetDifficulty()
        {
            return NonMacrolize(1) == "All" ? "" : NonMacrolize(1);
        }

        public override List<MessageBase> GetMessage()
        {
            List<MessageBase> messages = new List<MessageBase>();
            if (string.IsNullOrEmpty(NonMacrolize(0)))
                messages.Add(new ArgNotNullMessage(attributes[0].AttrCap, 0, this));
            return messages;
        }
    }
}
