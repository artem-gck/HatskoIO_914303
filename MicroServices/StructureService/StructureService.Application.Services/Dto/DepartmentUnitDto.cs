﻿namespace StructureService.Application.Services.Dto
{
    public class DepartmentUnitDto : BaseDto
    {
        public int UserId { get; set; }
        public int PositionId { get; set; }
        public int DepartmentId { get; set; }
    }
}
