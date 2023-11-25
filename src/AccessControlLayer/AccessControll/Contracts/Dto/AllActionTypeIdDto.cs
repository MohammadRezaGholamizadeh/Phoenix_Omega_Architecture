namespace AccessControlLayer.AccessControll.Contracts.Dto
{
    public class AllActionTypeIdDto
    {
        public AllActionTypeIdDto()
        {
            ActionTypesId = new List<string>();
        }

        public List<string> ActionTypesId { get; set; }
    }

    public class GetUserAccessResultDto
    {
        public string ActionTypesId { get; set; }
        public bool HasAccess { get; set; }
    }
}
