# Requerimientos

## Objetivos

Almacenar y analizar la información generada a partir de los torneos, y facilitar el cálculo de  estadísticas de  los jugadores.

Confeccionar una base de datos que sirva de soporte a la realización de torneos.

## Funcionalidades

* Registrar jugadores.
* Rgistrar decks.
* Crear torneos.
* Inscribir varios jugadores en los torneos. Los jugadores se pueden inscribir ellos mismos.
* En un torneo se pueden inscribir cualquier cantidad de participantes; no se pueden inscribir participantes depués de la fecha de inicio.
* La primera etapa de un torneo es la clasificación, donde se enfrentan todos contra todos y se eligen los 2^k con más victorias.
* Se realizan k rondas eliminatorias hasta obtener un ganador (semifinales, octavos de final, cuartos de final y la gran final).
* Llevar a cabo las partidas de los torneos hasta su finalización.
* Modelar todo lo que ocurre en los torneos.

## Entidades

* **Jugador**: nombre completo; municipio; provincia; teléfono; dirección. Pude poseer varios decks
* **Deck**: nombre; cantidad de cartas del main deck (número entre 40 y 60); cantidad de cartas del side deck (numero entre 0 y 15); cantidad de cartas del extra deck (numero entre 0 y 15); arquetipo (si más de la mitad de las cartas del deck son del mismo arquetipo, entonces el deck es de ese arquetipo, si no su arquetipo es Mixto; es requerido al crear el deck y debe seleccionarse de los arquetipos existentes).
* **Arquetipo**: las cartas de un mismo arquetipo cmparten el inicio de sus nombres. La base de datos debe almacenar todos los arquetipos existentes hasta la fecha.
* **Torneo**: nombre; fecha de inicio (hora incluida); dirección. Se divide en rondas o etapas.
* **Ronda de clasificación**: Se enfrentan todos contra todos en un match.
* **Ronda eliminatoria**: El jugador que pierde queda eliminado del torneo. Los emparejamientos se eligen al azar. Se realiza entre 2^x jugadores.
* **Partidas (Matches)**: se lleva a cabo entre dos jugadores, cada jugador usa un solo deck seleccionado por él para el presente torneo; se corresponde a un torneo y a una ronda en específico; resultado (es de 3 a ganar 2) par de enteros.
* En un torneo dado un jugador solo podrá utilizar un deck.

## Estadisticas

1. Los 𝑛 jugadores con más decks en su poder (ordenados de mayor a menor).
2. Los 𝑛 arquetipos más populares entre los jugadores (ordenados de mayor a menor).
3. La provincia/municipio donde es más popular (más jugadores lo usan) un arquetipo dado.
4. El campeón de un torneo.
5. Los 𝑛 jugadores  con  más  victorias  (en  un  intervalo  de  tiempo,  ordenados  de  mayor  a menor).
6. El arquetipo más utilizado por los jugadores en un torneo dado.
7. La cantidad de veces que los arquetipos que han sido el arquetipo del campeón en un grupo de torneos torneos(en un intervalo de tiempo).
8. La Provincia/Municipio con más campeones (en un intervalo de tiempo).
9. Dado un torneo y una ronda, cuál o cuáles son los arquetipos más representados (cantidad de jugadores usándolos).
10. Los n arquetipos más  utilizados  por  al  menos  un  jugador  en  los  torneos(ordenados  de mayor a menor).
