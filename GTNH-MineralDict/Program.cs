using System.Text;

namespace GTNH_MineralDict;

class Program
{
    static void Main(string[] args)
    {
        var processor = new MineralDictProcessor();
        processor.GenerateDict();
    }
}

public class MineralDictProcessor
{
    #region Const

    #endregion

    #region Fields

    private List<Material> Materials { get; set; } = [];

    // 水洗
    private readonly StringBuilder _h20WashInclude = new();

    private readonly StringBuilder _h2OWashExclude = new();

    // 汞洗
    private readonly StringBuilder _hgWashInclude = new();

    private readonly StringBuilder _hgWashExclude = new();

    // 过硫酸钠洗
    private readonly StringBuilder _na2S2O8WashInclude = new();

    private readonly StringBuilder _na2S2O8WashExclude = new();

    // 粉碎机
    private readonly StringBuilder _crusherInclude = new();

    private readonly StringBuilder _crusherExclude = new();

    // 筛选机
    private readonly StringBuilder _sieveInclude = new();

    private readonly StringBuilder _sieveExclude = new();

    // 热离
    private readonly StringBuilder _heatCentrifugeInclude = new();

    private readonly StringBuilder _heatCentrifugeExclude = new();

    // 离心
    private readonly StringBuilder _centrifugeInclude = new();

    private readonly StringBuilder _centrifugeExclude = new();

    // 矿处中间产物
    private readonly StringBuilder _intermediateInclude = new();
    private readonly StringBuilder _intermediateExclude = new();

    #endregion


    public void GenerateDict()
    {
        Materials = ImportMaterialData();
        GenerateMineralDict();
    }

    public List<Material> ImportMaterialData()
    {
        var ores = new List<Material>();

        // 导入OreData

        return ores;
    }

