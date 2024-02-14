using EasySave.DataAccess.Services.Factories;
using EasySave.Domain.Models;
using EasySave.Domain.Services;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySave.DataAccess.Services
{
    public class StateService : DataService<State>, IStateService
    {
        public StateService(IConfiguration configuration, IFileServiceFactory fileServiceFactory) 
        {
            string type = configuration["DataFilesTypes:StatesFileType"];
            string filePath = Path.Combine(configuration["DataFilesLocation:StatesFolderPath"], configuration["DataFilesLocation:StatesJsonFileName"]);
            base._fileService = fileServiceFactory.CreateFileService(type, filePath);
        }

    }
}
