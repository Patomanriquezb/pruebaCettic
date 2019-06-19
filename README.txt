Demo de Juego para Cettic

Requerimientos completados:
1. Auto sigue un camino, recoge �tems, esquiva enemigos.
2. Auto tiene vida inicial, al volverse 0 muere y juego termina.
3. Camara sigue al auto.
4. Auto tiene un poder especial: Escudo, limitado por cooldown.
5. Enemigos: Gallinas gigantes, tienen cantidad de da�o que afecta la vida del jugador.
6. Nivel dura 30 segundos o m�s dependiendo de los par�metros.
7. Demo desarrollado en 3D.
8. Input de teclado.
9. Items generan un puntaje que se representa durante el juego y al finalizar.
10. Par�metros del juego son modificables en el inspector de Unity, en escena GameScene en objeto GameController.
	a. Velocidad del personaje.
	b. Tiempo de duraci�n del nivel.
	c. Habilitar/Deshabilitar poder especial.
	d. Puntos por �tem.
	e. Vitalidad.
	f. Da�o de enemigos.
11. El juego cuenta con una autenticaci�n de usuarios local mediante PlayerPrefs, sin encriptaci�n. Permite creaci�n y autenticaci�n de usuarios.
12. El juego tiene men� principal.

Extras:
- Integraci�n de arte: modelos de auto, calle, postes, nubes, gallina y skybox descargados desde Asset Store.
- Mec�nicas extra: cambio de color del auto, enemigos aturden al jugador al chocar.
- UI/UX: interfaces simples que indican lo justo y necesario. Pruebas de usuario y asesor�a UX para men� principal y men� de final de juego.
- Est�ndares de codificaci�n de C#: se utiliza la convenci�n de nomenclatura de C#, alternando camelCase y PascalCase seg�n cada caso.
- Patrones de dise�o: se utiliza singleton para el GameController, States para el GameController y CarController. Se usa el modelo basado en Componentes est�ndar de Unity.