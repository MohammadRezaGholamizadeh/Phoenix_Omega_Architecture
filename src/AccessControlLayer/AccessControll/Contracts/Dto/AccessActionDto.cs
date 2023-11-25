namespace AccessControlLayer.AccessControll.Contracts.Dto
{
    public class AccessActionDto
    {
        public string ActionTypeId { get; set; } = default!;
        public string ResourceTypeId { get; set; } = default!;
    }
}
