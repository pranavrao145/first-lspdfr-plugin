using Rage;
using LSPD_First_Response.Mod.API;
using LSPD_First_Response.Mod.Callouts;

namespace FirstLSPDFRPlugin.Callouts
{
    [CalloutInfo("StolenVehicle", CalloutProbability.High)]
    
    public class StolenVehicle : Callout
    {
        private Ped Suspect;
        private Vehicle SuspectVehicle;
        private Vector3 SpawnPoint;
        private Blip SuspectBlip;
        private LHandle Pursuit;
        private bool PursuitCreated = false;

        public override void OnCalloutDisplayed()
        {
            SpawnPoint = World.GetNextPositionOnStreet(Game.LocalPlayer.Character.Position.Around(250f));
            
            ShowCalloutAreaBlipBeforeAccepting(SpawnPoint, 30f);
            AddMinimumDistanceCheck(20f, SpawnPoint);

            CalloutMessage = "Stolen Vehicle";
            CalloutPosition = SpawnPoint;
            
            Functions.PlayScannerAudioUsingPosition("WE_HAVE CRIME_GRAND_THEFT_AUTO IN_OR_ON_POSITION", SpawnPoint);

            base.OnCalloutDisplayed();
        }

        public override bool OnCalloutAccepted()
        {
            SuspectVehicle = new Vehicle("ZENTORNO", SpawnPoint) {IsPersistent = true};

            Suspect = SuspectVehicle.CreateRandomDriver();
            Suspect.IsPersistent = true;
            Suspect.BlockPermanentEvents = true;

            SuspectBlip = Suspect.AttachBlip();
            SuspectBlip.IsFriendly = false;

            Suspect.Tasks.CruiseWithVehicle(20f, VehicleDrivingFlags.Emergency);
            return base.OnCalloutAccepted();
        }

        public override void Process()
        {
            base.Process();

            if (!PursuitCreated && Game.LocalPlayer.Character.DistanceTo(Suspect.Position) < 30f)
            {
                Pursuit = Functions.CreatePursuit();
                Functions.AddPedToPursuit(Pursuit, Suspect);
                Functions.SetPursuitIsActiveForPlayer(Pursuit, true);
                PursuitCreated = true;
            }
            
            if (PursuitCreated && !Functions.IsPursuitStillRunning(Pursuit)) End();
        }

        public override void End()
        {
            base.End();
            if(Suspect.Exists()) Suspect.Dismiss();
            if(SuspectVehicle.Exists()) SuspectVehicle.Dismiss();
            if (SuspectBlip.Exists()) SuspectBlip.Delete();
        }
    }
}