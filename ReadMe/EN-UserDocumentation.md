# User documentation

## I) Executable installation

First of all, ProSoft presents an executable in order to install the EasySave software on your device.

![Aspose Words e993f0b3-7905-445e-acae-2c775ecdb285 001](https://github.com/agordienproject/EasySave/assets/127090687/95a0b779-f2be-40df-af01-601e4b4b1b18)

[!CAUTION]
You must run the execution with the administrator mode!

To do that, the executable file "EasySave.exe" must be launched. In some cases, Windows Defender could block this execution. If so, allow the execution.

After this, follow the step by step explanations of the installation assistant.

![Aspose Words e993f0b3-7905-445e-acae-2c775ecdb285 002](https://github.com/agordienproject/EasySave/assets/127090687/e22be2f4-3454-49f9-ac34-5253fd90d6a5)

![Aspose Words e993f0b3-7905-445e-acae-2c775ecdb285 004](https://github.com/agordienproject/EasySave/assets/127090687/d3eeab8a-8d06-42db-a735-a531dba552a6)

When the installation is done, an environment variable PATH is automatically added by the program in your device system.
This allows you to use EasySave from any terminal: 

![Aspose Words e993f0b3-7905-445e-acae-2c775ecdb285 005](https://github.com/agordienproject/EasySave/assets/127090687/83502ff4-dd0b-4140-8475-85e282053109)

At that point, you can launch EasySave two differents ways:

1. Launch the software **WITH THE ADMINISTRATOR MODE** from your device menu/desk: 

![Aspose Words e993f0b3-7905-445e-acae-2c775ecdb285 006](https://github.com/agordienproject/EasySave/assets/127090687/b1a325a9-3485-4355-9463-e3f47ed67315)

2. Launch any terminal **WITH THE ADMINISTRATOR MODE** and enter the command "EasySave": 

![Aspose Words e993f0b3-7905-445e-acae-2c775ecdb285 007](https://github.com/agordienproject/EasySave/assets/127090687/cbdaec4c-d555-42ce-be15-c0fe6db67cbe)

Using a terminal allows you to launch cammands in EasySave without staying in the software: 

![image](https://github.com/agordienproject/EasySave/assets/127090687/ebf6cfb1-1786-4902-bd23-fa2de9d3e63d)


## II) User manual

### 1/ Interface

The interface for this 1.0 version is the following: 

![image](https://github.com/agordienproject/EasySave/assets/127090687/e38a9e2a-3ea8-433e-a6f2-5c71fa5a5a17)

### 2/ Commands and options

#### Commands

[!WARNING]
When using paths with spaces, you must use quotes, otherwise EasySave doesn't count it as a valid path.  

- **EasySave create** : Create a new backup job.

- **EasySave show** : Display all the backup jobs.

- **EasySave delete** : Delete a backup job.

- **EasySave execute** : Launch a backup job.

#### Options

- **--name -n** : name of the backup job.

- **--source -s** : path of the source directory.

- **--destination -d** : path of the destination directory.

- **--type -t** : backup type (complete or differential).

- **--range -r** : backups range to execute.

- **--language -l** : change the language (English or French).

#### Examples

- EasySave create --name {backupName} --source {bourceDirectory} --destination {destinationDirectory} --type {backupType}

- EasySave show

- EasySave delete --name {backupName}

- EasySave execute -r 1-3 






