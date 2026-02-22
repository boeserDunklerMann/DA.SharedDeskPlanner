# DA.SharedDeskPlanner
Planner for shared desk rooms

English version see below

# Einführung
In Zeiten von Shared Desks ist es sinnvoll ein Raumplanungstool zu haben. D.h. wer bucht wann welchen Desk in welchem Raum
## Platforms, Dependencies
* .Net 10
* MySQL.EntityFrameworkCore 10.0.1
# Ziel
Ziel ist es, eine Blazor-Web-App zu erstellen, mit welcher der User einen Desk buchen kann.
# Realisierung
Im folgenden wird geschildert, wie das Projekt realisiert wird.

## fachliche Use-Cases
Ich möchte in der WebAPI verhindern, reintechnische Verben (CRUD bzw. FUCK) zu verwenden, so blickt man schnell nicht mehr durch, wenn man sehen will, was macht die Anwendung da eigentlich.
Siehe auch hier: https://www.heise.de/blog/Software-fachlich-modellieren-CRUD-war-gestern-10292363.html
oder hier: https://www.heise.de/blog/Warum-CRUD-fuer-Maerchen-und-Unternehmen-gleichermassen-ungeeignet-ist-10515489.html
Stattdessen drösel ich erstmal alle sinnvollen Use-Cases auf und erstelle danach die entsprechenden WebAPI-Endpoints
### Desk buchen

#### setzt voraus
1. Desk existiert
2. ...und ist einem Room zugeordnet
3. User existiert
4. Desk im gewünschten Zeitraum noch nicht gebucht

#### nötige Eingangsparameter
1. DeskID
2. UserID
3. Begin
4. End

## Serialisierung
Serialisiert soll das Ganze mittels Entity Framework in einer SQL-Datenbank (MariaDB)
## Server Bestandteile
### DA.SharedDeskPlanner.WebAPI
Rest-API zum Datenzugriff, FUCK Operationen für alle Entities, die in Model definiert sind.
Sowie Abbildung der Use-Cases als Rest-API-Endpoints.

## Client Bestandteile
Zur Administration (Stammdatenpflege, etc.) soll eine WPF Anwendung erstellt werden.
Für den Benutzer, den Desk-Bucher, soll eine Blazor-Web-App erstellt werden.
## Coded Projects

### DA.SharedDeskPlanner.Model
Alle Entitäten überschreiben dei Methoden `.Equals(...)` und `.GetHashCode()`

Basis-Model-Klasse `BaseModel` mit
- ID: int
- Name: string
- ChangeDate: DateTime?
- CreationDate: DateTime
- Deleted: bool

Entität `Room`, mit einer Liste von `Desk`s.
Entität `Desk`, mit einer Liste von `InventoryItem`s sowie einem `Room` und einer Liste von `Booking`s.
Entität `InventoryItem` mit einem `Desk` enthält Informationen zur Desk-Ausstattung (Monitor, Docking, Keyboard, Mouse etc.)
Entität `User` mit `FirstName`und `LastName` und eine Liste von `Booking`s
Entität `Booking` mit `BookingStart` und `BookingEnd` (jeweils DateTime) und je einem `User` und `Desk`

### DA.SharedDeskPlanner.Wpf

Enthält das Datenmodell
# Bemerkungen
1. alle Zeiten in UTC
2. FUCK: Find, Update, Create, Kill = CRUD
# Dokumentationen
1. https://dev.mysql.com/doc/connector-net/en/connector-net-entityframework-core-example.html
1. https://stackoverflow.com/questions/59909207/cannot-add-appsettings-json-inside-wpf-project-net-core-3-0

***

**English version** is coming soon
