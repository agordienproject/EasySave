# Documentation utilisateur

## Instalation de l'exécutable

## Guide d'utilisation

### 1/ Interface

![image](https://github.com/agordienproject/EasySave/assets/127090687/e38a9e2a-3ea8-433e-a6f2-5c71fa5a5a17)


### 2/ Liste des commandes et options


#### Commandes

- EasySave create : Créer un travail de sauvegarde.

- EasySave show : Afficher l'ensemble des travaux de sauvegarde.

- EasySave delete : Supprimer un travail de sauvegarde.

- EasySave execute : Exécuter un travail de sauvegarde.


#### Options

- --name -n : nom du travail de sauvegarde.

- --source -s : chemin du dossier source.

- --destination -d : chemin du dossier de destination.

- --type -t : type de sauvegarde (complète ou différentielle).

- --range -r: plage de sauvegarde à exécuter.


#### Exemples

- EasySave create --name {nomDeLaSauvegarde} --source {repertoireSource} --destination {repertoireDestination} --type {typeDeSauvegarde}
- EasySave show
- EasySave delete --name {nomDeLaSauvegarde}
- EasySave execute -r 1-3 


### 3/ Emplacements des fichiers

- BackupJobsFolderPath : .\\Data\\BackupJobs\\
- BackupJobsJsonPath : .\\Data\\BackupJobs\\backupjobs.json
- StatesFolderPath : .\\Data\\State\\
- StatesJsonFilePath : .\\Data\\State\\states.json
- LogsFolderPath : .\\Data\\Logs\\
