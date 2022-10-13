namespace CompanyManagementService.DataAccess.StructureEntities.UpdateRequest
{
    public class UpdateDepartmentRequest
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid CheifUserId { get; set; }
    }
}
