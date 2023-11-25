namespace AccessControlLayer.AccessControll.Contracts.Dto
{
    public class ActionTypeDto
    {
        public ActionTypeDto(string actionTypeId)
        {
            ActionTypeId = actionTypeId;
            ResourceIds = new List<string>();
        }

        public string ActionTypeId { get; set; }
        public List<string> ResourceIds { get; set; }
    }
}
