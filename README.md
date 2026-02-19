# DA.SharedDeskPlanner
Planner for shared desk rooms

English version see below

# Einführung
In Zeiten von Shared Desks ist es sinnvoll ein Raumplanungstool zu haben. D.h. wer bucht wann welchen Raum bzw. desk.
## Platforms, Dependencies
* .Net 10
* MySQL.EntityFrameworkCore 10.0.1
# Ziel
Ziel ist es, eine Blazor-Web-App zu erstellen, mit welcher der User einen Raum/Desk buchen kann.
# Realisierung
Im folgenden wird geschildert, wie das Projekt realisiert wird.
## Serialisierung
Serialisiert soll das Ganze mittels Entity Framework in einer SQL-Datenbank (MariaDB)
## Client Bestandteile
Zur Administration (Stammdatenpflege, etc.) soll eine WPF Anwendung erstellt werden.
Für den Benutzer, den Desk-Bucher, soll eine Blazor-Web-App erstellt werden.
## Code Projects
### DA.SharedDeskPlanner.Model
### DA.SharedDeskPlanner.Wpf

Enthält das Datenmodell
# Bemerkungen
1. alle Zeiten in UTC
# Dokumentationen
1. https://dev.mysql.com/doc/connector-net/en/connector-net-entityframework-core-example.html
1. https://stackoverflow.com/questions/59909207/cannot-add-appsettings-json-inside-wpf-project-net-core-3-0

***

**English version** is coming soon
