# System rezerwacji sal konferencyjnych

Jako użytkownik, chcę zarezerwować salę konferencyjną na wybraną godzinę, aby móc zorganizować spotkanie, pod warunkiem że sala jest dostępna i rezerwacja nie koliduje z inną.

---
**Business Rules:**

- Nie można zarezerwować sali w przeszłości.
- Nie można zarezerwować sali, jeśli pokrywa się z inną rezerwacją (to znaczy, że kolejna rezerwacja musi zacząć się po zakończeniu poprzedniej).
- Użytkownik może wybrac czas trwania rezerwacji w przedziale od 30 minut do 90 minut wlącznie.
- Między rezerwacjami musi być przynajmniej 15 minut przerwy*.

---
**Tests:**

- Rezerwacja poprawna.
- Próba rezerwacji w przeszlości.
- Próba rezerwacji zarezerwowanej sali.
- Time period start date musi być mniejszy od end date.
- Przypadki brzegowe (edge cases)
  - Ktoś próbuje zarezerwować sale dokładnie o godzinie, o której poprzednia rezerwacja sie kończy.
  - Czas trwania spotkania mniejszy od 30 minut. (3 asserty)
  - Czas trwania spotkania większy od 90 minut. (3 asserty)
  - Ktoś próbuje zarezerwować salę w tej sekundzie (DateTime.UtcNow())