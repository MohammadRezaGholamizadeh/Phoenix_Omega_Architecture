namespace AccessControlLayer.AccessControll.Contracts.Dto
{
    public class DeleteUserResourceDto
    {
        public DeleteUserResourceDto()
        {
            DeletingActionTypeId = new List<string>();
        }

        public List<string> DeletingActionTypeId { get; set; }
    }
}
