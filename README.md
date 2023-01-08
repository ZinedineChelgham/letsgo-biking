# Projet MWSOC Let's go bicking

Le but du projet "Let's Go Biking!" est de développer une petite application pour calculer les itinéraires en réduisant au maximum la distance à parcourir à pied (en utilisant des vélos à la place).

## Fonctionnalités implémentées

* mvp
* ActiveMQ V1
* Proxy / Cache

## Services Setup
### Routing Service
build vstudio `.\Services\Routing Service\bin\Debug\Routing Service.exe"`. 
Ne pas oublier d'executer le exe en admin
### Proxy / Cache
build sur vstudio est exécuter l'exécutable situé dans le répertoire `proxy-server\proxy-server\proxy-server\bin\Debug` (oui ça fait beaucoup de proxy-server)
ou exécuter directement l'exécutable déjà présent
Ne pas oublier de l'executer aussi en tant qu'admin

### ActiveMQ
Ne pas oublier `activemq start`
Adresse utilisée par le client `tcp://localhost:61616` to start the heavy client sucessfuly

### Heavy Java Client
Lancer le main depuis un IDE (la génération d'un jar ne marchait pas très bien)

## Déroulement d'une exécution 
- Lancer tout les services précédemment cités 
- Lancer le client Java
- Enjoy



