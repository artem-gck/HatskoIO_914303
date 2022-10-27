﻿namespace SignatureService.DataAccess.DataBase.Exceptiions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(Guid id, int version)
            : base($"Not found signatures for document with id = {id} and version = {version}")
        {

        }
    }
}
