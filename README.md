# ZSW_CZW_1015_InteligentyBudynek
Projekt realizowany w ramach zajęć Zastosowania Systemów Wbudowanych na Politechnice Wrocławskiej

###Cel projektu
Celem  jest zaprojektowanie oraz stworzenie systemu umożliwiającego zarządzanie komponentami inteligentnego budynku. Zadaniem automatyki budynkowej jest integrowanie instalacji występujących na obiekcie. System spaja wszystkie systemy w jedną całość, która pozwala efektywnie i w sposób oszczędny zarządzać całym obiektem z jednego miejsca oraz kontrolować parametry pracy poszczególnych urządzeń a także informować o problemach i awariach. Systemy te udostępniają zazwyczaj interfejs graficzny, który w czytelny sposób pozwala na podgląd parametrów pracy oraz zmianę wartości nastawionych. 
  
###Zawartość projektu
- Aplikacja webowa  
> SysWbudProject.sln  

- Aplikacja mobilna
>  WPHONE\SmartHomeWP.sln

- Schemat bazy danych

> DATABASE\SmartHomeProjectModel.pdf

##Zaimplementowane funkcjonalności
- tworzenie obiektów: budynek, piętro, pokój, urządzenie, telefon, użytkownik,
edycja obiektów: budynek, piętro, pokój, urządzenie, telefon, użytkownik,
monitorowanie obiektów: budynek, piętro, pokój, urządzenie, telefon, użytkownik,
usuwanie obiektów: budynek, piętro, pokój, urządzenie, telefon, użytkownik,
monitorowanie zdarzeń zachodzących w systemie.
  -Aplikacja internetowa pozwala użytkownikowi na sterowanie i monitorowanie stanu poszczególnych urządzeń podłączonych do inteligentnego budynku.

- Aplikacja internetowa odświeża dane po ich zmianie w aplikacji mobilnej.

- Aplikacja internetowa monitoruje połączenie z modułami sprzętowymi i wyświetla odpowiedni komunikat w przypadku zerwania połączenia.

- Aplikacja internetowa monitoruje połączenie z bazą danych i wyświetla odpowiedni komunikat w przypadku błędu połączenia.

- Aplikacja internetowa zapisuje zdarzenia w systemie do bazy danych.

- Aplikacja internetowa pozwala filtrować urządzenia.

- Aplikacja mobilna monitoruje połączenie z serwerem. W przypadku braku połączenia wyświetlany jest odpowiedni komunikat.

- Aplikacja mobilna pozwala filtrować urządzenia.

- Aplikacja mobilna umożliwia użytkownikowi sterowanie i monitorowanie stanu poszczególnych urządzeń podłączonych do inteligentnego budynku.

- Aplikacja mobilna odświeża dane po ich zmianie w aplikacji internetowej.

- Moduły sprzętowe sygnalizują odebranie i wysłanie informacji z oraz do serwera.

###Narzędzia programistyczne

Do poprawnego edytowania projektu potrzebne są następujące programy:
> - Visual Studio 2015( lub 2013) - min instalacyjne to projekty związane z C# + osoba robiąca mobilna apk doinstalowuje Windows Phone emulator. Wygodne sa tez pluginy do gita.
> - MS SQL Server 2014

Powyższe programy dostępne są do pobrania dla studentów W4 [TUTAJ](https://e5.onthehub.com/WebStore/OfferingsOfMajorVersionList.aspx?pmv=769faff4-d124-e511-940e-b8ca3a5db7a1&cmi_mnuMain=bdba23cf-e05e-e011-971f-0030487d8897&ws=98c060e9-b28b-e011-969d-0030487d8897&vsro=8)

---
