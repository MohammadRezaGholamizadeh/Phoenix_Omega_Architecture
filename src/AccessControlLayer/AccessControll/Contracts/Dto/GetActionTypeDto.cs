namespace AccessControlLayer.AccessControll.Contracts.Dto
{
    public class GetActionTypeDto
    {
        public GetActionTypeDto(string actionTypeId, string actionTypeName)
        {
            ActionTypeId = actionTypeId;
            ActionTypeName = actionTypeName;
        }

        public string ActionTypeId { get; set; }
        public string ActionTypeName { get; set; }
    }
}
