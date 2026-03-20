using System;
using NationalInstruments.Visa;

namespace HP8273BLLMTest.Instruments
{
    /// <summary>
    /// Driver for the HP 8673B Synthesized Signal Generator (2.0 - 26.0 GHz).
    /// Communicates over HP-IB (GPIB) using NI-VISA.
    /// Command reference: HP 8673B Operating Manual, Table 3-6 HP-IB Program Codes.
    /// </summary>
    public sealed class HP8673B : IDisposable
    {
        private readonly MessageBasedSession _session;

        /// <summary>
        /// Opens a VISA session to the instrument.
        /// </summary>
        /// <param name="gpibAddress">GPIB address (factory default is 19).</param>
        public HP8673B(int gpibAddress = 19)
        {
            var rm = new ResourceManager();
            string resourceString = $"GPIB0::{gpibAddress}::INSTR";
            _session = (MessageBasedSession)rm.Open(resourceString);
            _session.TimeoutMilliseconds = 5000;
        }

        /// <summary>
        /// Sets the CW output frequency.
        /// Valid range: 2.0 – 26.0 GHz (1.95 – 26.5 GHz overrange). Resolution: 10 Hz.
        /// HP-IB command: FR&lt;value&gt;GZ
        /// </summary>
        /// <param name="frequencyGHz">Frequency in GHz.</param>
        public void SetFrequency(double frequencyGHz)
        {
            if (frequencyGHz < 1.95 || frequencyGHz > 26.5)
                throw new ArgumentOutOfRangeException(nameof(frequencyGHz),
                    "Frequency must be between 1.95 and 26.5 GHz.");

            // Format with enough decimal places for 10 Hz resolution at GHz scale (8 d.p.)
            string command = $"FR{frequencyGHz:F8}GZ";
            Send(command);
        }

        /// <summary>
        /// Sets the RF output power level.
        /// Valid range: +8 dBm to -100 dBm (instrument dependent).
        /// HP-IB command: LE&lt;value&gt;DM
        /// </summary>
        /// <param name="powerDbm">Output power in dBm.</param>
        public void SetPower(double powerDbm)
        {
            if (powerDbm < -100.0 || powerDbm > 13.0)
                throw new ArgumentOutOfRangeException(nameof(powerDbm),
                    "Power must be between -100.0 and +13.0 dBm.");

            // LE sets level directly; DM = dBm units terminator
            string command = $"LE{powerDbm:F1}DM";
            Send(command);
        }

        private void Send(string command)
        {
            _session.FormattedIO.WriteLine(command);
        }

        public void Dispose()
        {
            _session?.Dispose();
        }
    }
}
