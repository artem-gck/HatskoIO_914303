namespace DocumentCrudService.Repositories.Entities
{
    public class DocumentNameEntity
    {
        public Guid Id { get; set; }
        public int Version { get; set; }
        public string FileName { get; set; }
        public DateTime UploadDate { get; set; }
    }
}
