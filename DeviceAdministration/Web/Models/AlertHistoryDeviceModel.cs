using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Microsoft.Azure.Devices.Applications.RemoteMonitoring.DeviceAdmin.Web.Models
{
    public class AlertHistoryDeviceModel
    {
        public string DeviceId
        {
            get;
            set;
        }

        public double Latitude
        {
            get;
            set;
        }

        public double Longitude
        {
            get;
            set;
        }

        public string BatteryLevel
        {
            get;
            set;
        }

        public string BatteryVoltage
        {
            get;
            set;
        }

        public string Temperature
        {
            get;
            set;
        }

        public AlertHistoryDeviceStatus Status
        {
            get;
            set;
        }
    }

    public enum AlertHistoryDeviceStatus
    {
        AllClear = 0,
        Caution,
        Critical
    }
}