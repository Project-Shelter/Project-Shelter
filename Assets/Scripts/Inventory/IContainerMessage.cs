namespace ItemContainer
{
    public interface IContainerMessage
    {
        //sender : 아이템 삭제, receiver : 아이템 추가
        public void SendItem(int sender, int receiver, ItemVO item)
        {
            
        }
    }
}