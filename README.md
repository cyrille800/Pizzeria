
Table des matières
Gestion Pizzeria	1
Présentation notre idée sur le sujet	2
Explications des démarches du projet	3
Description textuelle du projet	3
La partie connexion	4
Installation du projet	4



Rapport du problème en programmation orienté objet avancé 
Gestion Pizzeria
Nous avons été confrontés lors de ce 5éme semestre à un projet informatique qu’il nous a fallu réaliser en C# en utilisant les concepts de la programmation orientée Objet.
Le thème de cette année portait sur la réalisation d’une application pour faciliter la gestion d’une pizzeria en respectant le concept de la programmation orientée objet. Pour ce faire, il nous a été demandé de nous inspirer du sujet et de laisser libre court à notre imagination et notre savoir-faire. Nous avons essayé de voir plus grand en respectant le sujet et en ajoutant plusieurs idées qui nous ont permis au final de bien comprendre les concepts POO et aussi maitriser les différentes possibilités offertes par le langage informatique système C#.
Présentation notre idée sur le sujet

En plus d’une interface gestion des livreurs, des commandes et des clients qui sont contrôlés et manipulés par un commis, Nous avons proposé des interfaces de plus qui vont permettre aux clients et aux livreurs d’interagir à distance sur notre application. Notamment Nous avons :  

 
Utilisé par le client pour effectuer ses commandes
 
Utilisé par le livreur pour consulter sa livraison à effectué

Puis que nous avons donné la possibilité au client de faire ses commandes avec notre application, et la possibilité à notre livreur d’avoir un compte personnel, Nous n’avons pas vu l’utilité d’un compte Commis et nous l’avons remplacé par un compte administrateur qui va se charger de la partie Backend de l’application, c’est-à-dire la partie de gestion de l’application. Notre application fonctionnera donc, comme un Uber Eat des pizzas, car le client à la possibilité de gérer son panier dans lequel il pourra ajouter, modifier, supprimer et afficher les produits se trouvant à l’intérieur. Pour le bon fonctionnement de notre plateforme, nous avons due créer un catalogue de pizzas qui va permettre à un client de choisir ce qu’il veut commander, et la pizzeria se chargera de reproduire ce qu’il a commandé et ensuite il sera livré. Nous avons écrit des algorithmes de collectes de données en C# pour remplir notre catalogue de pizzeria. Pour cela, Nous avons utilisé la bibliothèque SELENIUM qui nous à permis d’accéder aux sites des pizzerias https://www.italiano-pizza.fr, https://www.allopizza94.com/index  en fin de pourvoir récupérer tous les contenus exploitables, dont nous avons récupérer les pizzas et les desserts de chaque pizzeria, Nous pourrions ajouter plus de sites, mais nous nous sommes limités à 2 à cause du temps. Ainsi, le client à une vaste possibilité de faires ses choix.

Le travail a été effectué par deux étudiants et l’outil GitHub nous a permis de travailler sans problème à distance donc voici le lien du projet. https://github.com/cyrille800/UserPizzeria



Explications des démarches du projet

Pour la bonne compréhension du fonctionnement de notre projet, nous avons préparé 2 diagrammes complet qui sont : 
Le diagramme de Classe qui explique les différentes classes utilisées dans le projet.
Le diagramme de cas d’utilisation qui explique les fonctions de chaque acteur de notre système.
Description textuelle du projet

Le Client
Le client parcourt le catalogue de produits que je propose, Sélectionne les pizzas et les desserts qu’ils souhaitent et qui seront automatiquement ajoutés dans son panier, ensuite il peut passer la commande qui automatiquement va sélectionner un livreur disponible, si aucun livreur n’est disponible, il y aura un message d’alerte pour lui demander de revenir après.
Livreur
Lorsque le livreur essaye de se connecte, s’il a été assigné à une commande, alors, nous affichons la commande moyennant un QR Code qui va lui permet d’avoir les détails de la commande, et ainsi  lui laisse la possibilité de dire s’il a fini sa commande ou pas.
Admin
Il se charge de gérer les livreurs c’est-à-dire, les opérations de CRUD et se charge également de faire des statistiques et analyse.
La partie connexion
Compte livreur
Une fois, que vous avez cliqué sur le bouton login de la page index du projet, vous avez besoin du numéro de portable du livreur pour vous connectez sur le compte.
	Admin
Il n’a pas de compte spécial, juste il doit saisir Admin pour se connecter. 

Installation du projet
Lors du premier lancement du projet sur votre ordinateur, vous devez cliquer sur le buton update de la page index, il va me permettre de mettre à jour notre catalogue de pizzeria, car initialement il n’existe pas, pour des raisons d’autorisation de système. 
 

Vous devez également désactiver votre Avast, temporairement, car mes scripts javascript exécuté dans le composant WebBrowser seront bloqué par Avast.  

Nous avons bien des choses à dire pour notre projet, mais le nombre maximal de pages est de 4.
