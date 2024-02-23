﻿using EasySave.Domain.Services;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySave.DataAccess.Services.Factories
{
    public class FileServiceFactory : IFileServiceFactory
    {
        public IFileService CreateFileService(string type, string filePath)
        {
            switch (type)
            {
                case "json":
                    return new JsonFileService(filePath);
                case "xml":
                    return new XMLFileService(filePath);
                default:
                    throw new ArgumentException("Invalid type", "type");
            }
        }
    }
}