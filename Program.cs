using HP8273BLLMTest.Commands;
using Spectre.Console.Cli;

namespace HP8273BLLMTest
{
    class Program
    {
        static int Main(string[] args)
        {
            var app = new CommandApp();
            app.Configure(config =>
            {
                config.SetApplicationName("HP8273BLLMTest");

                config.AddCommand<FrequencyCommand>("frequency")
                      .WithDescription("Set the CW output frequency of the HP 8673B.")
                      .WithExample(new[] { "frequency", "10.5" })
                      .WithExample(new[] { "frequency", "2.0", "--address", "19" });

                config.AddCommand<PowerCommand>("power")
                      .WithDescription("Set the RF output power level of the HP 8673B.")
                      .WithExample(new[] { "power", "-10" })
                      .WithExample(new[] { "power", "0", "--address", "19" });
            });

            return app.Run(args);
        }
    }
}
