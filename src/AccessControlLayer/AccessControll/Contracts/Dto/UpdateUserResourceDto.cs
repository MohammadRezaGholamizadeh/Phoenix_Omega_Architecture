namespace AccessControlLayer.AccessControll.Contracts.Dto
{
    public class UpdateUserResourceDto
    {
        public UpdateUserResourceDto()
        {
            NewActionTypes = new AddUserResourceAccessDto();
            DeletingActionTypesId = new DeleteUserResourceDto();
        }

        public AddUserResourceAccessDto NewActionTypes { get; set; }
        public DeleteUserResourceDto DeletingActionTypesId { get; set; }
    }
}
