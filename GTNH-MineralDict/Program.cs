using System.Text;

namespace GTNH_MineralDict;

class Program
{
    static void Main(string[] args)
    {
        var processor = new MineralDictProcessor();
        processor.GenerateOreData();
    }
}

public class MineralDictProcessor
{
    public List<Ore> Ores { get; set; } = [];
    // 水洗
    public readonly StringBuilder H20WashInclude = new();
    public readonly StringBuilder H2OWashExclude = new();
    // 汞洗
    public readonly StringBuilder HgWashInclude = new();
    public readonly StringBuilder HgWashExclude = new();
    // 过硫酸钠洗
    public readonly StringBuilder Na2S2O8WashInclude = new();
    public readonly StringBuilder Na2S2O8WashExclude = new();
    // 粉碎机
    public readonly StringBuilder CrusherInclude = new();
    public readonly StringBuilder CrusherExclude = new();
    // 筛选机
    public readonly StringBuilder SieveInclude = new();
    public readonly StringBuilder SieveExclude = new();
    // 热离
    public readonly StringBuilder HeatCentrifugeInclude = new();
    public readonly StringBuilder HeatCentrifugeExclude = new();
    // 离心
    public readonly StringBuilder CentrifugeInclude = new();
    public readonly StringBuilder CentrifugeExclude = new();
    // 矿处中间产物
    public readonly StringBuilder IntermediateInclude = new();
    public readonly StringBuilder IntermediateExclude = new();


    public void GenerateOreData()
    {
        Ores = ImportOreData();
        GenerateMineralDict();
    }

    public List<Ore> ImportOreData()
    {
        var ores = new List<Ore>();

        // 导入OreData

        return ores;
    }

    public void GenerateMineralDict()
    {
        // 生成MineralDict

    }

    public void AppendAll(string str)
    {
        H20WashInclude.Append(str).Append(Environment.NewLine);
        H2OWashExclude.Append(str).Append(Environment.NewLine);
        HgWashInclude.Append(str).Append(Environment.NewLine);
        HgWashExclude.Append(str).Append(Environment.NewLine);
        Na2S2O8WashInclude.Append(str).Append(Environment.NewLine);
        Na2S2O8WashExclude.Append(str).Append(Environment.NewLine);
        CrusherInclude.Append(str).Append(Environment.NewLine);
        CrusherExclude.Append(str).Append(Environment.NewLine);
        SieveInclude.Append(str).Append(Environment.NewLine);
        SieveExclude.Append(str).Append(Environment.NewLine);
        HeatCentrifugeInclude.Append(str).Append(Environment.NewLine);
        HeatCentrifugeExclude.Append(str).Append(Environment.NewLine);
        CentrifugeInclude.Append(str).Append(Environment.NewLine);
        CentrifugeExclude.Append(str).Append(Environment.NewLine);
        IntermediateInclude.Append(str).Append(Environment.NewLine);
        IntermediateExclude.Append(str).Append(Environment.NewLine);
    }
}

public class Ore(string name, string nameZh, ProcessMode mode)
{
    public string Name { get; set; } = name;
    public string NameZh { get; set; } = nameZh;
    public ProcessMode Mode { get; set; } = mode;
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