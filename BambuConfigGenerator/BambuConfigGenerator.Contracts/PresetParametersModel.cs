using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Globalization;
using BambuConfigGenerator.Contracts.Enums;

namespace BambuConfigGenerator.Contracts;

public partial class PresetParametersModel
{
    [JsonProperty("Brand")]
    public string Brand { get; set; }

    [JsonProperty("Type")]
    public FilamentTypes Type { get; set; }

    [JsonProperty("Serial")]
    public string Serial { get; set; }

    [JsonProperty("RecommendedTempMin")]
    public long RecommendedTempMin { get; set; }

    [JsonProperty("RecommendedTempMax")]
    public long RecommendedTempMax { get; set; }

    [JsonProperty("NozzleInitialLayerTemp")]
    public long NozzleInitialLayerTemp { get; set; }

    [JsonProperty("NozzleLayersTemp")]
    public long NozzleLayersTemp { get; set; }

    [JsonProperty("FFR")]
    public double Ffr { get; set; }

    [JsonProperty("K")]
    public double K { get; set; }
}

public partial class PresetParametersModel
{
    public static List<PresetParametersModel> FromJson(string json) => JsonConvert.DeserializeObject<List<PresetParametersModel>>(json, Converter.Settings);
}

public static class Serialize
{
    public static string ToJson(this List<PresetParametersModel> self) => JsonConvert.SerializeObject(self, Converter.Settings);
}

internal static class Converter
{
    public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
    {
        MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
        DateParseHandling = DateParseHandling.None,
        Converters =
            {
                TypeEnumConverter.Singleton,
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
    };
}

internal class TypeEnumConverter : JsonConverter
{
    public override bool CanConvert(Type t) => t == typeof(FilamentTypes) || t == typeof(FilamentTypes?);

    public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
    {
        if (reader.TokenType == JsonToken.Null) return null;
        var value = serializer.Deserialize<string>(reader);
        switch (value)
        {
            case "PETG":
                return FilamentTypes.PETG;
            case "PLA":
                return FilamentTypes.PLA;
            case "ABS":
                return FilamentTypes.ABS;
            case "ABS_GF":
                return FilamentTypes.ABS_GF;
            case "PET_CF":
                return FilamentTypes.PET_CF;
            case "PA_CF":
                return FilamentTypes.PA_CF;
            case "PC":
                return FilamentTypes.PC;
            case "ASA":
                return FilamentTypes.ASA;
            case "ASA_CF":
                return FilamentTypes.ASA_CF;
            case "ASA_AERO":
                return FilamentTypes.ASA_AERO;
            case "PA6_CF":
                return FilamentTypes.PA6_CF;
            case "PA_GF":
                return FilamentTypes.PA_GF;
            case "PETG_CF":
                return FilamentTypes.PETG_CF;
            case "PLA_AERO":
                return FilamentTypes.PLA_AERO;
            case "PLA_CF":
                return FilamentTypes.PLA_CF;
            case "PPS":
                return FilamentTypes.PPS;
            case "PPA_CF":
                return FilamentTypes.PPA_CF;
            case "PPA_GF":
                return FilamentTypes.PPA_GF;
            case "PPS_CF":
                return FilamentTypes.PPS_CF;
            case "BVOH":
                return FilamentTypes.BVOH;
            case "PVA":
                return FilamentTypes.PVA;
            case "PA":
                return FilamentTypes.PA;
            case "TPU":
                return FilamentTypes.TPU;
            case "TPU_AMS":
                return FilamentTypes.TPU_AMS;
            case "EVA":
                return FilamentTypes.EVA;
            case "HIPS":
                return FilamentTypes.HIPS;
            case "PCTG":
                return FilamentTypes.PCTG;
            case "PE_CF":
                return FilamentTypes.PE_CF;
            case "PHA":
                return FilamentTypes.PHA;
            case "PP_CF":
                return FilamentTypes.PP_CF;
            case "PP":
                return FilamentTypes.PP;
            case "PP_GF":
                return FilamentTypes.PP_GF;
            default:
                return FilamentTypes.Unknown; // Assuming you have an Unknown type or handle it accordingly
        }
        throw new Exception("Cannot unmarshal type TypeEnum");
    }

    public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
    {
        if (untypedValue == null)
        {
            serializer.Serialize(writer, null);
            return;
        }
        var value = (FilamentTypes)untypedValue;
        serializer.Serialize(writer, value.ToString());
    }

    public static readonly TypeEnumConverter Singleton = new TypeEnumConverter();
}