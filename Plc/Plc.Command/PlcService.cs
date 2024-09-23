using Microsoft.Extensions.Options;
using S7.Net;
using System.Diagnostics;

namespace Plc.Command;
public sealed class PlcService : IPlcService
{
    private readonly PlcOptions _plcOptions;
    private readonly S7.Net.Plc _plc;

    public PlcService(IOptions<PlcOptions> plcModel)
    {
        _plcOptions = plcModel.Value;

        _plc = new S7.Net.Plc(
            _plcOptions.GetCpuType(),
            _plcOptions.IpAddress,
            _plcOptions.Rack,
            _plcOptions.Slot);

        Connect();
    }

    public (bool, string) Connect()
    {
        try
        {
            if (_plc.IsConnected)
            {
                return (true, "Already connected.");
            }

            _plc.Open();

            if (_plc.IsConnected)
            {
                return (true, "Connected successfully.");
            }
            else
            {
                return (false, "Failed to connect.");
            }
        }
        catch (Exception ex)
        {
            return (false, $"Can't connect to PLC, Exception: {ex.Message}");
        }
    }

    public void Disconnect()
    {
        if (_plc.IsConnected)
        {
            _plc.Close();
        }
    }

    public async Task<(bool, string)> In(object rack, object pointX, object pointY, object pointZ)
    {
        var (isConnected, message) = Reconnect();

        if (isConnected is false)
        {
            return (isConnected, message);
        }

        try
        {          
            await SetToLocation(rack, pointX, pointY, pointZ);

            await _plc.WriteAsync(_plcOptions.Address.In, 1);

            return await GetStatusRunning();
        }
        catch (Exception ex)
        {
            return (false, ex.Message);
        }
    }

    public async Task<(bool, string)> Out(
        object rack,
        object pointX,
        object pointY,
        object pointZ)
    {
        var (isConnected, message) = Reconnect();

        if (isConnected is false)
        {
            return (isConnected, message);
        }

        try
        {
            await SetToLocation(rack, pointX, pointY, pointZ);

            await _plc.WriteAsync(_plcOptions.Address.Out, 1);

            return await GetStatusRunning();
        }
        catch (Exception ex)
        {
            return (false, ex.Message);
        }
    }

    public async Task<(bool, string)> GetStatusRunning()
    {
        var stopwatch = Stopwatch.StartNew();

        while (stopwatch.Elapsed < _plcOptions.TimeOut)
        {
            var dataReceive = await _plc.ReadBytesAsync(DataType.DataBlock, 26, 0, 19);
            var status = dataReceive[18].SelectBit(2);
            if (status)
            {
                return (true, string.Empty);
            }
            await Task.Delay(100); // Wait for 100 milliseconds before checking again
        }

        return (false, "Timeout");
    }

    /// <summary>
    /// Read data from PLC using address
    /// </summary>
    public async Task<object?> ReadAsync(string address)
    {
        var (isConnected, message) = Reconnect();

        if (isConnected is false)
        {
            throw new ArgumentException(message);
        }

        try
        {
            return await _plc.ReadAsync(address);
        }
        catch (Exception)
        {
            return null;
        }
    }

    /// <summary>
    /// Read bytes from PLC
    /// </summary>
    public async Task<byte[]> ReadBytesAsync(
        DataType dataType,
        int db,
        int startByteAdr,
        int count)
    {
        var (isConnected, message) = Reconnect();

        if (isConnected is false)
        {
            throw new ArgumentException(message);
        }

        try
        {
            return await _plc.ReadBytesAsync(dataType, db, startByteAdr, count);
        }
        catch (Exception)
        {
            return [];
        }
    }

    public async Task<(bool, string)> Transfer(
        object rack,
        object fromX,
        object fromY,
        object fromZ,
        object toX,
        object toY,
        object toZ)
    {
        var (isConnected, message) = Reconnect();

        if (isConnected is false)
        {
            return (isConnected, message);
        }

        try
        {
            await SetFromLocation(rack, fromX, fromY, fromZ);
            await SetToLocation(rack, toX, toY, toZ);
            await _plc.WriteAsync(_plcOptions.Address.Transfer, 1);

            return await GetStatusRunning();
        }
        catch (Exception ex)
        {
            return (false, ex.Message);
        }
    }

    private async Task SetFromLocation(object rack, object pointX, object pointY, object pointZ)
    {
        Int16 x = ToInt16(pointX);
        Int16 y = ToInt16(rack);
        Int16 z = ToInt16(pointZ);

        if (x <= 0)
            throw new ArgumentException("Invalid point X");
        if (y <= 0)
            throw new ArgumentException("Invalid rack");
        if (z <= 0)
            throw new ArgumentException("Invalid point Z");

        await _plc.WriteAsync(_plcOptions.Address.FromX, x);
        await _plc.WriteAsync(_plcOptions.Address.FromY, y);
        await _plc.WriteAsync(_plcOptions.Address.FromZ, z);
    }

    private async Task SetToLocation(object rack, object pointX, object pointY, object pointZ)
    {
        Int16 x = ToInt16(pointX);
        Int16 y = ToInt16(rack);
        Int16 z = ToInt16(pointZ);

        if (x <= 0)
            throw new ArgumentException("Invalid point X");
        if (y <= 0)
            throw new ArgumentException("Invalid rack");
        if (z <= 0)
            throw new ArgumentException("Invalid point Z");

        await _plc.WriteAsync(_plcOptions.Address.ToX, x);
        await _plc.WriteAsync(_plcOptions.Address.ToY, y);
        await _plc.WriteAsync(_plcOptions.Address.ToZ, z);
    }

    /// <summary>
    /// Convert object to Int16
    /// </summary>
    /// <param name="value">input value</param>
    /// <returns>result type Int16</returns>
    private static Int16 ToInt16(object value)
        => Int16.TryParse(value.ToString(), out var result) ? result : default;

    private (bool, string) Reconnect()
    {
        // If not connected or disconnected, try to connect
        if (_plc.IsConnected == false)
        {
            return Connect();
        }
        return (true, "Connected successfully.");
    }

}
