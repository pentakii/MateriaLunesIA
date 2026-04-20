Proyecto (Entrega 1): Dungeon Knight 
Genero
Dungeon Crawler de acción en primera persona y se incluira opcion de tercera persona.
Objetivo del Juego

El jugador asume el papel de un caballero que debe sobrevivir y limpiar las salas de un castillo infectado, derrotando enemigos mediante el combate cuerpo a cuerpo mientras gestiona su propia salud
con pociones.

Controles Básicos
WASD: Movimiento del personaje.
Mouse: Rotación de la cámara (mirar).
Espacio: Saltar.
Clic Izquierdo: Atacar con la espada
.
Sistemas de IA Implementados 
En esta instancia aunque no hay condicion de victoria, el proyecto integra sistemas de percepción y toma de decisiones que permiten comportamientos reales y observables.
:
Línea de Visión (Line of Sight - LoS):
Los enemigos utilizan un sistema de percepción basado en distancia, ángulo de visión y detección de obstáculos mediante Raycast
Esto permite que los agentes pierdan de vista al jugador si este se esconde detrás de muros o columnas del castillo
.
Máquina de Estados Finitos (FSM) - Esqueleto:
Gestiona el comportamiento del Esqueleto mediante 4 estados diferenciables: Patrol (patrulla), Pursuit (persecución), Attack (ataque) y Flee (huida)
.
Las transiciones son dinámicas; por ejemplo, el enemigo huye si su vida es menor al 25% o ataca si la distancia es mínima
.
Árbol de Decisiones (Decision Tree) - Araña de Élite:
Implementa una toma de decisiones jerárquica y más compleja que la FSM
.
La araña evalua constantemente condiciones (¿Veo al jugador? ¿Tengo poca vida? ¿Estoy en rango?) para ejecutar acciones como deambular (Wander), perseguir o atacar
.
Comportamientos de Movimiento (Steering Behaviours):
Se han integrado algoritmos de movimiento local para que los agentes se desplacen en busca del jugador, y asi poder atacarlo.
.
Seek: Para perseguir al jugador
.
Flee: Para escapar cuando la salud es baja
.
Wander: Para que la araña explore el entorno de forma aleatoria y natural

Identidad Visual y Estética
El proyecto mantiene una estética medieval coherente, utilizando modelos de personajes (caballero, esqueletos, arañas) y entornos de mazmorra que responden a una misma lógica visual.
