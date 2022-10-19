namespace CompanyManagementService.DataAccess.StructureEntities.Responce
{
    public class UserResponce
    {
        public Guid Id { get; set; }
        public int Salary { get; set; }
        public string Position { get; set; }
        public string Department { get; set; }
        public Guid CheifUserId { get; set; }
    }
}
