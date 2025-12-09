using LuaSTGEditorSharp.EditorData.Node.NodeAttributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuaSTGEditorSharp.EditorData.Node.Scenes;

[Serializable, NodeIcon("scene.png")]
[CannotHaveParent(typeof(Scene))] // Cannot be a child of itself
[RCInvoke(0)]
public class Scene : TreeNode
{
    [JsonConstructor]
    private Scene() : base() { }

    public Scene(DocumentData workSpaceData)
        : this(workSpaceData, "", "false", "false") { }

    public Scene(DocumentData workSpaceData, string name, string isEntry, string isMenu)
        : base(workSpaceData)
    {
        Name = name;
        IsEntry = isEntry;
        IsMenu = isMenu;
    }

    [JsonIgnore, NodeAttribute]
    public string Name
    {
        get => DoubleCheckAttr(0).attrInput;
        set => DoubleCheckAttr(0).AttrInput = value;
    }

    [JsonIgnore, NodeAttribute]
    public string IsEntry
    {
        get => DoubleCheckAttr(1, "bool", "Is Entry Scene?").attrInput;
        set => DoubleCheckAttr(1, "bool", "Is Entry Scene?").AttrInput = value;
    }

    [JsonIgnore, NodeAttribute]
    public string IsMenu
    {
        get => DoubleCheckAttr(2, "bool", "Is Menu?").attrInput;
        set => DoubleCheckAttr(2, "bool", "Is Menu?").AttrInput = value;
    }

    public override string ToString()
    {
        return $"Scene \"{NonMacrolize(0)}\"" +
            (NonMacrolize(1) == "true" ? ", is entry" : "") +
            (NonMacrolize(2) == "true" ? ", is menu" : "");
    }

    public override IEnumerable<string> ToLua(int spacing)
    {
        string sp = Indent(spacing);
        TreeNode parent = GetLogicalParent();
        bool isInGroupCtx = parent is SceneGroup;

        if (!isInGroupCtx)
            yield return sp + $"last_scene = SceneManager.new(\"{Macrolize(0)}\", {Macrolize(1)}, {Macrolize(2)})\n";
        else
            yield return sp + $"last_scene = last_scene_group:newScene(\"{Macrolize(0)}\")\n";
        foreach (var c in base.ToLua(spacing))
            yield return c;
    }

    public override IEnumerable<Tuple<int, TreeNode>> GetLines()
    {
        TreeNode parent = GetLogicalParent();
        bool isInGroupCtx = parent is SceneGroup;

        yield return new Tuple<int, TreeNode>(1, this);
        foreach (Tuple<int, TreeNode> t in GetChildLines())
            yield return t;
    }

    public override object Clone()
    {
        var n = new Scene(parentWorkSpace);
        n.DeepCopyFrom(this);
        return n;
    }
}
