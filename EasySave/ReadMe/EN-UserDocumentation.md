# User documentation

## I) Executable installation

First of all, ProSoft presents an executable in order to install the EasySave software on your device.

![Aspose Words e993f0b3-7905-445e-acae-2c775ecdb285 001](https://github.com/agordienproject/EasySave/assets/127090687/95a0b779-f2be-40df-af01-601e4b4b1b18)

>[!CAUTION]
>You must run the execution with the administrator mode!

To do that, the executable file "EasySave.exe" must be launched. In some cases, Windows Defender could block this execution. If so, allow the execution.

After this, follow the step by step explanations of the installation assistant.

![image](https://github.com/agordienproject/EasySave/assets/150005779/aa7ea79a-7a00-4651-9f1f-fc0e1921c956)

![image](https://github.com/agordienproject/EasySave/assets/150005779/476d128c-40cd-4625-8489-513baa182c20)

When the installation is done, an environment variable PATH is automatically added by the program in your device system.
This allows you to use EasySave from any terminal: 

![Aspose Words e993f0b3-7905-445e-acae-2c775ecdb285 005](https://github.com/agordienproject/EasySave/assets/127090687/83502ff4-dd0b-4140-8475-85e282053109)

At that point, you can launch EasySave two differents ways:

1. Launch the software **WITH THE ADMINISTRATOR MODE** from your device menu/desk: 

![Aspose Words e993f0b3-7905-445e-acae-2c775ecdb285 006](https://github.com/agordienproject/EasySave/assets/127090687/b1a325a9-3485-4355-9463-e3f47ed67315)

2. Launch any terminal **WITH THE ADMINISTRATOR MODE** and enter the command "EasySave": 

![image](https://github.com/agordienproject/EasySave/assets/150005779/d5534494-d67b-473f-b042-9f2dfe4bf16b)

Using a terminal allows you to launch cammands in EasySave without staying in the software: 

![image](https://github.com/agordienproject/EasySave/assets/127090687/ebf6cfb1-1786-4902-bd23-fa2de9d3e63d)


## II) User manual

### 1/ Interface

The interface for this 1.0 version is the following: 

![image](https://github.com/agordienproject/EasySave/assets/150005779/44ffbeb2-c8d1-48a7-98f5-6cd9a1b69ea6)

### 2/ Commands and options

#### Commands

> [!WARNING]
> When using paths with spaces, you must use quotes, otherwise EasySave doesn't count it as a valid path.  

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

**Create a backup job :**
```
EasySave create --name {nomDeLaSauvegarde} --source {repertoireSource} --destination {repertoireDestination} --type {typeDeSauvegarde}
```

**View list of backup jobs :**
```
EasySave show
```

**Delete a backup job:**
```
EasySave delete --name {nomDeLaSauvegarde}
```

**Run one or more backup jobs:**
```
EasySave execute -r 1-3 // Execute les travaux 1 à 3
EasySave execute -r 1;3 // Execute les travaux 1 et 3
EasySave execute -r 5   // Execute le travail 5
```

**Change app language:**
```
EasySave language -l en
```





