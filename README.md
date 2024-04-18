1. Make sure docker deamon is running
2. docker-compose up in solution root directory
3. Open localhost:8085

REST API w .NET 8 i C#, wewnętrznie oparte o listę tagów dostarczanych przez API StackOverflow (https://api.stackexchange.com/docs). Założenia projektu:

1. pobrać min. 1000 tagów z API SO do lokalnej bazy danych lub innego trwałego cache
2. pobrane może nastąpić na starcie lub przy pierwszym żądaniu, od razu w całości lub stopniowo tylko brakujących danych
3. obliczyć procentowy udział tagów w całej pobranej populacji (źródłowe pole count, odpowiednio przeliczone)
4. udostępnić tagi poprzez stronicowane API z opcją sortowania po nazwie i udziale w obu kierunkach
5. udostępnić metodę API do wymuszenia ponownego pobrania tagów z SO
6. udostępnić definicję OpenAPI przygotowanych metod API
7. uwzględnić logowanie oraz obsługę błędów i konfigurację uruchomieniową usługi
8. przygotować kilka wybranych testów jednostkowych wewnętrznych usług implementacji
9. przygotować kilka wybranych testów integracyjnych opartych o udostępniane API
10. wykorzystać konteneryzację do zapewnienia powtarzalnego budowania i uruchamiania projektu
11. rozwiązanie opublikować w repozytorium GitHub
12. całość powinna się uruchamiać po wykonaniu wyłącznie polecenia "docker compose up"
