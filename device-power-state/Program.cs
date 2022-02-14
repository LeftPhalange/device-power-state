using System;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;
using System.IO;
using System.ComponentModel;

namespace device_power_state
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("device_power_state (arguments [both are required]: deviceId, timeoutInSeconds)");
                return;
            }
            var deviceId = args[0];
            var secs = int.Parse(args[1]);
            var currentState = SystemInformation.PowerStatus.PowerLineStatus == PowerLineStatus.Offline;
            
            /* Start loop to detect power line status and toggle device status */
            while (true)
            {
                bool isRunningOnBattery = (SystemInformation.PowerStatus.PowerLineStatus == PowerLineStatus.Offline);
                
                // If current state is the same at the next iteration, leave it alone
                if (currentState == isRunningOnBattery)
                {
                    Thread.Sleep(secs);
                    continue;
                }

                // Toggle device based on PowerLineStatus
                AppendText($"Is charging: {SystemInformation.PowerStatus.PowerLineStatus}");
                currentState = ToggleDevice(deviceId, isRunningOnBattery);

                Thread.Sleep(TimeSpan.FromSeconds(secs));
            }
        }

        static void AppendText(string Text)
           => Console.WriteLine(string.Format("[{0} {1}] {2}", DateTime.Now.ToShortDateString(), DateTime.Now.ToLongTimeString(), Text));

        static bool ToggleDevice(string hardwareId, bool enableDevice)
        {
            string argument = enableDevice ? "/enable-device" : "/disable-device";
            AppendText($"{(enableDevice ? $"Enabling device {hardwareId}." : $"Disabling device {hardwareId}.")}");
            try
            {
                Process.Start(new ProcessStartInfo()
                {
                    FileName = "pnputil",
                    Arguments = $"{argument} \"{hardwareId}\"",
                    CreateNoWindow = true,
                    UseShellExecute = false
                });
            }
            catch (Win32Exception ex)
            {
                AppendText($"Exception found: {ex.Message}");
            }
            return enableDevice;
        }
    }
}
