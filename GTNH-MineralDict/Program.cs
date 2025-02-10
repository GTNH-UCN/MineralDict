using System.Text;
using System.Text.Json;

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
    private const string DataSourcePath = "Assets/datasource.json";

    #region Fields

    private List<Material> Materials { get; set; } = [];

    // 水洗
    private readonly StringBuilder _h20WashInclude = new();
    private readonly StringBuilder _h2OWashExclude = new();

    // 汞洗
    private readonly StringBuilder _hgWashInclude = new();
    private readonly StringBuilder _hgWashExclude = new();

    // 过硫酸钠洗
    private readonly StringBuilder _spWashInclude = new();
    private readonly StringBuilder _spWashExclude = new();

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

    private static List<Material> ImportMaterialData()
    {
        // 导入OreData
        // 从Assets/datasource.json中反序列化数据
        var json = File.ReadAllText(DataSourcePath);
        var ores = JsonSerializer.Deserialize<List<Material>>(json);

        return ores ?? throw new InvalidOperationException("解析矿辞数据源失败，请检查数据源格式。");
    }

    private void GenerateMineralDict()
    {
        // 生成MineralDict
        AppendAll("===Ore/RawOre 原矿/粗矿===");
        AppendAll("");

        // 全部粉掉
        foreach (var m in Materials)
        {
            // TODO 处理油砂
            AppendMaterial(m.DictNameOre, m.DictNameOreZh, needCrusher: true, isIntermediate: true);
            AppendMaterial(m.DictNameRawOre, m.DictNameRawOreZh, needCrusher: true, isIntermediate: true);
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
                case ProcessMode.SpXiShai:
                    AppendMaterial(m.DictNameCrushed, m.DictNameCrushedZh, needSpWash: true, isIntermediate: true);
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
                case ProcessMode.SpXiShai:
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
            AppendMaterial(m.DictNameDustPure, m.DictNameDustPureZh, needCentrifuge: true, isIntermediate: true);
            AppendMaterial(m.DictNameDustImpure, m.DictNameDustImpureZh, needCentrifuge: true, isIntermediate: true);
        }
    }

    private void ExportMineralDict()
    {
        // 获取项目根目录
        var projectRoot = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "../../../"));
        var artifactsDir = Path.Combine(projectRoot, "Artifacts");

        // 确保Artifacts目录存在
        Directory.CreateDirectory(artifactsDir);

        // 导出文件
        File.WriteAllText(Path.Combine(artifactsDir, "1-H2OWashInclude.txt"), _h20WashInclude.ToString());
        File.WriteAllText(Path.Combine(artifactsDir, "1-H2OWashExclude.txt"), _h2OWashExclude.ToString());
        File.WriteAllText(Path.Combine(artifactsDir, "2-HgWashInclude.txt"), _hgWashInclude.ToString());
        File.WriteAllText(Path.Combine(artifactsDir, "2-HgWashExclude.txt"), _hgWashExclude.ToString());
        File.WriteAllText(Path.Combine(artifactsDir, "3-SpWashInclude.txt"), _spWashInclude.ToString());
        File.WriteAllText(Path.Combine(artifactsDir, "3-SpWashExclude.txt"), _spWashExclude.ToString());
        File.WriteAllText(Path.Combine(artifactsDir, "4-CrusherInclude.txt"), _crusherInclude.ToString());
        File.WriteAllText(Path.Combine(artifactsDir, "4-CrusherExclude.txt"), _crusherExclude.ToString());
        File.WriteAllText(Path.Combine(artifactsDir, "5-SieveInclude.txt"), _sieveInclude.ToString());
        File.WriteAllText(Path.Combine(artifactsDir, "5-SieveExclude.txt"), _sieveExclude.ToString());
        File.WriteAllText(Path.Combine(artifactsDir, "6-HeatCentrifugeInclude.txt"), _heatCentrifugeInclude.ToString());
        File.WriteAllText(Path.Combine(artifactsDir, "6-HeatCentrifugeExclude.txt"), _heatCentrifugeExclude.ToString());
        File.WriteAllText(Path.Combine(artifactsDir, "7-CentrifugeInclude.txt"), _centrifugeInclude.ToString());
        File.WriteAllText(Path.Combine(artifactsDir, "7-CentrifugeExclude.txt"), _centrifugeExclude.ToString());
        File.WriteAllText(Path.Combine(artifactsDir, "8-IntermediateInclude.txt"), _intermediateInclude.ToString());
        File.WriteAllText(Path.Combine(artifactsDir, "8-IntermediateExclude.txt"), _intermediateExclude.ToString());
    }

    private void AppendAll(string str)
    {
        _h20WashInclude.Append(str).Append(Environment.NewLine);
        _h2OWashExclude.Append(str).Append(Environment.NewLine);
        _hgWashInclude.Append(str).Append(Environment.NewLine);
        _hgWashExclude.Append(str).Append(Environment.NewLine);
        _spWashInclude.Append(str).Append(Environment.NewLine);
        _spWashExclude.Append(str).Append(Environment.NewLine);
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

    private void AppendMaterial(
        string nameDict,
        string nameZh,
        bool needH20Wash = false,
        bool needHgWash = false,
        bool needSpWash = false,
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

        if (needSpWash)
        {
            _spWashInclude.AppendLine(nameDict + " //" + nameZh);
        }
        else
        {
            _spWashExclude.AppendLine(nameDict + " //" + nameZh);
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
    public string Name { get; set; } = name.Trim();
    public string NameZh { get; set; } = nameZh.Trim();
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
    SpXiShai,
    FenLi,
}