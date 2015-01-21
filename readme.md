# Répertoir

[![Build status](https://ci.appveyor.com/api/projects/status/3w71kfbk0g9yq5i0)](https://ci.appveyor.com/project/michelc/repertoir)

Gestion d'un petit répertoire de contacts pour servir de banc d'essai
à la création d'une application avec ASP.NET MVC 4.


## Ce qui est déjà fait

### Généralités

* Entity Framework Code First
* Entity Migration : en vue d'un déploiement sur AppHarbor
* MiniProfiler : pour connaitre le code SQL généré par EF
* LowercaseRoutesMVC : pour avoir des URLs en minuscule
* AutoMapper : pour passer des classes Model à ViewModel
* Déploiement sur AppHarbor

### Charte graphique

* Skeleton : pour avoir un affichage responsive (y compris pour les formulaires)
* Mise au point des CSS pour les tables, les icones, les boutons, les formulaires
  et les vues affichant seulement des données
* Adaptation des CSS pour les smartphones

### Formulaires

* Regroupement des Javascripts en fin de page dans une vue partielle
* Ajout de contrôles EditorTemplates typés HTML5 pour la saisie des numéros de
  téléphone, des adresses email et des URLs.
* Ajout d'un EditorTemplate pour la saisie de la civilité (trop spécifique ?)
* Ajout d'un EditorTemplate pour la saisie des chaines de caractères pour styler
  la zone de saisie en fonction de sa taille
* Mise en commun du code des vues Create.cshtml et Edit.cshtml dans une vue
  partielle _Editor.cshtml
* Mise en évidence des zones de saisie obligatoire (création du helper
  Html.CaptionFor() pour remplacer Html.LabelFor())
* Protection contre les attaques CSRF
* Trim() des chaines postées dans un formulaire
* Création d'un helper HtmlCancel() pour abandonner la saisie

### Divers

* Mise en commun du code des vues Details.cshtml et Delete.cshtml dans une vue
  partielle _Display.cshtml
* Création d'un helper HtmlCrud() pour afficher un menu des actions CRUD
* Utilisation du CDN Microsoft pour les fichiers jQuery
* Affichage d'une carte Google Maps pour localiser les contacts
* Gestion de tags (tables Tags et Contacts_Tags plus saisie via Chosen)


## Idées et Todo

### Pour le "framework"

* Rechercher si AntiXSS est nécessaire en plus de AntiForgeryToken : OUI !!!
  * [Securing Your ASP.NET Applications](http://msdn.microsoft.com/en-us/magazine/hh708755.aspx)
  * [ASP.NET MVC security and hacking: Defense-in-depth](https://sites.google.com/site/muazkh/asp-net-mvc-security-and-hacking-defense-in-depth)
* Compléter les tests unitaires
  * ListCompanies dans le contrôleur People
  * Présence de l'attribut ValidateAntiForgeryToken
  * Models et ViewModels


### Pour l'application

* Détail d'une personne => afficher le détail de sa société
* Gérer notes, liens, pièces jointes ... sur les contacts
* Gérer les doublons
