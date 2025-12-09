using LuaSTGEditorSharp.EditorData.Node.NodeAttributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuaSTGEditorSharp.EditorData.Node.Scenes;

[Serializable, NodeIcon("addbranch.png")]
[LeafNode, RequireParent(typeof(SceneGroup))]
[RCInvoke(0)]
public class AddBranch : TreeNode
{
    [JsonConstructor]
    private AddBranch() : base() { }

    public AddBranch(DocumentData workSpaceData)
        : this(workSpaceData, "5", "\"6a\", \"6b\"") { }

    public AddBranch(DocumentData workSpaceData, string from, string to)
        : base(workSpaceData)
    {
        From = from;
        To = to;
    }

    [JsonIgnore, NodeAttribute]
    public string From
    {
        get => DoubleCheckAttr(0).attrInput;
        set => DoubleCheckAttr(0).AttrInput = value;
    }

    [JsonIgnore, NodeAttribute]
    public string To
    {
        get => DoubleCheckAttr(1).attrInput;
        set => DoubleCheckAttr(1).AttrInput = value;
    }

    public override string ToString()
    {
        return $"Scene {NonMacrolize(0)} branches to ({NonMacrolize(1)})";
    }

    public override IEnumerable<string> ToLua(int spacing)
    {
        string sp = Indent(spacing);

        yield return sp + $"last_scene_group:addBranch({Macrolize(0)}, {{ {Macrolize(1)} }})";
    }

    public override IEnumerable<Tuple<int, TreeNode>> GetLines()
    {
        yield return new Tuple<int, TreeNode>(1, this);
    }

    public override object Clone()
    {
        var n = new AddBranch(parentWorkSpace);
        n.DeepCopyFrom(this);
        return n;
    }
}
