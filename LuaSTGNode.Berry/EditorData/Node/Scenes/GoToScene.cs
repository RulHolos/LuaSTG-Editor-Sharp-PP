using LuaSTGEditorSharp.EditorData.Node.NodeAttributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuaSTGEditorSharp.EditorData.Node.Scenes;

[Serializable, NodeIcon("scene.png")]
[LeafNode]
[RCInvoke(0)]
public class GoToScene : TreeNode
{
    [JsonConstructor]
    private GoToScene() : base() { }

    public GoToScene(DocumentData workSpaceData)
        : this(workSpaceData, "", "false", "false") { }

    public GoToScene(DocumentData workSpaceData, string name, string isEntry, string isMenu)
        : base(workSpaceData)
    {
        Name = name;
    }

    [JsonIgnore, NodeAttribute]
    public string Name
    {
        get => DoubleCheckAttr(0).attrInput;
        set => DoubleCheckAttr(0).AttrInput = value;
    }

    public override string ToString()
    {
        return $"";
    }

    public override IEnumerable<string> ToLua(int spacing)
    {
        string sp = Indent(spacing);
        yield return sp + $"";
    }

    public override IEnumerable<Tuple<int, TreeNode>> GetLines()
    {
        yield return new Tuple<int, TreeNode>(1, this);
    }

    public override object Clone()
    {
        var n = new GoToScene(parentWorkSpace);
        n.DeepCopyFrom(this);
        return n;
    }
}
