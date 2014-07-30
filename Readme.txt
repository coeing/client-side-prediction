Client Side Prediction Beispielprogramm
***************************************

Steuerung
---------

Pfeiltasten (links|rechts): Die Spielfigur auf dem Client bewegen
P: Prediction ein-/ausschalten
R: Reconciliation ein-/ausschalten

Szenarien
---------

1. Keine Prediction/Reconciliation

Beim Programmstart sind sowohl Prediction als auch Reconciliation ausgeschaltet. Beim Bewegen der Spielfigur fällt sofort die Verzögerung bei der Bewegung auf dem Client auf. Dabei beträgt der eingestellte Lag lediglich 150 ms.

2. Prediction an, Reconciliation aus

Wird die Prediction eingeschaltet, die Reconciliation jedoch ausgeschaltet, bewegt sich die Spielfigur sofort nach Tastendruck. Sie springt allerdings zurück, sobald das erste Update vom Server eintrifft.

3. Prediction und Reconciliation an

Sind sowohl Prediction als auch Reconciliation angeschaltet, bewegt sich die Spielfigur auf dem Client sofort auf Tastendruck und bleibt auch im Folgenden direkt ansprechbar und dem Server voraus. Aus Spielersicht gibt es keinen Unterschied mehr zwischen dem Spielen lokal und online.