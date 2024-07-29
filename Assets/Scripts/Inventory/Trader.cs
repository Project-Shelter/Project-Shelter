using UnityEngine;

namespace ItemContainer
{
    public class Trader : Interactable
    {
        private PopupTrade trade;
        private bool isTradeOpen = false;

        public void Awake()
        {
            trade = Util.FindChild(Managers.UI.Root, "Pop_Trade", true).GetComponent<PopupTrade>();
        }
        
        public override void StartInteract(Actor actor)
        {
            base.StartInteract(actor);
            OpenTrade();
        }
        
        public override void Interacting()
        {
        }

        public override void StopInteract()
        {
        }

        public override bool CanKeepInteracting()
        {
            return false;
        }

        private void OpenTrade()
        {
            trade.Open();
            isTradeOpen = true;
        }
        
        private void CloseTrade()
        {
            isTradeOpen = false;
            trade.Close();
        }
    }
}