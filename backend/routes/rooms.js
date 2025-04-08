const express = require('express');
const db = require('../db'); // Po³¹czenie z baz¹

const router = express.Router();

/**
 * @swagger
 * /api/rooms/all:
 *   get:
 *     description: Zwraca listê dostêpnych sal
 *     responses:
 *       200:
 *         description: Lista sal
 */
router.get('/all', async (req, res) => {
    try {
        const [rooms] = await db.query(`
            SELECT 
                id_room AS id, 
                name, 
                address, 
                seats, 
                CAST(latitude AS DECIMAL(10,6)) AS latitude, 
                CAST(longitude AS DECIMAL(10,6)) AS longitude, 
                description 
            FROM rooms
        `);

        // Konwersja do JSON
        const formattedRooms = rooms.map(room => ({
            ...room,
            latitude: parseFloat(room.latitude),  // Konwersja string -> liczba
            longitude: parseFloat(room.longitude) // Konwersja string -> liczba
        }));

        res.json(formattedRooms);
    } catch (err) {
        console.error("B³¹d pobierania sal:", err);
        res.status(500).json({ message: "B³¹d serwera" });
    }
});

/**
 * @swagger
 * /api/rooms/update/{id}:
 *   put:
 *     description: Aktualizuje dane sali na podstawie jej ID
 *     parameters:
 *       - in: path
 *         name: id
 *         required: true
 *         description: ID sali do zaktualizowania
 *         schema:
 *           type: integer
 *       - in: body
 *         name: room
 *         description: Nowe dane sali
 *         required: true
 *         schema:
 *           type: object
 *           properties:
 *             name:
 *               type: string
 *             address:
 *               type: string
 *             seats:
 *               type: integer
 *             latitude:
 *               type: number
 *               format: float
 *             longitude:
 *               type: number
 *               format: float
 *             description:
 *               type: string
 *     responses:
 *       200:
 *         description: Sala zosta³a zaktualizowana
 *       404:
 *         description: Sala nie istnieje
 *       500:
 *         description: B³¹d serwera
 */
router.put('/update/:id', async (req, res) => {
    const { id } = req.params;
    const { name, address, seats, latitude, longitude, description } = req.body;

    try {
        const [result] = await db.query(`
            UPDATE rooms 
            SET name = ?, address = ?, seats = ?, latitude = ?, longitude = ?, description = ?
            WHERE id_room = ?
        `, [name, address, seats, latitude, longitude, description, id]);

        if (result.affectedRows > 0) {
            res.json({ message: "Sala zosta³a zaktualizowana" });
        } else {
            res.status(404).json({ message: "Sala nie istnieje" });
        }
    } catch (err) {
        console.error("B³¹d aktualizacji sali:", err);
        res.status(500).json({ message: "B³¹d serwera", error: err.message });
    }
});

/**
 * @swagger
 * /api/rooms/most-booked-rooms:
 *   get:
 *     description: Zwraca 5 najczêœciej wybieranych sal
 *     responses:
 *       200:
 *         description: Lista najczêœciej wybieranych sal
 */
router.get("/most-booked-rooms", async (req, res) => {
    try {
        const [results] = await db.query(`
            SELECT r.id_room AS room_id, r.name AS room_name, COUNT(res.id_reservation) AS booking_count
		FROM reservations res
		JOIN rooms r ON res.room_id = r.id_room
		WHERE res.status = 'confirmed'
		GROUP BY r.id_room, r.name
		ORDER BY booking_count DESC
		LIMIT 5;
        `);
        res.json(results);
    } catch (err) {
        console.error("B³¹d pobierania najczêœciej wybieranych sal:", err);
        res.status(500).json({ message: "B³¹d serwera" });
    }
});


module.exports = router;
