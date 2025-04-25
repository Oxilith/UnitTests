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
  ~~- Rezerwacja nie może być mniejsza niż 1 miejsce i większa niż 5 miejsc w rzędzie.~~
  ~~- Rzędy są numerowane od 1 do n (n<10)~~.
  ~~- Rząd 1 jest najkrótszy, rząd n jest najdłuższy.~~
  ~~- Każdy kolejny rząd musi być dłuższy od poprzedniego.~~
- Pierwszy rzad musi mieć numer 1.
- Każdy kolejny rząd musi miec numer o 1 większy od poprzedniego.
  ~~- Nie można dodać dwóch lub więcej rzędów o tym samym numerze do sali kinowej.~~
- Rzędy są immutable.
- Rezerwacja nie może być w przeszłości.
- Rezerwacja nie może być dalej niż 30 dni od dnia dzisiejszego.
- Nie można rezerwować miejsc, które są już zarezerwowane. ??
  ~~- Nie można utworzyć rezerwacji, jeśli nie ma wystarczającej liczby miejsc w rzędzie.~~
- Każda rezerwacja powinna być unikalna
    - Id rezerwacji jest generowane automatycznie.
    - Czas trwania rezerwacji jest przypisany do rezerwacji
    - Czas utworzenia rezerwacji jest generowane automatycznie.
    - Czas trwania rezerwacji jest przekazywany przez użytkownika.
    - Nie można mieć dwóch rezerwacji z tym samym id.
    - Jeśli rezerwacja została utworzona nie można jej zminiać.

---

**_Hints: Red-Green-Refactor, analiza przypadków, parametryzacja, atrapy, refactoring_**

**Cel**: Zastosować wiedzę teoretyczną i praktyczną do napisania testów jednostkowych do realnego, istniejącego kodu z
różnymi wyzwaniami.

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