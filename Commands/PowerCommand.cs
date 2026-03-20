using System.ComponentModel;
using System.Threading;
using HP8273BLLMTest.Instruments;
using Spectre.Console;
using Spectre.Console.Cli;

namespace HP8273BLLMTest.Commands
{
    public sealed class PowerCommand : Command<PowerCommand.Settings>
    {
        public sealed class Settings : CommandSettings
        {
            [CommandArgument(0, "<power_dbm>")]
            [Description("Output power level in dBm (e.g. -10). Range: -100 to +13 dBm.")]
            public double PowerDbm { get; set; }

            [CommandOption("--address|-a <address>")]
            [DefaultValue(19)]
            [Description("GPIB address of the instrument. Default: 19.")]
            public int GpibAddress { get; set; }
        }

        public override int Execute(CommandContext context, Settings settings, CancellationToken cancellationToken)
        {
            AnsiConsole.MarkupLine($"[bold]HP 8673B[/] — Setting power to [cyan]{settings.PowerDbm:F1} dBm[/] (GPIB address [yellow]{settings.GpibAddress}[/])");

            using (var instrument = new HP8673B(settings.GpibAddress))
            {
                instrument.SetPower(settings.PowerDbm);
            }

            AnsiConsole.MarkupLine("[green]Done.[/]");
            return 0;
        }
    }
}
