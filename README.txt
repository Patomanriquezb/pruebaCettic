Demo de Juego para Cettic

Requerimientos completados:
1. Auto sigue un camino, recoge ítems, esquiva enemigos.
2. Auto tiene vida inicial, al volverse 0 muere y juego termina.
3. Camara sigue al auto.
4. Auto tiene un poder especial: Escudo, limitado por cooldown.
5. Enemigos: Gallinas gigantes, tienen cantidad de daño que afecta la vida del jugador.
6. Nivel dura 30 segundos o más dependiendo de los parámetros.
7. Demo desarrollado en 3D.
8. Input de teclado.
9. Items generan un puntaje que se representa durante el juego y al finalizar.
10. Parámetros del juego son modificables en el inspector de Unity, en escena GameScene en objeto GameController.
	a. Velocidad del personaje.
	b. Tiempo de duración del nivel.
	c. Habilitar/Deshabilitar poder especial.
	d. Puntos por ítem.
	e. Vitalidad.
	f. Daño de enemigos.
11. El juego cuenta con una autenticación de usuarios local mediante PlayerPrefs, sin encriptación. Permite creación y autenticación de usuarios.
12. El juego tiene menú principal.

Extras:
- Integración de arte: modelos de auto, calle, postes, nubes, gallina y skybox descargados desde Asset Store.
- Mecánicas extra: cambio de color del auto, enemigos aturden al jugador al chocar.
- UI/UX: interfaces simples que indican lo justo y necesario. Pruebas de usuario y asesoría UX para menú principal y menú de final de juego.
- Estándares de codificación de C#: se utiliza la convención de nomenclatura de C#, alternando camelCase y PascalCase según cada caso.
- Patrones de diseño: se utiliza singleton para el GameController, States para el GameController y CarController. Se usa el modelo basado en Componentes estándar de Unity.