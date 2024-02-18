﻿using EasySave.Enums;
using System.ComponentModel;

namespace EasySave.Models
{
    public class BackupJobInfo : INamedEntity, INotifyPropertyChanged
    {
        private string? _backupName;
        public string? BackupName
        {
            get
            {
                return _backupName;
            }
            set
            {
                _backupName = value;
                OnPropertyChanged(nameof(BackupName));
            }
        }

        private string _sourceDirectory;
        public string SourceDirectory
        {
            get
            {
                return _sourceDirectory;
            }
            set
            {
                _sourceDirectory = value;
                OnPropertyChanged(nameof(SourceDirectory));
            }
        }

        private string _targetDirectory;
        public string TargetDirectory
        {
            get
            {
                return _targetDirectory;
            }
            set
            {
                _targetDirectory = value;
                OnPropertyChanged(nameof(TargetDirectory));
            }
        }
        
        private BackupType _backupType;
        public BackupType BackupType
        {
            get
            {
                return _backupType;
            }
            set
            {
                _backupType = value;
                OnPropertyChanged(nameof(BackupType));
            }
        }

        private BackupState _backupState;
        public BackupState BackupState
        {
            get
            {
                return _backupState;
            }
            set
            {
                _backupState = value;
                OnPropertyChanged(nameof(BackupState));
            }
        }

        private string? _backupTime;
        public string? BackupTime
        {
            get
            {
                return _backupTime;
            }
            set
            {
                _backupTime = value;
                OnPropertyChanged(nameof(BackupTime));
            }
        }

        private int _totalFilesNumber;
        public int TotalFilesNumber
        {
            get
            {
                return _totalFilesNumber;
            }
            set
            {
                _totalFilesNumber = value;
                OnPropertyChanged(nameof(TotalFilesNumber));
            }
        }

        private double _totalFilesSize;
        public double TotalFilesSize
        {
            get
            {
                return _totalFilesSize;
            }
            set
            {
                    _totalFilesSize = value;
                    OnPropertyChanged(nameof(TotalFilesSize));
                    OnPropertyChanged(nameof(Progression)); // Assurez-vous de notifier également pour la propriété calculée
            }
        }

        private int _nbFilesLeftToDo;
        public int NbFilesLeftToDo
        {
            get
            {
                return _nbFilesLeftToDo;
            }
            set
            {
                _nbFilesLeftToDo = value;
                OnPropertyChanged(nameof(NbFilesLeftToDo));
            }
        }

        private double _filesSizeLeftToDo;
        public double FilesSizeLeftToDo
        {
            get
            {
                return _filesSizeLeftToDo;
            }
            set
            {
                    _filesSizeLeftToDo = value;
                    OnPropertyChanged(nameof(FilesSizeLeftToDo));
                    OnPropertyChanged(nameof(Progression)); // Assurez-vous de notifier également pour la propriété calculée
            }
        }

        private string? _sourceTransferingFilePath;
        public string? SourceTransferingFilePath
        {
            get
            {
                return _sourceTransferingFilePath;
            }
            set
            {
                _sourceTransferingFilePath = value;
                OnPropertyChanged(nameof(SourceTransferingFilePath));
            }
        }

        private string? _targetTransferingFilePath;
        public string? TargetTransferingFilePath
        {
            get
            {
                return _targetTransferingFilePath;
            }
            set
            {
                _targetTransferingFilePath = value;
                OnPropertyChanged(nameof(TargetTransferingFilePath));
            }
        }


        public string Progression
        {
            get
            {
                if (TotalFilesSize > 0 && FilesSizeLeftToDo != 0)
                {
                    return (((TotalFilesSize - FilesSizeLeftToDo) / TotalFilesSize) * 100).ToString("#.##") + "%";
                }
                return "";
            }
        }

        public BackupJobInfo
        (   string backupName,
            string sourceDirectory, 
            string targetDirectory, 
            BackupType backupType, 
            BackupState backupState, 
            string? backupTime, 
            int totalFilesNumber, 
            long totalFilesSize, 
            int nbFilesLeftToDo, 
            long filesSizeLeftToDo, 
            string? sourceTransferingFilePath, 
            string? targetTransferingFilePath)
        {
            BackupName = backupName;
            SourceDirectory = sourceDirectory;
            TargetDirectory = targetDirectory;
            BackupType = backupType;
            BackupState = backupState;
            BackupTime = backupTime;
            TotalFilesNumber = totalFilesNumber;
            TotalFilesSize = totalFilesSize;
            NbFilesLeftToDo = nbFilesLeftToDo;
            FilesSizeLeftToDo = filesSizeLeftToDo;
            SourceTransferingFilePath = sourceTransferingFilePath;
            TargetTransferingFilePath = targetTransferingFilePath;
        }
        
        public BackupJobInfo()
        {
            BackupName = "";
            SourceDirectory = "";
            TargetDirectory = "";
            BackupType = BackupType.Complete;
            BackupState = BackupState.Inactive;
            BackupTime = null;
            TotalFilesNumber = 0;
            TotalFilesSize = 0;
            NbFilesLeftToDo = 0;
            FilesSizeLeftToDo = 0;
            SourceTransferingFilePath = "";
            TargetTransferingFilePath = "";
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
