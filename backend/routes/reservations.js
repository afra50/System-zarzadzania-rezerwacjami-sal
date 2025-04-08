const express = require("express");
const router = express.Router();
const db = require("../db");

/**
 * @swagger
 * /api/reservations/occupied/{room_id}:
 *   get:
 *     description: Zwraca zajête terminy dla danej sali.
 *     parameters:
 *       - in: path
 *         name: room_id
 *         required: true
 *         description: ID sali, dla której chcemy pobraæ zajête terminy
 *         schema:
 *           type: integer
 *     responses:
 *       200:
 *         description: Zajête terminy zosta³y zwrócone
 *       500:
 *         description: B³¹d serwera
 */
router.get("/occupied/:room_id", async (req, res) => {
    const { room_id } = req.params;

    try {
        const [results] = await db.query(
            "SELECT start_datetime, end_datetime FROM reservations WHERE room_id = ? AND status != 'cancelled'",
            [room_id]
        );

        res.json(results);
    } catch (err) {
        console.error("B³¹d pobierania zajêtych terminów:", err);
        res.status(500).json({ message: "B³¹d serwera" });
    }
});

/**
 * @swagger
 * /api/reservations/user/{email}:
 *   get:
 *     description: Zwraca rezerwacje dla u¿ytkownika na podstawie emaila
 *     parameters:
 *       - in: path
 *         name: email
 *         required: true
 *         description: E-mail u¿ytkownika
 *         schema:
 *           type: string
 *     responses:
 *       200:
 *         description: Lista rezerwacji
 *       404:
 *         description: U¿ytkownik nie znaleziony
 */
router.get("/user/:email", async (req, res) => {
    const { email } = req.params;

    try {
        // Pobierz id_user na podstawie emaila
        const [userResult] = await db.query("SELECT id_user FROM users WHERE email = ?", [email]);

        if (userResult.length === 0) {
            return res.status(404).json({ message: "U¿ytkownik nie znaleziony!" });
        }

        const user_id = userResult[0].id_user; // Pobieramy id_user

        // Pobierz wszystkie rezerwacje dla tego u¿ytkownika
        const [results] = await db.query(
            "SELECT * FROM reservations WHERE user_id = ?",
            [user_id]
        );

        res.json(results);
    } catch (err) {
        console.error("B³¹d pobierania rezerwacji dla u¿ytkownika:", err);
        res.status(500).json({ message: "B³¹d serwera" });
    }
});

/**
 * @swagger
 * /api/reservations/cancel/{id}:
 *   delete:
 *     description: Anuluje rezerwacjê na podstawie jej ID
 *     parameters:
 *       - in: path
 *         name: id
 *         required: true
 *         description: ID rezerwacji
 *         schema:
 *           type: integer
 *     responses:
 *       200:
 *         description: Rezerwacja anulowana
 *       404:
 *         description: Rezerwacja nie znaleziona
 */
router.delete("/cancel/:id", async (req, res) => {
    const { id } = req.params;

    try {
        const [deleteResult] = await db.query(
            "UPDATE reservations SET status = 'cancelled' WHERE id_reservation = ? AND status != 'cancelled'",
            [id]
        );

        if (deleteResult.affectedRows === 0) {
            return res.status(404).json({ message: "Rezerwacja nie znaleziona lub ju¿ anulowana" });
        }

        res.json({ message: "Rezerwacja anulowana pomyœlnie!" });
    } catch (err) {
        console.error("B³¹d anulowania rezerwacji:", err);
        res.status(500).json({ message: "B³¹d serwera" });
    }
});

/**
 * @swagger
 * /api/reservations/confirm/{id}:
 *   put:
 *     description: Potwierdza rezerwacjê na podstawie jej ID
 *     parameters:
 *       - in: path
 *         name: id
 *         required: true
 *         description: ID rezerwacji
 *         schema:
 *           type: integer
 *     responses:
 *       200:
 *         description: Rezerwacja potwierdzona
 *       404:
 *         description: Rezerwacja nie znaleziona
 *       500:
 *         description: B³¹d serwera
 */
