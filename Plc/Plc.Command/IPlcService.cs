namespace Plc.Command;
public interface IPlcService
{
    (bool, string) Connect();
    void Disconnect();
    Task<object?> ReadAsync(string address);
    Task<byte[]> ReadBytesAsync(S7.Net.DataType dataType, int db, int startByteAdr, int count);
    Task<(bool, string)> In(object rack, object pointX, object pointY, object pointZ);
    Task<(bool, string)> Out(object rack, object pointX, object pointY, object pointZ);
    Task<(bool, string)> Transfer(
        object rack,
        object fromX,
        object fromY,
        object fromZ,
        object toX,
        object toY,
        object toZ);
}
