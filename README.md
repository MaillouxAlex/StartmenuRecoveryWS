
# Start Menu Recovery 

### Start Menu Recovery Web Service (API et Console web)
*	Accueille les données amassées grâce à Lnk Collector
*	Base de données questionnée par Lnk Creator
*	Console de gestion Web disponible.
*	Seulement les raccourcis autoriés avec une date d'approbation via la console web seront propagés.
*	Le champs Status sert seuelement à aider à faire des trie et des recherches dans l'interface web.

### Lnk Collector 1.0 FR (Powershell)
*	Outil de collecte des informations sur les raccourcis toujours existants dans notre parc
*	Déployé par SCCM\MECM automatiquement sur chaque ordinateur, dès que possible, dès que l’ordinateur est branché à un réseau
*	L’installation et l’exécution sont silencieuses, aucun redémarrage ni interaction
*	S’exécute une seule fois et crée un fichier log des raccourcis récupérés à l’endroit suivant : "C:\Windows\_Cegep\Lnk Collector 1.0 FR\Lnk Collector.json"
*	Emplacement des journaux d’exécution : "C:\Windows\Logs\Software\CegepdeChicoutimi_LnkCollector_1.0_x64_EN_01_PSAppDeployToolkit_Install.log"
*	Emplacement du package de gestion : ".\gestion\Lnk Collector 1.0 FR\Package\Lnk Collector 1.0 FR" 


### Lnk Creator 1.0 FR (Powershell)
*	Outil permettant de créer automatiquement les raccourcis manquants du menu démarrer
*	Déployé par SCCM\MECM automatiquement sur chaque ordinateur, dès que possible, dès que l’ordinateur est branché à un réseau
*	L’installation et l’exécution sont silencieuses, aucun redémarrage ni interaction
*	L’outil s’installe dans le répertoire suivant : "C:\Windows\_Cegep\Lnk Creator 1.0 FR"
*	S’exécute à toutes les 30 minutes via une tâche planifiée à la recherche de raccourcis manquants sur l’ordinateur
*	Pour qu’un raccourci soit créé, il doit avoir été approuvé au préalable, doit avoir le même répertoire source du logiciel concerné, être manquant, avoir été récupéré par Lnk Collector ou avoir été créé dans le service Web
*	Vous pouvez l’exécuter manuellement avec des droits d’administrateur : "C:\Windows\_Cegep\Lnk Creator 1.0 FR\Deploy-Application.exe"
*	Vous pouvez l’exécuter manuellement en appelant la tâche planifiée avec des droits d’administrateur via Powershell :  Start-ScheduledTask -TaskName "LnkCreator"
*	Vous pouvez exécuter le script SCCM\MECM suivant sur des ordinateurs ciblés dans la console : Run Lnk Creator LocalInstance
*	Vous pouvez l’installer manuellement s’il n’est pas encore propagé par SCCM\MECM (attention, nécessite des droits pour accéder au répertoire, le faire sécuritairement) : ".\Lnk Creator 1.0 FR\Package\Lnk Creator 1.0 FR\Deploy-Application.exe"
*	Journal dressant la liste des raccourcis créés par l’outil : "C:\Windows\_Cegep\Lnk Creator 1.0 FR\LnkCreator.log"
*	Emplacement des journaux d’exécution : "C:\Windows\Logs\Software\CégepdeChicoutimi_LnkCreatorLocalInstance_1.0_x64_EN_01_PSAppDeployToolkit_Install.log"
*	Emplacement du package de gestion : ".\gestion\Lnk Creator 1.0 FR\Package\Lnk Creator 1.0 FR"

## Installation

### prérequis
* Windows Server + IIS 10
* [Sql Server 2019](https://go.microsoft.com/fwlink/p/?linkid=2216019&clcid=0xc0c&culture=fr-ca&country=ca)
* [Asp.net Core](https://dotnet.microsoft.com/en-us/download/dotnet/thank-you/runtime-aspnetcore-7.0.2-windows-hosting-bundle-installer)

### Publication
* Créer un site web dans IIS.
* À l'aide de visualstudio publier l'application vers le réperoire.

### Modification des scripts avant le déploiement de ceux-ci
* modifier .\Lnk Creator 1.0 FR\Files\_Cegep\Lnk Creator 1.0 FR\Deploy-Application.ps1
   * line 55 $URI = http://servername.com/api/Sorcuts
   * line 57 $RestoredURI = http://servername.com/api/SorcutRestored
* Lnk Collector 1.0 FR\Files\_Cegep\Lnk Collector 1.0 FR\Deploy-Application.ps1
   * line 55 $Url = "http://servername.com/api/Sorcuts"

### Database
* Créer la base de données
    * Collation: SQL_Latin1_General_CP1_CI_AS
    * Ajouter appPool de iis en data Read/Write
    * Run  <path IIS>\StartMenuRecoveryWS\EFSQLScripts\StartmenuRecoveryWS.Data.ApplicationDbContext.sql
    * Modifier "appsettings.json"  section ConnectionStrings

### Administrateur par défaut
* Utilisateur: admin@app.local
* Mot de passe : Admin123$
    
## Authors

- [@Cégep de Chicoutimi - Alex Mailloux](https://www.github.com/MaillouxAlex)


## MIT License

Copyright (c) 2023 Cégep de Chicoutimi

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
