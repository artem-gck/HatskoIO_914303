﻿namespace CompanyManagementService.Services.Dto
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Patronymic { get; set; }
        public string Email { get; set; }
        public string Position { get; set; }
        public string Department { get; set; }
    }
}
