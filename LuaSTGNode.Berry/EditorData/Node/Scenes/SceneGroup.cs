using LuaSTGEditorSharp.EditorData.Node.NodeAttributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuaSTGEditorSharp.EditorData.Node.Scenes;

[Serializable, NodeIcon("scenegroup.png")]
[ClassNode]
[RCInvoke(0)]
public class SceneGroup : TreeNode
{
    [JsonConstructor]
    private SceneGroup() : base() { }

    public SceneGroup(DocumentData workSpaceData)
        : this(workSpaceData, "Normal", "1", "false", "{ life = 3, bombs = 2 }", "true") { }

    public SceneGroup(DocumentData workSpaceData, string name, string diff, string isEntry, string resources, string allow_pr)
        : base(workSpaceData)
    {
        Name = name;
        DifficultyValue = diff;
        IsEntry = isEntry;
        StartResources = resources;
        AllowPr = allow_pr;
    }

    [JsonIgnore, NodeAttribute]
    public string Name
    {
        get => DoubleCheckAttr(0, "stageGroup").attrInput;
        set => DoubleCheckAttr(0, "stageGroup").AttrInput = value;
    }

    [JsonIgnore, NodeAttribute]
    public string DifficultyValue
    {
        get => DoubleCheckAttr(1, "difficulty", "Difficulty Index").attrInput;
        set => DoubleCheckAttr(1, "difficulty", "Difficulty Index").AttrInput = value;
    }

    [JsonIgnore, NodeAttribute]
    public string IsEntry
    {
        get => DoubleCheckAttr(2, "bool", "Is Entry Group?").attrInput;
        set => DoubleCheckAttr(2, "bool", "Is Entry Group?").AttrInput = value;
    }

    [JsonIgnore, NodeAttribute]
    public string StartResources
    {
        get => DoubleCheckAttr(3, name: "Start Resources").attrInput;
        set => DoubleCheckAttr(3, name: "Start Resources").AttrInput = value;
    }

    [JsonIgnore, NodeAttribute]
    public string AllowPr
    {
        get => DoubleCheckAttr(4, "bool", "Allow Practice").attrInput;
        set => DoubleCheckAttr(4, "bool", "Allow Practice").AttrInput = value;
    }

    public override string ToString()
    {
        return $"Scene Group \"{NonMacrolize(0)}\"";
    }

    public override IEnumerable<string> ToLua(int spacing)
    {
        string sp = Indent(spacing);
        string res = Macrolize(3);
        if (string.IsNullOrEmpty(Macrolize(3)))
            res = "{}";

        yield return sp + $"last_scene_group = SceneManager.newGroup(\"{Macrolize(0)}\", {Macrolize(1)}, {Macrolize(2)}, {res}, {Macrolize(3)})\n";
        foreach (var c in base.ToLua(spacing))
            yield return c;
    }

    public override IEnumerable<Tuple<int, TreeNode>> GetLines()
    {
        yield return new Tuple<int, TreeNode>(1, this);
        foreach (Tuple<int, TreeNode> t in GetChildLines())
            yield return t;
    }

    public override object Clone()
    {
        var n = new SceneGroup(parentWorkSpace);
        n.DeepCopyFrom(this);
        return n;
    }
}
