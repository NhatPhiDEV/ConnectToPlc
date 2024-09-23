# Hướng dẫn cách thiết lập

## Bước 1: Thêm phần PlcConfig vào appsettings.json

Sao chép đoạn mã sau và dán vào tệp `appsettings.json` của bạn:
```
{
  "PlcConfig": {
    "CpuType": "S71200",
    "IpAddress": "192.168.0.13",
    "Rack": 0,
    "Slot": 0,
    "Address": {
      "Correct": "DB26.DBX18.0",
      "Incorrect": "DB26.DBX18.1",
      "Barcode": "DB26.DBD14",
      "In": "DB26.DBX0.0",
      "Out": "DB26.DBX0.1",
      "Transfer": "DB26.DBX0.2",
      "FromX": "DB26.DBW2",
      "FromY": "DB26.DBW4",
      "FromZ": "DB26.DBW6",
      "ToX": "DB26.DBW8",
      "ToY": "DB26.DBW10",
      "ToZ": "DB26.DBW12"
    },
    "TimeOut": "00:05:00"
  }
}

```
## Bước 2: Cấu hình Dependency Injection (DI)

Thêm cấu hình AddPlcCommand cho cấu hình DI nơi bạn muốn sử dụng và Inject interface IPlcService vào để gọi
đến các phương thức:
```
public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
{
    // Cấu hình PlcCommand
    services.AddPlcCommand(configuration);

    // Các dịch vụ khác
}
```

## Mô tả các hàm trong PlcService

### `Connect()`

Kết nối với PLC. Nếu đã kết nối, trả về thông báo "Already connected". Nếu kết nối thành công, trả về thông báo "Connected successfully". Nếu không, trả về thông báo lỗi.

### `Disconnect()`

Ngắt kết nối với PLC.

### `In(object rack, object pointX, object pointY, object pointZ)`

Xử lý nhập kho. Trả về trạng thái và thông báo.

### `Out(object rack, object pointX, object pointY, object pointZ)`

Xử lý xuất kho. Trả về trạng thái và thông báo.

### `GetStatusRunning()`

Kiểm tra trạng thái thực thi của PLC trong khoảng thời gian `TimeOut`. Trả về trạng thái và thông báo.

### `ReadAsync(string address)`

Đọc dữ liệu từ PLC thông qua địa chỉ.

### `ReadBytesAsync(DataType dataType, int db, int startByteAdr, int count)`

Đọc dữ liệu dạng byte từ PLC. Trả về mảng byte đọc được.

### `Transfer(object rack, object fromX, object fromY, object fromZ, object toX, object toY, object toZ)`

Xử lý điều chuyển trong kho. Trả về trạng thái và thông báo.

### `SetFromLocation(object rack, object pointX, object pointY, object pointZ)`

Thiết lập vị trí bắt đầu cho quá trình di chuyển (sử dụng khi transfer).

### `SetToLocation(object rack, object pointX, object pointY, object pointZ)`

Thiết lập vị trí kết thúc cho quá trình di chuyển.

### `ToInt16(object value)`

Chuyển đổi giá trị sang kiểu `Int16`. Trả về giá trị `Int16` hoặc giá trị mặc định nếu chuyển đổi thất bại.

### `Reconnect()`

Kiểm tra kết nối và kết nối lại nếu cần thiết.

## Hoàn thành