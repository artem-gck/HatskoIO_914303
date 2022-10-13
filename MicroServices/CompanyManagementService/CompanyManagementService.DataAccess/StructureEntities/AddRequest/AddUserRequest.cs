namespace CompanyManagementService.DataAccess.StructureEntities.AddRequest
{
    public class AddUserRequest
    {
        public Guid Id { get; set; }
        public int Salary { get; set; }
        public string Position { get; set; }
        public Guid CheifUserId { get; set; }
    }
}