    public void GenerateMineralDict()
    {
        // 生成MineralDict
        AppendAll("===Ore/RawOre 原矿/粗矿===");
        AppendAll("");
        foreach (var m in Materials)
        {
            // TODO 处理油砂
            AppendMaterial(m.DictNameOre, m.DictNameOreZh, needCrusher: true);
            AppendMaterial(m.DictNameRawOre, m.DictNameRawOreZh, needCrusher: true);
        }

        AppendAll("");
        AppendAll("===Crushed 粉碎矿石===");
        AppendAll("");

        foreach (var m in Materials)
        {
            switch (m.Mode)
            {
                case ProcessMode.Ignore:
                    AppendMaterial(m.DictNameCrushed, m.DictNameCrushedZh);
                    break;
                case ProcessMode.XiFenLi:
                case ProcessMode.XiShai:
                case ProcessMode.XiReFen:
                case ProcessMode.Xi:
                    AppendMaterial(m.DictNameCrushed, m.DictNameCrushedZh, needH20Wash: true, isIntermediate: true);
                    break;
                case ProcessMode.HgReFen:
                    AppendMaterial(m.DictNameCrushed, m.DictNameCrushedZh, needHgWash: true, isIntermediate: true);
                    break;
                case ProcessMode.Na2S2O8Shai:
                    AppendMaterial(m.DictNameCrushed, m.DictNameCrushedZh, needNa2S2O8Wash: true, isIntermediate: true);
                    break;
                case ProcessMode.FenLi:
                    AppendMaterial(m.DictNameCrushed, m.DictNameCrushedZh, needCrusher: true, isIntermediate: true);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        AppendAll("");
        AppendAll("===CrushedPurified 洗净矿石===");
        AppendAll("");

        foreach (var m in Materials)
        {
            switch (m.Mode)
            {
                case ProcessMode.Ignore:
                    break;
                case ProcessMode.XiFenLi:
                    break;
                case ProcessMode.XiShai:
                    break;
                case ProcessMode.XiReFen:
                    break;
                case ProcessMode.Xi:
                    break;
                case ProcessMode.HgReFen:
                    break;
                case ProcessMode.Na2S2O8Shai:
                    break;
                case ProcessMode.FenLi:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    public void AppendAll(string str)
    {
        _h20WashInclude.Append(str).Append(Environment.NewLine);
        _h2OWashExclude.Append(str).Append(Environment.NewLine);
        _hgWashInclude.Append(str).Append(Environment.NewLine);
        _hgWashExclude.Append(str).Append(Environment.NewLine);
        _na2S2O8WashInclude.Append(str).Append(Environment.NewLine);
        _na2S2O8WashExclude.Append(str).Append(Environment.NewLine);
        _crusherInclude.Append(str).Append(Environment.NewLine);
        _crusherExclude.Append(str).Append(Environment.NewLine);
        _sieveInclude.Append(str).Append(Environment.NewLine);
        _sieveExclude.Append(str).Append(Environment.NewLine);
        _heatCentrifugeInclude.Append(str).Append(Environment.NewLine);
        _heatCentrifugeExclude.Append(str).Append(Environment.NewLine);
        _centrifugeInclude.Append(str).Append(Environment.NewLine);
        _centrifugeExclude.Append(str).Append(Environment.NewLine);
        _intermediateInclude.Append(str).Append(Environment.NewLine);
        _intermediateExclude.Append(str).Append(Environment.NewLine);
    }

    public void AppendMaterial(
        string nameDict,
        string nameZh,
        bool needH20Wash = false,
        bool needHgWash = false,
        bool needNa2S2O8Wash = false,
        bool needCrusher = false,
        bool needSieve = false,
        bool needHeatCentrifuge = false,
        bool needCentrifuge = false,
        bool isIntermediate = false)
    {
    }
}

public class Material(string name, string nameZh, ProcessMode mode)
{
    // public const string PrefixOre = "ore";
    // public const string PrefixRawOre = "rawOre";
    // public const string PrefixCrushed = "crushed";
    // public const string PrefixCrushedPurified = "crushedPurified";
    // public const string PrefixCrushedCentrifuged = "crushedCentrifuged";
    // public const string PrefixDust = "dust";
    // public const string PrefixDustPure = "dustPure";
    // public const string PrefixDustImpure = "dustImpure";

    // public const string PrefixOreZh = "原矿";
    // public const string PrefixRawOreZh = "粗矿";
    // public const string PrefixCrushedZh = "粉碎矿石";
    // public const string PrefixCrushedPurifiedZh = "洗净矿石";
    // public const string PrefixCrushedCentrifugedZh = "离心矿石";
    // public const string PrefixDustZh = "粉";
    // public const string PrefixDustPureZh = "纯净粉";
    // public const string PrefixDustImpureZh = "含杂粉";

    public string Name { get; set; } = name;
    public string NameZh { get; set; } = nameZh;
    public ProcessMode Mode { get; set; } = mode;

    public string DictNameOre => "ore" + Name;
    public string DictNameRawOre => "rawOre" + Name;
    public string DictNameCrushed => "crushed" + Name;
    public string DictNameCrushedPurified => "crushedPurified" + Name;
    public string DictNameCrushedCentrifuged => "crushedCentrifuged" + Name;
    public string DictNameDust => "dust" + Name;
    public string DictNameDustPure => "dustPure" + Name;
    public string DictNameDustImpure => "dustImpure" + Name;

    public string DictNameOreZh => NameZh + "-原矿";
    public string DictNameRawOreZh => NameZh + "-粗矿";
    public string DictNameCrushedZh => NameZh + "-粉碎矿石";
    public string DictNameCrushedPurifiedZh => NameZh + "-洗净矿石";
    public string DictNameCrushedCentrifugedZh => NameZh + "-离心矿石";
    public string DictNameDustZh => NameZh + "-粉";
    public string DictNameDustPureZh => NameZh + "-纯净粉";
    public string DictNameDustImpureZh => NameZh + "-含杂粉";
}

public enum ProcessMode
{
    Ignore = 0,
    XiFenLi,
    XiShai,
    XiReFen,
    Xi,
    HgReFen,
    Na2S2O8Shai,
    FenLi,
}