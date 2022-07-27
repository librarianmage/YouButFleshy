using System;
using XRL.Core;

namespace XRL.World.Parts
{
    [Serializable]
    public class Books_FleshyPet : IPart
    {
        public string NamePrefix = "{{c|mechanical}}";

        public string DescriptionPostfix = "There is a low, persistent hum emanating outward.";

        public bool KeepNatural = true;

        public bool InheritDescription = true;

        public override bool SameAs(IPart P) => true;

        public override bool WantEvent(int ID, int cascade) =>
            base.WantEvent(ID, cascade) || ID == ObjectCreatedEvent.ID;

        public override bool HandleEvent(ObjectCreatedEvent E)
        {
            /* Copy player */

            GameObject thePlayer = IComponent<GameObject>.ThePlayer;
            GamePlayer gamePlayer = XRLCore.Core.Game.Player;

            GameObject pet = thePlayer.DeepCopy();

            /* Stylize pet */

            pet.pRender.DisplayName = "{{rusty|fleshy}} " + pet.pRender.DisplayName;
            pet.GetPart<Description>().Short = "It's you. There is the disconcerting sound of breathing emanating outward.";

            pet.AddPart<Pettable>();
            pet.RemovePart<OpeningStory>();

            // TODO: Add story, dialogue?

            /* Make pet actually a pet */

            pet.PartyLeader = thePlayer;
            pet.SetActive();

            /* Spawn pet */

            thePlayer.CurrentCell.GetConnectedSpawnLocation().AddObject(pet);

            /* Mechanize player */

            Mutations playerMutations = gamePlayer.Body.GetPart<Mutations>();
            playerMutations.MutationList.ForEach((mutation) => playerMutations.RemoveMutation(mutation));

            gamePlayer.Body.SafeForeachInventoryEquipmentAndCybernetics((C) => C.Destroy(Silent: true));

            Roboticized.Roboticize(thePlayer);

            /* Delete self */

            ParentObject.Destroy(Silent: true);

            return base.HandleEvent(E);
        }
    }
}
