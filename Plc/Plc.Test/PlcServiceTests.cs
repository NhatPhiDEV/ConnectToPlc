using Moq;
using Plc.Command;
using S7.Net;

namespace Plc.Test;

public class PlcServiceTests
{
    private readonly Mock<IPlcService> _plcServiceMock;

    public PlcServiceTests()
    {
        _plcServiceMock = new Mock<IPlcService>();
    }

    [Fact]
    public void Connect_ShouldReturnTrue_WhenConnectionIsSuccessful()
    {
        // Arrange
        _plcServiceMock.Setup(service => service.Connect()).Returns((true, string.Empty));

        // Act
        var result = _plcServiceMock.Object.Connect();

        // Assert
        Assert.True(result.Item1);
        Assert.Equal(string.Empty, result.Item2);
    }

    [Fact]
    public void Disconnect_ShouldCallDisconnectMethod()
    {
        // Arrange
        _plcServiceMock.Setup(service => service.Disconnect());

        // Act
        _plcServiceMock.Object.Disconnect();

        // Assert
        _plcServiceMock.Verify(service => service.Disconnect(), Times.Once);
    }

    [Fact]
    public async Task ReadAsync_ShouldReturnExpectedResult()
    {
        // Arrange
        var expectedValue = new object();
        _plcServiceMock.Setup(service => service.ReadAsync(It.IsAny<string>())).ReturnsAsync(expectedValue);

        // Act
        var result = await _plcServiceMock.Object.ReadAsync("someAddress");

        // Assert
        Assert.Equal(expectedValue, result);
    }

    [Fact]
    public async Task ReadBytesAsync_ShouldReturnExpectedBytes()
    {
        // Arrange
        var expectedBytes = new byte[] { 1, 2, 3 };
        _plcServiceMock.Setup(service => service.ReadBytesAsync(It.IsAny<DataType>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(expectedBytes);

        // Act
        var result = await _plcServiceMock.Object.ReadBytesAsync(DataType.DataBlock, 1, 0, 3);

        // Assert
        Assert.Equal(expectedBytes, result);
    }

    [Fact]
    public async Task In_ShouldReturnExpectedResult()
    {
        // Arrange
        var expectedResult = (true, "Success");
        _plcServiceMock.Setup(service => service.In(It.IsAny<object>(), It.IsAny<object>(), It.IsAny<object>(), It.IsAny<object>())).ReturnsAsync(expectedResult);

        // Act
        var result = await _plcServiceMock.Object.In(new object(), new object(), new object(), new object());

        // Assert
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public async Task Out_ShouldReturnExpectedResult()
    {
        // Arrange
        var expectedResult = (true, "Success");
        _plcServiceMock.Setup(service => service.Out(It.IsAny<object>(), It.IsAny<object>(), It.IsAny<object>(), It.IsAny<object>())).ReturnsAsync(expectedResult);

        // Act
        var result = await _plcServiceMock.Object.Out(new object(), new object(), new object(), new object());

        // Assert
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public async Task Transfer_ShouldReturnExpectedResult()
    {
        // Arrange
        var expectedResult = (true, "Success");
        _plcServiceMock.Setup(service => service.Transfer(It.IsAny<object>(), It.IsAny<object>(), It.IsAny<object>(), It.IsAny<object>(), It.IsAny<object>(), It.IsAny<object>(), It.IsAny<object>())).ReturnsAsync(expectedResult);

        // Act
        var result = await _plcServiceMock.Object.Transfer(new object(), new object(), new object(), new object(), new object(), new object(), new object());

        // Assert
        Assert.Equal(expectedResult, result);
    }
}