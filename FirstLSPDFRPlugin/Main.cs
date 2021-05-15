using LSPD_First_Response.Mod.API;
using Rage;

namespace FirstLSPDFRPlugin
{
    public class Main : Plugin
    {
        // when plugin is initialized
        public override void Initialize()
        {
            Functions.OnOnDutyStateChanged += OnOnDutyStateChangedHandler;
            Game.LogTrivial("First LSPDFR Plugin by Pranav Rao has been loaded.");
        }
        
        // when plugin is terminated
        public override void Finally()
        {
            Game.LogTrivial("First LSPDFR Plugin has been cleaned up.");
        }

        private void OnOnDutyStateChangedHandler(bool onduty)
        {
            if (onduty)
            {
                RegisterCallouts();
            }
        }

        private void RegisterCallouts()
        {
            Functions.RegisterCallout(typeof(Callouts.StolenVehicle));
        }

    }
}