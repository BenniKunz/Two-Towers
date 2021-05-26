# A0 Tower Defence Game – 2 Towers / Konzept
Dynamisches 2D Tower Defence Game mit der Möglichkeit die Angreifer mit Hilfe von Towern sowie Rechenoperationen aufzuhalten.

# A1 Features im Endprodukt:
* Gegner spawnen an fixiertem Punkt der statischen Map
* Gegner(fixierte Anzahl) laufen anhand fixierter Punkte auf der Karte zu einem fixierten Zielpunkt
* der Spieler verliert Leben, wenn ein Gegner das Ziel erreicht
* Spieler kann max. 2 Tower platzieren, die auf die Gegner schießen. Bei jedem zusätzlichen Tower verschwindet der zuerst gesetzte Tower wieder
* die Tower können nur an definierten Stellen der Karte platziert werden
* Game-Loop endet, wenn der Spieler mit Hilfe der Tower alle Gegner zerstört hat oder so viele Gegner das Ziel erreichen, dass der Spieler keine Leben mehr hat
* Rectangle Sprite Collsion für Buttons und Matheoperationen
* Per-Pixel-Collision für Weapon und Enemy
* Hauptmenü-Screen mit Optionen (Musik, Schwierigkeitsgrad, Spiel beenden, Anleitung, Impressum)
* ein Tower mit einer Mindest-Range und einer hohen Maximal-Range und ein Tower ohne Mindest-Range und einer geringen Maximal-Range
* Health Bars für Gegner
* verschiedene Gegner Sprites ( random gewählt)
* zusätzlicher Gegner, der mit Mathematik getötet werden muss
* Rechenoperationen -1, +1, /2, Wurzel
