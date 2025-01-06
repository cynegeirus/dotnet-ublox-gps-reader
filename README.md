# uBlox GPS Module Reader

This repository contains a simple C# application that reads GPS data from a uBlox GPS module via a serial port. It processes the GPS NMEA sentences, particularly `$GPGGA` and `$GPRMC`, and converts the raw latitude and longitude values into decimal degrees.

## Requirements

- .NET Framework SDK
- A uBlox GPS module connected via a serial port (e.g., COM5)
- A serial-to-USB adapter (if using a USB GPS device)

## Application Logic

- The application attempts to connect to the GPS module via the serial port.
- It listens for GPS data on the specified serial port.
- The GPS data (NMEA sentences) is parsed, and the latitude and longitude values are extracted from `$GPGGA` and `$GPRMC` sentences.
- The raw coordinates are converted from the `DDMM.MMMM` format to decimal degrees and displayed on the console.

## Code Walkthrough

1. **Serial Port Configuration**:  
   The application uses the `System.IO.Ports.SerialPort` class to communicate with the GPS module. You need to specify the correct port name (e.g., COM5), baud rate, stop bits, parity, and handshake for the connection.

2. **Data Reception**:  
   When data is received from the GPS module, the `GpsModuleOnDataReceived` event handler is triggered. It reads the data from the serial port, splits it by newline characters, and checks for the `$GPGGA` and `$GPRMC` sentences.

3. **Coordinate Conversion**:  
   The `ConvertToDecimalDegrees` method is used to convert the raw latitude and longitude values (in the `DDMM.MMMM` format) into decimal degrees.

4. **Output**:  
   The coordinates are printed on the console in the following format:
   ```
   yyyy-MM-ddTHH:mm:ss => latitude, longitude
   ```

## Example Output

```bash
2025-01-06T12:30:45 => 37.774929, -122.419418
```

## Notes

- The `Thread.Sleep(300)` is added to throttle the data processing slightly. You can adjust or remove this delay depending on the performance requirements.
- Make sure that the GPS module is powered on and outputting NMEA sentences.
- The program works best with a direct connection to the GPS module. If using a USB-to-serial adapter, ensure the correct COM port is specified.

## License

This project is licensed under the [MIT License](LICENSE). See the license file for details.

## Issues, Feature Requests or Support

Please use the Issue > New Issue button to submit issues, feature requests or support issues directly to me. You can also send an e-mail to akin.bicer@outlook.com.tr.
