# device-power-state

This is a simple .NET Framework 4.0/C# application that disables or enables a device driver, given a hardware instance ID from PnPUtil, in Windows based on the power state. Useful for disabling integrated graphics when running on A/C power to leverage discrete graphics fully.

## How it works
* This application detects the power state (SystemInformation.PowerStatus.PowerLineStatus) every X seconds.
* If the PowerLineStatus is Offline (or is on battery power), it will enable or re-enable the specified device driver.
* Conversely, if the PowerLineStatus is Online (or is on A/C power), it will disable the specified device driver.

## Syntax (not including brackets, both required):
`device-power-state [deviceId] [timeoutInSeconds]`
* deviceId: The Instance ID found in PnPUtil when running this command: `pnputil /enum-devices /connected`
* timeoutInSeconds: How long the cycles should be in between detecting the power state, measured in seconds

## Tested on
* Windows 10
* Windows 11
* Visual Studio Community 2019