using Newtonsoft.Json;

public partial class Module
{
    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("activationCost")]
    public double ActivationCost { get; set; }

    [JsonProperty("activationTime")]
    public double ActivationTime { get; set; }

    [JsonProperty("reactivationDelay", NullValueHandling = NullValueHandling.Ignore)]
    public double ReactivationDelay { get; set; }

    [JsonProperty("online")]
    public bool IsOnline { get; set; }

    [JsonIgnore]
    private double _runningTimer = 0;

    [JsonIgnore]
    public bool isActive => _runningTimer > 0;

    public void ActivateModule() {
        _runningTimer = ActivationTime + ReactivationDelay;
    }

    public void Step(double stepSize) {
        _runningTimer -= stepSize;
    }
}