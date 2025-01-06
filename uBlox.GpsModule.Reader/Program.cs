using System;
using System.Globalization;
using System.IO.Ports;
using System.Threading;

namespace uBlox.GpsModule.Reader
{
    public class Program
    {
        private static SerialPort _serialPort;

        private static void Main(string[] args)
        {
            _serialPort = new SerialPort
            {
                PortName = "COM5",
                StopBits = StopBits.One,
                Parity = Parity.None,
                Handshake = Handshake.None
            };

            try
            {
                _serialPort.Open();
                Console.WriteLine($"{DateTime.Now:s} => Successful connection to the GPS module.");
                _serialPort.DataReceived += GpsModuleOnDataReceived;
            }
            catch (Exception e)
            {
                Console.WriteLine($"{DateTime.Now:s} => An error occurred while establishing a connection to the GPS module.\n\t - Error Message: {e.Message}");
                _serialPort.Close();
                _serialPort.Dispose();
            }

            Console.ReadLine();
        }

        private static void GpsModuleOnDataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            if (!_serialPort.IsOpen || _serialPort.BytesToRead == 0)
                return;

            var gpsDataResult = _serialPort.ReadExisting();
            var gpsDataSplits = gpsDataResult.Split('\n');

            foreach (var line in gpsDataSplits)
            {
                if (!line.StartsWith("$GPGGA") && !line.StartsWith("$GPRMC"))
                    continue;

                Console.WriteLine(line);
                var parts = line.Split(',');

                switch (parts[0])
                {
                    case "$GPGGA":
                    case "$GPRMC":
                    {
                        if (parts.Length >= 5)
                        {
                            var rawLatitude = parts[3];
                            var rawLongitude = parts[5];

                            var latitude = ConvertToDecimalDegrees(rawLatitude);
                            var longitude = ConvertToDecimalDegrees(rawLongitude);

                            Console.WriteLine($"{DateTime.Now} => {latitude:F6}, {longitude:F6}");
                            Thread.Sleep(300);
                        }
                        break;
                    }
                }
            }
        }

        private static double ConvertToDecimalDegrees(string rawCoordinate)
        {
            if (string.IsNullOrWhiteSpace(rawCoordinate) || rawCoordinate == "N" || rawCoordinate == "E")
                return 0.0;

            var coordinate = double.Parse(rawCoordinate);
            var degrees = Math.Floor(coordinate / 100);
            var minutes = coordinate - degrees * 100;
            var decimalDegrees = degrees + minutes / 60;

            return decimalDegrees;
        }
    }
}