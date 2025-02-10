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
        ExportMineralDict();
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

        // 全部粉掉
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
                case ProcessMode.Xi:
                    AppendMaterial(m.DictNameCrushedPurified, m.DictNameCrushedPurifiedZh);
                    break;
                case ProcessMode.XiFenLi:
                case ProcessMode.Ignore:
                case ProcessMode.FenLi:
                    AppendMaterial(m.DictNameCrushedPurified, m.DictNameCrushedPurifiedZh, needCrusher: true,
                        isIntermediate: true);
                    break;
                case ProcessMode.XiShai:
                case ProcessMode.Na2S2O8Shai:
                    AppendMaterial(m.DictNameCrushedPurified, m.DictNameCrushedPurifiedZh, needSieve: true,
                        isIntermediate: true);
                    break;
                case ProcessMode.XiReFen:
                case ProcessMode.HgReFen:
                    AppendMaterial(m.DictNameCrushedPurified, m.DictNameCrushedPurifiedZh, needHeatCentrifuge: true,
                        isIntermediate: true);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        AppendAll("");
        AppendAll("===CrushedCentrifuged 热离（离心）矿石===");
        AppendAll("");

        // 全部粉掉
        foreach (var m in Materials)
        {
            AppendMaterial(m.DictNameCrushedCentrifuged, m.DictNameCrushedCentrifugedZh, needCrusher: true,
                isIntermediate: true);
        }

        AppendAll("");
        AppendAll("===Dust 粉===");
        AppendAll("");

        // 不处理
        foreach (var m in Materials)
        {
            AppendMaterial(m.DictNameDust, m.DictNameDustZh);
        }

        AppendAll("");
        AppendAll("===DustPure/dustImpure 纯净粉/含杂粉===");
        AppendAll("");

        // 全部离心
        foreach (var m in Materials)
        {
            AppendMaterial(m.DictNameDustPure, m.DictNameDustPureZh, needCentrifuge: true);
            AppendMaterial(m.DictNameDustImpure, m.DictNameDustImpureZh, needCentrifuge: true);
        }
    }

    public void ExportMineralDict()
    {
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
        if (needH20Wash)
        {
            _h20WashInclude.AppendLine(nameDict + " //" + nameZh);
        }
        else
        {
            _h2OWashExclude.AppendLine(nameDict + " //" + nameZh);
        }

        if (needHgWash)
        {
            _hgWashInclude.AppendLine(nameDict + " //" + nameZh);
        }
        else
        {
            _hgWashExclude.AppendLine(nameDict + " //" + nameZh);
        }

        if (needNa2S2O8Wash)
        {
            _na2S2O8WashInclude.AppendLine(nameDict + " //" + nameZh);
        }
        else
        {
            _na2S2O8WashExclude.AppendLine(nameDict + " //" + nameZh);
        }

        if (needCrusher)
        {
            _crusherInclude.AppendLine(nameDict + " //" + nameZh);
        }
        else
        {
            _crusherExclude.AppendLine(nameDict + " //" + nameZh);
        }

        if (needSieve)
        {
            _sieveInclude.AppendLine(nameDict + " //" + nameZh);
        }
        else
        {
            _sieveExclude.AppendLine(nameDict + " //" + nameZh);
        }

        if (needHeatCentrifuge)
        {
            _heatCentrifugeInclude.AppendLine(nameDict + " //" + nameZh);
        }
        else
        {
            _heatCentrifugeExclude.AppendLine(nameDict + " //" + nameZh);
        }

        if (needCentrifuge)
        {
            _centrifugeInclude.AppendLine(nameDict + " //" + nameZh);
        }
        else
        {
            _centrifugeExclude.AppendLine(nameDict + " //" + nameZh);
        }

        if (isIntermediate)
        {
            _intermediateInclude.AppendLine(nameDict + " //" + nameZh);
        }
        else
        {
            _intermediateExclude.AppendLine(nameDict + " //" + nameZh);
        }
    }
}

public class Material(string name, string nameZh, ProcessMode mode)
{
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