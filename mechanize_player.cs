using System;

namespace XRL.World.Parts
{
    [Serializable]
    public class Books_MechaPlayer : MechaPlayer
    {
        public override bool WantEvent(int ID, int cascade) => base.WantEvent(ID, cascade) || ID == ObjectCreatedEvent.ID;
        public override bool HandleEvent(ObjectCreatedEvent E)
        {
            bool val = base.HandleEvent(E);
            // Now we wish to change the original body's name, etc.
            MetricsManager.LogInfo("Swapping bodies!");
            GameObject thePlayer = IComponent<GameObject>.ThePlayer;
            var humanBody = thePlayer.Body;
            var mechBody = ParentObject.Body;
            MetricsManager.LogInfo("Grabbed body objects!");
            thePlayer.RemovePart(humanBody);
            ParentObject.RemovePart(mechBody);
            MetricsManager.LogInfo("Removed body objects!");
            thePlayer.AddPart(mechBody);
            ParentObject.AddPart(humanBody);
            MetricsManager.LogInfo("Swapped body objects!");
            return val;
        }
    }
}