router.put("/confirm/:id", async (req, res) => {
    const { id } = req.params;

    try {
        const [updateResult] = await db.query(
            "UPDATE reservations SET status = 'confirmed' WHERE id_reservation = ? AND status != 'confirmed'",
            [id]
        );

        if (updateResult.affectedRows === 0) {
            return res.status(404).json({ message: "Rezerwacja nie znaleziona lub ju¿ potwierdzona" });
        }

        res.json({ message: "Rezerwacja zosta³a potwierdzona!" });
    } catch (err) {
        console.error("B³¹d potwierdzania rezerwacji:", err);
        res.status(500).json({ message: "B³¹d serwera" });
    }
});

/**
 * @swagger
 * /api/reservations/list:
 *   get:
 *     description: Pobiera wszystkie rezerwacje, które nie s¹ anulowane i których czas zakoñczenia to dziœ lub póŸniej.
 *     parameters:
 *       - in: query
 *         name: room_id
 *         description: ID sali, dla której chcemy pobraæ rezerwacje
 *         schema:
 *           type: integer
 *     responses:
 *       200:
 *         description: Lista rezerwacji
 *       500:
 *         description: B³¹d serwera
 */
router.get("/list", async (req, res) => {
    const { room_id } = req.query; // Pobranie parametru z zapytania GET (filtr po sali)
    const today = new Date();
    today.setHours(0, 0, 0, 0); // Ustalamy pocz¹tek dzisiejszego dnia

    try {
        let query = "SELECT * FROM reservations WHERE end_datetime >= ? AND status != 'cancelled'";
        let params = [today];

        if (room_id) {
            query += " AND room_id = ?";
            params.push(room_id);
        }

        const [results] = await db.query(query, params);
        res.json(results);
    } catch (err) {
        console.error("B³¹d pobierania rezerwacji:", err);
        res.status(500).json({ message: "B³¹d serwera" });
    }
});

/**
 * @swagger
 * /api/reservations/reservations-by-day:
 *   get:
 *     description: Zwraca liczbê rezerwacji w poszczególnych dniach tygodnia.
 *     responses:
 *       200:
 *         description: Liczba rezerwacji wg dnia tygodnia
 *       500:
 *         description: B³¹d serwera
 */
router.get("/reservations-by-day", async (req, res) => {
    try {
        const [results] = await db.query(`
            SELECT 
    		CASE DAYOFWEEK(start_datetime)
        		WHEN 1 THEN 'Niedziela'
        		WHEN 2 THEN 'Poniedzialek'
       			WHEN 3 THEN 'Wtorek'
        		WHEN 4 THEN 'Sroda'
        		WHEN 5 THEN 'Czwartek'
        		WHEN 6 THEN 'Piatek'
        		WHEN 7 THEN 'Sobota'
    		END AS day_name,
    		COUNT(*) as reservation_count
		FROM reservations
		WHERE status = 'confirmed'
		GROUP BY day_name
		ORDER BY FIELD(day_name, 'Poniedzia³ek', 'Wtorek', 'Œroda', 'Czwartek', 'Pi¹tek', 'Sobota', 'Niedziela');
        `);
        res.json(results);
    } catch (err) {
        console.error("B³¹d pobierania rezerwacji wg dnia tygodnia:", err);
        res.status(500).json({ message: "B³¹d serwera" });
    }
});

/**
 * @swagger
 * /api/reservations/reservations-by-month:
 *   get:
 *     description: Zwraca liczbê rezerwacji w ka¿dym miesi¹cu danego roku
 *     parameters:
 *       - in: query
 *         name: year
 *         description: Rok, dla którego chcemy pobraæ liczbê rezerwacji
 *         schema:
 *           type: integer
 *           example: 2025
 *     responses:
 *       200:
 *         description: Liczba rezerwacji wg miesi¹ca
 *       500:
 *         description: B³¹d serwera
 */
