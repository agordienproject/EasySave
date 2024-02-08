# Documentation utilisateur

## I) Instalation de l'exécutable

Pour commencer, ProSoft vous fournit un exécutable qui va s’occuper d’installer tout le programme dont vous avez besoin.

![Aspose Words e993f0b3-7905-445e-acae-2c775ecdb285 001](https://github.com/agordienproject/EasySave/assets/127090687/95a0b779-f2be-40df-af01-601e4b4b1b18)


Pour ce faire, il faut lancer l’exe « EasySave.exe ». Il est fort probable que Windows Defender ne laisse pas passer cette exécution. Pas de soucis, autorisez l’exécution.

Une fois l’exécutable lancé, un assistant d’installation va se lancer. Vous pouvez simplement suivre toutes les étapes renseignées pour l’installer.

![Aspose Words e993f0b3-7905-445e-acae-2c775ecdb285 002](https://github.com/agordienproject/EasySave/assets/127090687/e22be2f4-3454-49f9-ac34-5253fd90d6a5)


![Une image contenant texte, capture d’écran

Description générée automatiquement](Aspose.Words.e993f0b3-7905-445e-acae-2c775ecdb285.003.png)

![Aspose Words e993f0b3-7905-445e-acae-2c775ecdb285 004](https://github.com/agordienproject/EasySave/assets/127090687/d3eeab8a-8d06-42db-a735-a531dba552a6)


Une fois l’installation finie, le programme se charge de rajouter automatiquement une variable d’environnement PATH dans votre système, afin que vous puissiez l’utiliser depuis n’importe quel terminal :

![Aspose Words e993f0b3-7905-445e-acae-2c775ecdb285 005](https://github.com/agordienproject/EasySave/assets/127090687/83502ff4-dd0b-4140-8475-85e282053109)


Une fois que tout est installé, vous pouvez lancer le programme de 2 manières différentes :

1. Lancer le programme depuis votre bureau si vous l’avez ajouté lors de l’installation

![Aspose Words e993f0b3-7905-445e-acae-2c775ecdb285 006](https://github.com/agordienproject/EasySave/assets/127090687/b1a325a9-3485-4355-9463-e3f47ed67315)


2. Lancer n’importe quel terminal puis lancez la commande EasySave

![Aspose Words e993f0b3-7905-445e-acae-2c775ecdb285 007](https://github.com/agordienproject/EasySave/assets/127090687/cbdaec4c-d555-42ce-be15-c0fe6db67cbe)


L'utilisation d'un terminal permet d'envoyer une commande sans rester dans l'application :

![image](https://github.com/agordienproject/EasySave/assets/127090687/ebf6cfb1-1786-4902-bd23-fa2de9d3e63d)




## II) Guide d'utilisation

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

