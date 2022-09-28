﻿namespace UsersService.DataAccess.Entities
{
    public class UserInfoEntity
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? Patronymic { get; set; }
        public string? Email { get; set; }
        public int UserId { get; set; }
    }
}
