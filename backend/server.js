require('dotenv').config();
const express = require('express');
const cors = require('cors');
const db = require('./db'); // Import po³¹czenia z baz¹
const authRoutes = require('./routes/auth'); // Import autoryzacji
const roomsRoutes = require('./routes/rooms'); // Import obs³ugi sal
const reservationsRoutes = require("./routes/reservations"); // Import obs³ugi rezerwacji

// Import Swaggera
const swaggerJsdoc = require("swagger-jsdoc");
const swaggerUi = require("swagger-ui-express");

const app = express();
app.use(cors());
app.use(express.json());

// Konfiguracja Swaggera
const options = {
  definition: {
    openapi: "3.0.0", // wersja OpenAPI
    info: {
      title: "Rezerwacje Sal API", // Nazwa projektu
      version: "1.0.0", // Wersja API
      description: "API do zarz¹dzania rezerwacjami sal konferencyjnych", // Opis projektu
    },
  },
  apis: ["./routes/*.js"], // Wskazuje pliki z definicjami Swaggera (jeœli u¿ywasz komentarzy)
};

// Generowanie specyfikacji Swaggera
const swaggerSpec = swaggerJsdoc(options);

// Dodanie routingu do Swagger UI
app.use("/api-docs", swaggerUi.serve, swaggerUi.setup(swaggerSpec));

// Endpoint testowy
app.get("/", (req, res) => {
  res.send("API dla aplikacji WPF dzia³a!");
});

// Pod³¹czenie routera dla logowania i rejestracji
app.use('/api/auth', authRoutes);

// Pod³¹czenie routera dla sal
app.use('/api/rooms', roomsRoutes);

// Pod³¹czenie routera dla rezerwacji
app.use("/api/reservations", reservationsRoutes);

const PORT = process.env.PORT || 5001;
app.listen(PORT, '0.0.0.0', () => {
  console.log(`Serwer dzia³a na porcie ${PORT}`);
});
