# System rezerwacji miejsc w kinie
W tej domenie zarządzamy rezerwacjami miejsc na seanse filmowe. Kluczowe pojęcia to:

### MovieTheater (Entity)
Reprezentuje pojedynczą salę kinową z unikalnym identyfikatorem, utrzymuje listę wszystkich dokonanych rezerwacji.

### Reservation (Entity)
Rezerwacja z unikalnym identyfikatorem, przypisana do ~~konkretnego seansu i~~ zestawu miejsc.

### SeatPosition (Value Object)
Para (rzęd, numer), niezmienna, definiuje położenie pojedynczego miejsca.

## Business Rules (reguły biznesowe):

- Między rezerwacjami musi być co najmniej 1 wolne miejsce (dla dystansu społecznego).
- Wszystkie miejsca w ramach jednej rezerwacji muszą być w tym samym rzędzie i ułożone kolejno.
- Rezerwacja nie może być mniejsza niż 1 miejsce i większa niż 5 miejsc.
- Rezerwacja nie może być w przeszłości.
- Rezerwacja nie może być dalej niż 30 dni od dnia dzisiejszego.
- Nie można rezerwować miejsc, które są już zarezerwowane.
- Nie można utworzyć rezerwacji, jeśli nie ma wystarczającej liczby miejsc w rzędzie.
- Każda rezerwacja powinna być unikalna
  - Id rezerwacji jest generowane automatycznie.
  - Nie można mieć dwóch rezerwacji z tym samym id.
  - Jeśli rezerwacja została utworzona nie można jej zminiać.
  
---

**_Hints: Red-Green-Refactor, analiza przypadków, parametryzacja, atrapy, refactoring_**

**Cel**: Zastosować wiedzę teoretyczną i praktyczną do napisania testów jednostkowych do realnego, istniejącego kodu z różnymi wyzwaniami.
- Zidentyfikować kluczowe ścieżki i wartości graniczne.
- Napisać pierwsze testy w stylu R-G-R.
- Zastosować parametryzację.
- Przeprowadzić podstawowy refactoring pod testowalność (np. wyciągnięcie zależności, czysta logika).


---
**Domain Layer**:
 - Dokończyć metodę MovieTheater.Reserve(...) według reguł.
 - Dokonać refaktoryzacji, aby poprawić testowalność w razie potrzeby i po zakończeniu pisania testów. 

**Testy**:
- Pomyślne rezerwacje (różne scenariusze).
- Konflikt miejsc (overlap).
- Brak dystansu: zero przerwy i 1 miejsce gap.
- Scenariusze brzegowe: rząd start/end.
- Mock IMovieTheaterRepository.
- Występowanie wyjątku w przypadku naruszenia reguły domeny.
- **[TestCase]** aby przetestować różne rozmiary rezerwacji (np. 1, 2, 5 miejsc).
- **_Inne scenariusze by pokryć reguły biznesowe w razie potrzeby._**