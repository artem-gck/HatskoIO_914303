namespace CompanyManagementService.DataAccess.StructureEntities.UpdateRequest
{
    public class UpdateUserRequest
    {
        public Guid Id { get; set; }
        public int Salary { get; set; }
        public string Position { get; set; }
        public Guid CheifUserId { get; set; }
    }
}
