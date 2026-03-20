using System.ComponentModel;
using System.Threading;
using HP8273BLLMTest.Instruments;
using Spectre.Console;
using Spectre.Console.Cli;

namespace HP8273BLLMTest.Commands
{
    public sealed class FrequencyCommand : Command<FrequencyCommand.Settings>
    {
        public sealed class Settings : CommandSettings
        {
            [CommandArgument(0, "<frequency_ghz>")]
            [Description("Output frequency in GHz (e.g. 10.5). Range: 1.95 – 26.5 GHz.")]
            public double FrequencyGHz { get; set; }

            [CommandOption("--address|-a <address>")]
            [DefaultValue(19)]
            [Description("GPIB address of the instrument. Default: 19.")]
            public int GpibAddress { get; set; }
        }

        public override int Execute(CommandContext context, Settings settings, CancellationToken cancellationToken)
        {
            AnsiConsole.MarkupLine($"[bold]HP 8673B[/] — Setting frequency to [cyan]{settings.FrequencyGHz:F6} GHz[/] (GPIB address [yellow]{settings.GpibAddress}[/])");

            using (var instrument = new HP8673B(settings.GpibAddress))
            {
                instrument.SetFrequency(settings.FrequencyGHz);
            }

            AnsiConsole.MarkupLine("[green]Done.[/]");
            return 0;
        }
    }
}
