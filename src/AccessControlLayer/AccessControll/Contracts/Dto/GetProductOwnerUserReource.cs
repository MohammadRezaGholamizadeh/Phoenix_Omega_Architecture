namespace AccessControlLayer.AccessControll.Contracts.Dto
{
    public class GetApplicationUserReource
    {
        public GetApplicationUserReource(
            string actionTypeId)
        {
            ActionTypeId = actionTypeId;
            ResourcesId = new List<string>();
        }

        public string ActionTypeId { get; set; }
        public List<string> ResourcesId { get; set; }
    }
}
