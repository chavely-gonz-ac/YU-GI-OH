# Torneos de Yu-Gi-Oh

Yu-Gi-Oh! es un juego de cartas coleccionables de origen japonés basado en el juego ficticio de Duelo de Monstruos.

El juego de cartas ha alcanzado tal popularidad que se planifican eventos internacionales en los que jugadores de cualquier edad y nivel pueden competir juntos en torneos y hacer amigos nuevos, ver cartas nuevas y alardear de nuevas estrategias.

Hay eventos de Yu-Gi-Oh! para cualquier clase de Duelista, desde pequeños torneos hasta eventos masivos internacionales donde los mejores Duelistas del mundo juegan para conseguir grandes premios.

---

## Objetivo

Con el objetivo de almacenar y analizar la información generada a partir de los torneos, y facilitar el cálculo de estadísticas de los jugadores, se quiere confeccionar una **base de datos** que sirva de soporte a la realización de torneos del juego de cartas coleccionables Yu-Gi-Oh!.

Debe ser posible:

- Registrar jugadores y los decks (mazos de cartas) que utilizan.
- Crear torneos e inscribir jugadores.
- Llevar a cabo partidas de los torneos hasta su finalización.
- Modelar todo lo que ocurre en los torneos.

---

## Jugadores

De los jugadores se desea guardar:

- Nombre completo
- Municipio
- Provincia
- Teléfono (opcional)
- Dirección

Los jugadores se inscriben en los torneos en los que estén interesados.

---

## Partidas

Una partida de Yu-Gi-Oh! se lleva a cabo entre **dos jugadores**, cada uno usando un **Deck**.

- Las partidas corresponden a un torneo y una ronda específica.
- Son **al mejor de 3** (gana el primero que llegue a 2).
- De cada partida se desea guardar el **resultado** como un par de enteros (ej. `2-1`, `0-2`).

---

## Decks

Un jugador puede poseer varios Decks distintos. De cada Deck se desea conocer:

- Nombre del Deck (ej: "Deck de Exodia", "Six Samurai", "Deck de Daño").
- Cantidad de cartas en el **Main Deck** (entre 40 y 60).
- Cantidad de cartas en el **Side Deck** (entre 0 y 15).
- Cantidad de cartas en el **Extra Deck** (entre 0 y 15).
- Arquetipo.

### Arquetipos

- Las cartas de un mismo arquetipo comparten el inicio de sus nombres.
- Si un Deck tiene más del 50% de cartas de un mismo arquetipo X, entonces se considera de ese arquetipo.
- Si no, se clasifica como **Mixto** (arquetipo adicional).
- La base de datos deberá guardar **todos los arquetipos existentes de Yu-Gi-Oh!** hasta la fecha.

---

## Torneos

De un torneo se desea guardar:

- Nombre del Torneo
- Fecha de Inicio (con hora)
- Dirección (lugar de la competición)

### Inscripción

- Se pueden inscribir **cualquier cantidad de participantes**.
- No se pueden inscribir jugadores después de la fecha de inicio.
- Cada jugador debe seleccionar **uno y solo un Deck** para participar.
- No es posible cambiar de mazo durante el torneo.

### Etapas

- La primera etapa es la **ronda de clasificación** (todos contra todos).
- Después se seleccionan los `2^k` mejores jugadores.
- Se realizan **k rondas eliminatorias** hasta obtener un campeón.
- Los emparejamientos de las eliminatorias se deciden al azar.

---

## Consultas esperadas de la base de datos

1. Los *n* jugadores con más decks en su poder (ordenados de mayor a menor).
2. Los *n* arquetipos más populares entre los jugadores (ordenados de mayor a menor).
3. La provincia/municipio donde es más popular un arquetipo dado.
4. El campeón de un torneo.
5. Los *n* jugadores con más victorias en un intervalo de tiempo.
6. El arquetipo más utilizado por los jugadores en un torneo dado.
7. La cantidad de veces que un arquetipo ha sido el arquetipo del campeón en un conjunto de torneos (en un intervalo de tiempo).
8. La provincia/municipio con más campeones (en un intervalo de tiempo).
9. Dado un torneo y una ronda, los arquetipos más representados (cantidad de jugadores usándolos).
10. Los *n* arquetipos más utilizados en torneos (ordenados de mayor a menor).

---

💡 *Nota*: Se recomienda generalizar cada consulta lo más posible, ya que ayudará a obtener mejores resultados en el análisis.
