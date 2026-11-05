# Directivas de Arquitectura para GitHub Copilot

- Utiliza siempre C# moderno (.NET 10).
- Aplica inyección de dependencias estricta a través del constructor.
- Toda API REST debe utilizar controladores convencionales (`ControllerBase`) con rutas en minúsculas y formato kebab-case.
- Documenta todos los métodos públicos y controladores utilizando comentarios XML (`<summary>`).
- En las respuestas HTTP, devuelve códigos de estado apropiados:
  - 200 OK para consultas exitosas.
  - 400 Bad Request cuando una validación falle.
  - 404 Not Found cuando un recurso no exista.