const express = require('express');
const bcrypt = require('bcryptjs');
const db = require('../db'); // Import polaczenia z baza

const router = express.Router();

/**
 * @swagger
 * /api/auth/register:
 *   post:
 *     description: Rejestruje nowego u¿ytkownika w systemie.
 *     requestBody:
 *       required: true
 *       content:
 *         application/json:
 *           schema:
 *             type: object
 *             properties:
 *               name:
 *                 type: string
 *                 description: Imiê i nazwisko u¿ytkownika
 *               email:
 *                 type: string
 *                 description: E-mail u¿ytkownika
 *               password:
 *                 type: string
 *                 description: Has³o u¿ytkownika
 *               role:
 *                 type: string
 *                 description: Rola u¿ytkownika (np. "client", "admin")
 *     responses:
 *       201:
 *         description: Rejestracja zakoñczona sukcesem
 *       400:
 *         description: E-mail jest ju¿ u¿ywany lub brak wymaganych pól
 *       500:
 *         description: B³¹d serwera
 */
router.post('/register', async (req, res) => {
    const { name, email, password, role } = req.body;

    if (!name || !email || !password || !role) {
        return res.status(400).json({ message: "Wszystkie pola sa wymagane" });
    }

    try {
        console.log("Sprawdzanie, czy e-mail juz istnieje...");
        const [existingUser] = await db.query("SELECT * FROM users WHERE `email` = ?", [email]);
        
        if (existingUser.length > 0) {
            console.error("E-mail juz istnieje:", email);
            return res.status(400).json({ message: "E-mail jest juz uzywany" });
        }

        console.log("Hashowanie hasla...");
        const hashedPassword = await bcrypt.hash(password, 10);

        console.log("Wstawianie uzytkownika do bazy...");
        await db.query("INSERT INTO users (name, `email`, password, role) VALUES (?, ?, ?, ?)", 
            [name, email, hashedPassword, role]);

        console.log("Uzytkownik zarejestrowany:", email);
        res.status(201).json({ message: "Rejestracja zakonczona sukcesem!" });
    } catch (err) {
        console.error("Blad serwera:", err.message);
        res.status(500).json({ message: "Blad serwera", error: err.message });
    }
});

/**
 * @swagger
 * /api/auth/login:
 *   post:
 *     description: Loguje u¿ytkownika na podstawie jego e-maila i has³a.
 *     requestBody:
 *       required: true
 *       content:
 *         application/json:
 *           schema:
 *             type: object
 *             properties:
 *               email:
 *                 type: string
 *                 description: E-mail u¿ytkownika
 *               password:
 *                 type: string
 *                 description: Has³o u¿ytkownika
 *     responses:
 *       200:
 *         description: Zalogowano pomyœlnie
 *         content:
 *           application/json:
 *             schema:
 *               type: object
 *               properties:
 *                 message:
 *                   type: string
 *                   example: "Zalogowano pomyœlnie"
 *                 user:
 *                   type: object
 *                   properties:
 *                     id:
 *                       type: integer
 *                       description: ID u¿ytkownika
 *                     name:
 *                       type: string
 *                       description: Imiê u¿ytkownika
 *                     email:
 *                       type: string
 *                       description: E-mail u¿ytkownika
 *                     role:
 *                       type: string
 *                       description: Rola u¿ytkownika
 *       400:
 *         description: Brak wymaganych pól
 *       401:
 *         description: Nieprawid³owy e-mail lub has³o
 *       500:
 *         description: B³¹d serwera
 */
router.post('/login', async (req, res) => {
    const { email, password } = req.body;

    if (!email || !password) {
        return res.status(400).json({ message: "Wszystkie pola sa wymagane" });
    }

    try {
        console.log("Wyszukiwanie uzytkownika w bazie...");
        const [user] = await db.query("SELECT * FROM users WHERE `email` = ?", [email]);
        
        if (user.length === 0) {
            console.error("Nieprawidlowy e-mail lub haslo:", email);
            return res.status(401).json({ message: "Nieprawidlowy e-mail lub haslo" });
        }

        console.log("Sprawdzanie hasla...");
        const validPassword = await bcrypt.compare(password, user[0].password);
        if (!validPassword) {
            console.error("Bledne haslo dla uzytkownika:", email);
            return res.status(401).json({ message: "Nieprawidlowy e-mail lub haslo" });
        }

        console.log("Uzytkownik zalogowany:", email);
        res.json({
            message: "Zalogowano pomyslnie",
            user: {
                id: user[0].id,
                name: user[0].name,
                email: user[0]['email'],
                role: user[0].role
            }
        });
    } catch (err) {
        console.error("Blad serwera:", err.message);
        res.status(500).json({ message: "Blad serwera", error: err.message });
    }
});

module.exports = router;
