# HP8273BLLMTest

A .NET Framework 4.7.2 command-line tool for remotely controlling the **HP 8673B Synthesized Signal Generator** over GPIB (HP-IB) using the National Instruments VISA and NI-488.2 libraries.

---

## The HP 8673B Synthesized Signal Generator

The HP 8673B is a high-performance microwave signal generator made by Hewlett-Packard, covering a frequency range of **2.0 to 26.0 GHz** (with an overrange of 1.95 to 26.5 GHz). It was designed for use in automated test systems and laboratories requiring a stable, calibrated RF/microwave source.

Key characteristics:
- **Frequency range:** 2.0 – 26.0 GHz (1.95 – 26.5 GHz overrange)
- **Frequency resolution:** 10 Hz
- **Output level:** +8 dBm to -100 dBm (frequency and option dependent)
- **Modulation:** AM, FM, and pulse modulation
- **Remote control:** Full HP-IB (GPIB / IEEE-488.2) programmability
- **Default GPIB address:** 19

The 8673B uses HP's proprietary HP-IB command language (not SCPI). Commands are short ASCII strings sent over the GPIB bus — for example `FR10GZ` to set 10 GHz, or `LE-10DM` to set -10 dBm.

Although HP's test and measurement division became Agilent Technologies and later Keysight Technologies, the 8673B remains a popular instrument in labs due to its wide frequency coverage and reliable performance.

---

## Prerequisites

### Hardware
- An NI GPIB interface card or USB-GPIB adapter (e.g. GPIB-USB-HS)
- The HP 8673B connected to the GPIB bus at address **19** (factory default)

### Software
- [NI-VISA](#ni-visa) — National Instruments Virtual Instrument Software Architecture
- [NI-488.2](#ni-4882) — National Instruments GPIB driver

Both are installed as part of **NI-VISA** from the National Instruments website (see below).

---

## Commands

Run the application from the command line. Use `--help` at any point for usage information.

```
HP8273BLLMTest --help
HP8273BLLMTest frequency --help
HP8273BLLMTest power --help
```

### `frequency` — Set the CW Output Frequency

Sets the continuous wave (CW) output frequency of the HP 8673B.

```
HP8273BLLMTest frequency <frequency_ghz> [--address <gpib_address>]
```

| Argument / Option | Description |
|---|---|
| `<frequency_ghz>` | Frequency in GHz (e.g. `10.5` for 10.5 GHz) |
| `--address` / `-a` | GPIB address of the instrument. Default: `19` |

**Examples:**
```
HP8273BLLMTest frequency 10.5
HP8273BLLMTest frequency 2.0
HP8273BLLMTest frequency 26.0 --address 19
```

Sends HP-IB command: `FR<value>GZ`

---

### `power` — Set the RF Output Power Level

Sets the RF output power level of the HP 8673B.

```
HP8273BLLMTest power <power_dbm> [--address <gpib_address>]
```

| Argument / Option | Description |
|---|---|
| `<power_dbm>` | Output power in dBm (e.g. `-10` for -10 dBm) |
| `--address` / `-a` | GPIB address of the instrument. Default: `19` |

**Examples:**
```
HP8273BLLMTest power -10
HP8273BLLMTest power 0
HP8273BLLMTest power -50 --address 19
```

Sends HP-IB command: `LE<value>DM`

---

## NI-VISA

**NI-VISA** (National Instruments Virtual Instrument Software Architecture) is NI's implementation of the VISA standard — a widely used I/O API for communicating with test and measurement instruments over buses including GPIB, USB, Ethernet, Serial, and PXI.

This project references two VISA-related assemblies:

### `NationalInstruments.Visa.dll`
The NI .NET implementation of the VISA standard. Provides the `ResourceManager` and `MessageBasedSession` classes used to open a connection to the instrument and send/receive commands.

- **Location (after NI-VISA install):**
  `C:\Program Files (x86)\IVI Foundation\VISA\Microsoft.NET\Framework32\v4.0.30319\NI VISA.NET 26.0\`
- **Download:** Available as part of the **NI-VISA** driver package from the NI website:
  https://www.ni.com/en/support/downloads/drivers/download.ni-visa.html

### `Ivi.Visa.dll`
The vendor-neutral **IVI Foundation VISA.NET Shared Components** — a set of standard interfaces (`IResourceManager`, `IMessageBasedSession`, etc.) that NI-VISA implements. Including this allows code to be written against the standard interface rather than the NI-specific implementation.

- **Location (after NI-VISA install):**
  `C:\Program Files (x86)\IVI Foundation\VISA\Microsoft.NET\Framework32\v4.0.30319\VISA.NET Shared Components 8.0.2\`
- **More information:** https://www.ivifoundation.org/specifications/default.aspx

---

## NI-488.2

**NI-488.2** is National Instruments' driver for the IEEE-488.2 standard (commonly known as GPIB — General Purpose Interface Bus). It provides low-level GPIB bus control and is required for any NI GPIB hardware (cards, USB adapters, etc.) to function.

### `NationalInstruments.NI4882.dll`
The .NET wrapper around the NI-488.2 driver. Exposes GPIB bus operations at a lower level than VISA — useful if you need direct control of bus events, addressing, or service requests beyond what VISA exposes.

- **Location (after NI-488.2 install):**
  `C:\Program Files (x86)\National Instruments\MeasurementStudioVS2012\DotNET\Assemblies\Current\`
- **Download:** Available as part of the **NI-488.2** driver package:
  https://www.ni.com/en/support/downloads/drivers/download.ni-488-2.html

> **Note:** NI-VISA and NI-488.2 are typically installed together. Installing NI-VISA will usually install the NI-488.2 driver as a dependency. The recommended starting point is to install NI-VISA from the link above.

---

## Building

This project requires Visual Studio 2019/2022 or MSBuild with the .NET Framework 4.7.2 targeting pack installed. The `dotnet` CLI does not support .NET Framework targets.

```
msbuild HP8273BLLMTest.csproj /p:Configuration=Debug
```

Or use the included VS Code tasks: press **Ctrl+Shift+B** to build.

---

## Dependencies

| Package | Version | Purpose |
|---|---|---|
| [Spectre.Console.Cli](https://spectreconsole.net/cli) | 0.53.1 | Command-line interface framework |
| NationalInstruments.Visa | 26.0 | NI-VISA .NET API |
| Ivi.Visa | 8.0.2 | IVI Foundation VISA.NET standard interfaces |
| NationalInstruments.NI4882 | 13.0 | NI-488.2 GPIB .NET API |
