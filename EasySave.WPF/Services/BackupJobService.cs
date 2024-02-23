﻿using EasySave.Models;
using EasySave.Services.Factories;
using EasySave.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Xml.Linq;

namespace EasySave.Services
{
    public class BackupJobService : IBackupJobService
    {
        private IFileService _fileService { get; set; }
        private static object _lock = new object();

        public BackupJobService(IFileServiceFactory fileServiceFactory)
        {
            string filePath = Path.Combine(Properties.Settings.Default.StateFolderPath, Properties.Settings.Default.StateFileName);
            _fileService = fileServiceFactory.CreateFileService("json", filePath);
        }

        public List<BackupJobInfo> GetAll()
        {
            List<BackupJobInfo> list;

            lock (_lock)
            {
                list = _fileService.Read<BackupJobInfo>();
            }

            if (list == null)
                return new List<BackupJobInfo>();

            return list;
        }

        public BackupJobInfo? Get(string name)
        {
            List<BackupJobInfo> list = GetAll();

            BackupJobInfo? entity = list.FirstOrDefault(entity => entity.BackupName == name); ;

            return entity;
        }

        public BackupJobInfo Create(BackupJobInfo entity)
        {
            List<BackupJobInfo> list = GetAll();

            list.Add(entity);

            _fileService.Write<BackupJobInfo>(list);

            return entity;
        }

        public BackupJobInfo? Update(BackupJobInfo entity)
        {
            List<BackupJobInfo> list = GetAll();
            BackupJobInfo? existingEntity = list.FirstOrDefault(x => x.BackupName == entity.BackupName);

            if (existingEntity != null)
            {
                int index = list.IndexOf(existingEntity);
                list[index] = entity;

                lock (_lock)
                {
                    _fileService.Write(list);
                }

                return entity;
            }

            return default(BackupJobInfo);
        }

        public bool Delete(string name)
        {
            List<BackupJobInfo> list = GetAll();

            BackupJobInfo? entityToDelete = list.FirstOrDefault(entity => entity.BackupName == name);

            if (entityToDelete != null)
            {
                list.Remove(entityToDelete);

                _fileService.Write(list);

                return true;
            }

            return false;
        }
    }



}