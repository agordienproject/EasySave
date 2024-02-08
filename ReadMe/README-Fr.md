# Documentation utilisateur

## I) Instalation de l'exécutable

Pour commencer, ProSoft vous fournit un exécutable qui va s’occuper d’installer tout le programme dont vous avez besoin.

![](Aspose.Words.e993f0b3-7905-445e-acae-2c775ecdb285.001.png)

Pour ce faire, il faut lancer l’exe « EasySave.exe ». Il est fort probable que Windows Defender ne laisse pas passer cette exécution. Pas de soucis, autorisez l’exécution.

Une fois l’exécutable lancé, un assistant d’installation va se lancer. Vous pouvez simplement suivre toutes les étapes renseignées pour l’installer.

![](Aspose.Words.e993f0b3-7905-445e-acae-2c775ecdb285.002.png)

![Une image contenant texte, capture d’écran

Description générée automatiquement](Aspose.Words.e993f0b3-7905-445e-acae-2c775ecdb285.003.png)

![Une image contenant texte, capture d’écran, affichage, logiciel](Aspose.Words.e993f0b3-7905-445e-acae-2c775ecdb285.004.png)

Une fois l’installation finie, le programme se charge de rajouter automatiquement une variable d’environnement PATH dans votre système, afin que vous puissiez l’utiliser depuis n’importe quel terminal :

![Une image contenant texte, capture d’écran, Police, nombre

Description générée automatiquement](Aspose.Words.e993f0b3-7905-445e-acae-2c775ecdb285.005.png)

Une fois que tout est installé, vous pouvez lancer le programme de 2 manières différentes :

1. Lancer le programme depuis votre bureau si vous l’avez ajouté lors de l’installation

![Une image contenant texte, capture d’écran, logo, graphisme

Description générée automatiquement](Aspose.Words.e993f0b3-7905-445e-acae-2c775ecdb285.006.png)

2. Lancer n’importe quel terminal puis lancez la commande EasySave

![](Aspose.Words.e993f0b3-7905-445e-acae-2c775ecdb285.007.png)

L'utilisation d'un terminal permet d'envoyer une commande sans rester dans l'application :

![image](https://github.com/agordienproject/EasySave/assets/127090687/1ea63361-6e7b-4271-ac3c-dc91c5931074)



## II) Guide d'utilisation

### 1/ Accès à l'application

#### Via 

### 1/ Interface

L'interface de la version 1.0 du logiciel EasySave est la suivante :

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
