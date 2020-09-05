using System.Linq;
using System.Collections.Generic;

using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

public partial class Ship
{
    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("capacitor")]
    public Capacitor Capacitor { get; set; }

    [JsonProperty("modules")]
    public Module[] Modules { get; set; }

    [JsonIgnore]
    public IEnumerable<Module> OnlineModules => Modules.Where(module => module.IsOnline);
}

public partial class Capacitor
{
    [JsonProperty("capacity")]
    public double MaxCapacity { get; set; }
    [JsonIgnore]
    public double CurrentCapacity { get; set; }
    [JsonIgnore]
    public double CapacityPercent => (CurrentCapacity / MaxCapacity) * 100;

    [JsonProperty("rechargeTime")]
    public double RechargeTime { get; set; }

    ///
    /// This is in GJ/s
    [JsonIgnore]
    public double CurrentRechargeRate {
        get {
            return ((10 * MaxCapacity) / RechargeTime) * (System.Math.Sqrt(CurrentCapacity/MaxCapacity) - (CurrentCapacity/MaxCapacity));
        }
    }

    public void Reset() {
        CurrentCapacity = MaxCapacity;
    }
}

public partial class Ship
{
    public static Ship FromJson(string json) => JsonConvert.DeserializeObject<Ship>(json, Converter.Settings);
}

public static class Serialize
{
    public static string ToJson(this Ship self) => JsonConvert.SerializeObject(self, Converter.Settings);
}

internal static class Converter
{
    public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
    {
        MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
        DateParseHandling = DateParseHandling.None,
        Converters =
        {
            new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
        },
    };
}