router.get("/reservations-by-month", async (req, res) => {
    const year = req.query.year || new Date().getFullYear(); // Domyœlnie bie¿¹cy rok
    try {
        const [results] = await db.query(`
            SELECT 
    		CASE MONTH(start_datetime)
        		WHEN 1 THEN 'Styczen'
        		WHEN 2 THEN 'Luty'
        		WHEN 3 THEN 'Marzec'
        		WHEN 4 THEN 'Kwiecien'
        		WHEN 5 THEN 'Maj'
        		WHEN 6 THEN 'Czerwiec'
        		WHEN 7 THEN 'Lipiec'
        		WHEN 8 THEN 'Sierpien'
        		WHEN 9 THEN 'Wrzesien'
        		WHEN 10 THEN 'Pazdziernik'
        		WHEN 11 THEN 'Listopad'
        		WHEN 12 THEN 'Grudzien'
    		END AS month_name,
    		COUNT(*) as reservation_count
		FROM reservations
		WHERE status = 'confirmed' AND YEAR(start_datetime) = ?
		GROUP BY month_name
		ORDER BY FIELD(month_name, 'Styczeñ', 'Luty', 'Marzec', 'Kwiecieñ', 'Maj', 'Czerwiec', 'Lipiec', 'Sierpieñ', 'Wrzesieñ', 'PaŸdziernik', 'Listopad', 'Grudzieñ');
        `, [year]);
        res.json(results);
    } catch (err) {
        console.error("B³¹d pobierania rezerwacji wg miesi¹ca:", err);
        res.status(500).json({ message: "B³¹d serwera" });
    }
});

/**
 * @swagger
 * /api/reservations:
 *   post:
 *     description: Dodaje now¹ rezerwacjê
 *     requestBody:
 *       required: true
 *       content:
 *         application/json:
 *           schema:
 *             type: object
 *             properties:
 *               room_id:
 *                 type: integer
 *               email:
 *                 type: string
 *               start_datetime:
 *                 type: string
 *                 format: date-time
 *               end_datetime:
 *                 type: string
 *                 format: date-time
 *     responses:
 *       201:
 *         description: Rezerwacja dodana pomyœlnie
 *       400:
 *         description: Brak wymaganych pól
 *       404:
 *         description: U¿ytkownik nie znaleziony
 *       409:
 *         description: Sala jest ju¿ zarezerwowana w podanym terminie
 *       500:
 *         description: B³¹d serwera
 */
router.post("/", async (req, res) => {
    const { room_id, email, start_datetime, end_datetime } = req.body;

    if (!room_id || !email || !start_datetime || !end_datetime) {
        return res.status(400).json({ message: "Wszystkie pola s¹ wymagane!" });
    }

    try {
        // Pobranie id_user na podstawie emaila
        const [userResult] = await db.query("SELECT id_user FROM users WHERE email = ?", [email]);

        if (userResult.length === 0) {
            return res.status(404).json({ message: "U¿ytkownik nie znaleziony!" });
        }

        const user_id = userResult[0].id_user; // ? Poprawiona nazwa kolumny

        // Sprawdzenie dostêpnoœci sali
        const [availability] = await db.query(
            "SELECT * FROM reservations WHERE room_id = ? AND (start_datetime < ? AND end_datetime > ?)",
            [room_id, end_datetime, start_datetime]
        );

        if (availability.length > 0) {
            return res.status(409).json({ message: "Sala jest ju¿ zarezerwowana w podanym terminie!" });
        }

        // Dodanie rezerwacji
        const [insertResult] = await db.query(
            "INSERT INTO reservations (user_id, room_id, start_datetime, end_datetime, status) VALUES (?, ?, ?, ?, 'pending')",
            [user_id, room_id, start_datetime, end_datetime]
        );

        res.status(201).json({ message: "Rezerwacja dodana pomyœlnie!", reservation_id: insertResult.insertId });

    } catch (err) {
        console.error("B³¹d operacji w bazie danych:", err);
        res.status(500).json({ message: "B³¹d serwera" });
    }
});

module.exports = router;
