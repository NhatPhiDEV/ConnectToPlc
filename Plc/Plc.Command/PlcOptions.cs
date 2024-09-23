namespace Plc.Command;
public class PlcOptions
{
    /// <summary>
    /// Support S71200, S71500, S7300
    /// </summary>
    public string CpuType { get; set; } = string.Empty;

    /// <summary>
    /// TimeOut for connect or execute command to PLC
    /// </summary>
    public TimeSpan TimeOut { get; set; }

    /// <summary>
    /// IpAddress of PLC
    /// </summary>
    public string IpAddress { get; set; } = string.Empty;
    public short Rack { get; set; }
    public short Slot { get; set; }
    public Address Address { get; set; } = null!;

    /// <summary>
    /// Convert string to CpuType
    /// </summary>
    /// <returns></returns>
    /// <exception cref="ArgumentException">Invalid CPU type config</exception>
    public S7.Net.CpuType GetCpuType()
    {
        return CpuType switch
        {
            "S71200" => S7.Net.CpuType.S71200,
            "S71500" => S7.Net.CpuType.S71500,
            "S7300" => S7.Net.CpuType.S7300,
            _ => S7.Net.CpuType.S71200,
        };
    }
}

public class Address
{
    /// <summary>
    /// Case auto scan barcode (not implement)
    /// </summary>
    public string Correct { get; set; } = string.Empty;
    public string Incorrect { get; set; } = string.Empty;
    public string Barcode { get; set; } = string.Empty;

    /// <summary>
    /// From Point (X: Line, Y: Rack, Z: Row)
    /// Using: Transfer
    /// </summary>
    public string FromX { get; set; } = string.Empty;
    public string FromY { get; set; } = string.Empty;
    public string FromZ { get; set; } = string.Empty;

    /// <summary>
    /// To Point (X: Line, Y: Rack, Z: Row)
    /// Using: In, out and Transfer
    /// </summary>
    public string ToX { get; set; } = string.Empty;
    public string ToY { get; set; } = string.Empty;
    public string ToZ { get; set; } = string.Empty;

    // Command
    public string In { get; set; } = string.Empty;
    public string Out { get; set; } = string.Empty;
    public string Transfer { get; set; } = string.Empty;
}