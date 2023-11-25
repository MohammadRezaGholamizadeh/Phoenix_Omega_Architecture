namespace AccessControlLayer.AccessControll.Contracts.Dto
{
    public class AddUserResourceAccessDto
    {
        public AddUserResourceAccessDto()
        {
            ActionTypes = new List<ActionTypeDto>();
        }

        public List<ActionTypeDto> ActionTypes { get; set; }
    }
}
