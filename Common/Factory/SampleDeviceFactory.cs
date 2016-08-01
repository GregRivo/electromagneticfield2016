using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Microsoft.Azure.Devices.Applications.RemoteMonitoring.Common.DeviceSchema;
using Microsoft.Azure.Devices.Applications.RemoteMonitoring.Common.Models;

namespace Microsoft.Azure.Devices.Applications.RemoteMonitoring.Common.Factory
{
    public static class SampleDeviceFactory
    {
        public const string OBJECT_TYPE_DEVICE_INFO = "DeviceInfo";

        public const string VERSION_1_0 = "1.0";

        private static Random rand = new Random();

        private const int MAX_COMMANDS_SUPPORTED = 6;

        private const bool IS_SIMULATED_DEVICE = true;

        private static List<string> DefaultDeviceNames = new List<string>{
            "SampleDevice001", 
            "SampleDevice002", 
            "SampleDevice003", 
            "SampleDevice004"
        };

        private class Location
        {
            public double Latitude { get; set; }
            public double Longitude { get; set; }
            
            public Location(double latitude, double longitude)
            {
                Latitude = latitude;
                Longitude = longitude;
            }

        }

        // Random locations within Losely Park, Guildford
        private static List<Location> _possibleDeviceLocations = new List<Location>{
            new Location(51.216401, -0.604197),
            new Location(51.217036, -0.604940),
            new Location(51.216673, -0.605981),
            new Location(51.215840, -0.605048),
            new Location(51.215887, -0.603889),
            new Location(51.215961, -0.602977),
            new Location(51.215874, -0.602441),
            new Location(51.216190, -0.602023),
            new Location(51.216022, -0.601165),
            new Location(51.215693, -0.601326),
            new Location(51.215229, -0.601519),
            new Location(51.215021, -0.601809),
            new Location(51.214571, -0.602463),
            new Location(51.214403, -0.603547),
            new Location(51.214369, -0.604405),
            new Location(51.214174, -0.605489)
        };

        public static dynamic GetSampleSimulatedDevice(string deviceId, string key)
        {
            dynamic device = DeviceSchemaHelper.BuildDeviceStructure(deviceId, true, null);

            AssignDeviceProperties(deviceId, device);
            device.ObjectType = OBJECT_TYPE_DEVICE_INFO;
            device.Version = VERSION_1_0;
            device.IsSimulatedDevice = IS_SIMULATED_DEVICE;

            AssignTelemetry(device);
            AssignCommands(device);

            return device;
        }

        public static dynamic GetSampleDevice(Random randomNumber, SecurityKeys keys)
        {
            string deviceId = 
                string.Format(
                    CultureInfo.InvariantCulture,
                    "00000-DEV-{0}C-{1}LK-{2}D-{3}",
                    MAX_COMMANDS_SUPPORTED, 
                    randomNumber.Next(99999),
                    randomNumber.Next(99999),
                    randomNumber.Next(99999));

            dynamic device = DeviceSchemaHelper.BuildDeviceStructure(deviceId, false, null);
            device.ObjectName = "IoT Device Description";

            AssignDeviceProperties(deviceId, device);
            AssignTelemetry(device);
            AssignCommands(device);

            return device;
        }

        private static void AssignDeviceProperties(string deviceId, dynamic device)
        {
            int randomId = rand.Next(0, _possibleDeviceLocations.Count - 1); 
            dynamic deviceProperties = DeviceSchemaHelper.GetDeviceProperties(device);
            deviceProperties.HubEnabledState = true;
            deviceProperties.BatteryLevel = randomId * 6 + "%";
            deviceProperties.BatteryVoltage = "3." + randomId + "V";
            deviceProperties.Temperature = 20 + (randomId / 2) + "°C";

            // Choose a location among the 16 above and set Lat and Long for device properties
            deviceProperties.Latitude = _possibleDeviceLocations[randomId].Latitude;
            deviceProperties.Longitude = _possibleDeviceLocations[randomId].Longitude;
        }

        private static void AssignTelemetry(dynamic device)
        {
            dynamic telemetry = CommandSchemaHelper.CreateNewTelemetry("Temperature", "Temperature", "double");
            CommandSchemaHelper.AddTelemetryToDevice(device, telemetry);

            //telemetry = CommandSchemaHelper.CreateNewTelemetry("Humidity", "Humidity", "double");
            //CommandSchemaHelper.AddTelemetryToDevice(device, telemetry);
        }

        private static void AssignCommands(dynamic device)
        {
            dynamic command = CommandSchemaHelper.CreateNewCommand("PingDevice");
            CommandSchemaHelper.AddCommandToDevice(device, command);
            
            command = CommandSchemaHelper.CreateNewCommand("StartTelemetry");
            CommandSchemaHelper.AddCommandToDevice(device, command);
            
            command = CommandSchemaHelper.CreateNewCommand("StopTelemetry");
            CommandSchemaHelper.AddCommandToDevice(device, command);
            
            command = CommandSchemaHelper.CreateNewCommand("ChangeSetPointTemp");
            CommandSchemaHelper.DefineNewParameterOnCommand(command, "SetPointTemp", "double");
            CommandSchemaHelper.AddCommandToDevice(device, command);
            
            command = CommandSchemaHelper.CreateNewCommand("DiagnosticTelemetry");
            CommandSchemaHelper.DefineNewParameterOnCommand(command, "Active", "boolean");
            CommandSchemaHelper.AddCommandToDevice(device, command);
            
            command = CommandSchemaHelper.CreateNewCommand("ChangeDeviceState");
            CommandSchemaHelper.DefineNewParameterOnCommand(command, "DeviceState", "string");
            CommandSchemaHelper.AddCommandToDevice(device, command);
        }

        public static List<string> GetDefaultDeviceNames()
        {
            long milliTime = DateTime.Now.Millisecond;
            return DefaultDeviceNames.Select(r => string.Concat(r, "_" + milliTime)).ToList();
        }
    }
}
